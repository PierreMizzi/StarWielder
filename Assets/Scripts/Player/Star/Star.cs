using System;
using System.Collections.Generic;
using PierreMizzi.Extensions.CursorManagement;
using PierreMizzi.SoundManager;
using PierreMizzi.Useful;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PierreMizzi.Gameplay.Players
{

	public class Star : MonoBehaviour, IStateMachine
	{

		#region Channels

		[Header("Channels")]
		[SerializeField] public PlayerChannel m_playerChannel;
		[SerializeField] private GameChannel m_gameChannel;
		[SerializeField] private CameraChannel m_cameraChannel;
		[SerializeField] private CursorChannel m_cursorChannel;

		public PlayerChannel playerChannel => m_playerChannel;
		public GameChannel gameChannel => m_gameChannel;
		public CameraChannel cameraChannel => m_cameraChannel;
		public CursorChannel cursorChannel => m_cursorChannel;

		#endregion

		#region Main

		[Header("Main")]
		[SerializeField] private StarSettings m_settings;
		public StarSettings settings => m_settings;

		public InputActionReference m_mouseClickAction;
		public InputActionReference mouseClickAction => m_mouseClickAction;

		private void CallbackGameOver(GameOverReason reason)
		{
			ChangeState(StarStateType.Idle);
		}

		#endregion

		#region StateMachine

		[Header("State Machine")]
		[SerializeField] private StarStateType m_initialState = StarStateType.Docked;
		public List<AState> states { get; set; } = new List<AState>();
		public AState currentState { get; set; }

		public virtual void InitializeStates()
		{
			states = new List<AState>()
			{
				new StarStateIdle(this),
				new StarStateDocked(this),
				new StarStateFree(this),
				new StarStateReturning(this),
				new StarStateTransfering(this),
			};

			ChangeState(m_initialState);
		}

		public void ChangeState(StarStateType nextState, StarStateType previousState = StarStateType.None)
		{
			ChangeState((int)previousState, (int)nextState);
		}

		public void ChangeState(int previousState, int nextState)
		{
			currentState?.Exit();

			currentState = states.Find((AState newState) => newState.type == nextState);
			if (currentState != null)
				currentState.Enter(previousState);
			else
			{
				Debug.LogError($"Couldn't find a new state of type : {nextState}. Going Inactive");
			}
		}

		public void UpdateState()
		{
			currentState?.Update();
		}

		#endregion

		#region MonoBehaviour

		protected void Awake()
		{
			onCollisionEnter2D = (Collision2D other) => { };
			onTriggerEnter2D = (Collider2D other) => { };

			m_circleCollider = GetComponent<CircleCollider2D>();
			m_rigidbody = GetComponent<Rigidbody2D>();
			m_animator = GetComponent<Animator>();

			m_currentEnergy = m_settings.baseEnergy;

			InitializeStates();
		}

		private void Start()
		{
			if (m_gameChannel.onGameOver != null)
				m_gameChannel.onGameOver += CallbackGameOver;

			m_gameChannel.onSetHighestEnergy.Invoke(m_currentEnergy);

			m_rigidbody.AddForce(transform.up * 10, ForceMode2D.Impulse);
		}

		protected void Update()
		{
			UpdateState();
			ManageScaleFromVelocity();
		}

		private void OnDestroy()
		{
			if (m_gameChannel.onGameOver != null)
				m_gameChannel.onGameOver -= CallbackGameOver;
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			onCollisionEnter2D.Invoke(other);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (isOnShip)
				return;

			onTriggerEnter2D.Invoke(other);

			if (UtilsClass.CheckLayer(m_enemyLayer.value, other.gameObject.layer))
				AbsorbEnemyStar(other);
		}

		#endregion

		#region Physic

		public void UpdateRotationFromVelocity()
		{
			if (rigidbody.velocity != Vector2.zero)
				transform.up = rigidbody.velocity.normalized;
		}

		#endregion

		#region Ship

		[Header("Ship")]
		[SerializeField] private Ship m_ship;
		public Ship ship => m_ship;

		[HideInInspector]
		public bool isOnShip;

		public void SetFree()
		{
			isOnShip = false;
			transform.SetParent(null);
		}

		public void SetOnShip()
		{
			isOnShip = true;

			m_rigidbody.velocity = Vector2.zero;

			transform.SetParent(ship.starAnchor);
			transform.localPosition = Vector2.zero;
			transform.localRotation = Quaternion.identity;
		}

		#endregion

		#region Speed

		public float currentSpeed => m_settings.baseSpeed + m_currentEnergy * m_settings.speedFromEnergyRatio;

		private float m_scaleFromVelocity;
		private Vector3 m_localScaleFromVelocity;

		public void ManageScaleFromVelocity()
		{
			if (m_rigidbody.velocity == Vector2.zero)
				transform.localScale = Vector3.one;
			else
			{
				m_scaleFromVelocity = 1f - m_rigidbody.velocity.sqrMagnitude * m_settings.squishRatio;
				m_scaleFromVelocity = Mathf.Clamp(m_scaleFromVelocity, m_settings.minScaleFromVelocity, 1f);
				m_localScaleFromVelocity.Set(m_scaleFromVelocity, 1, 1);
				transform.localScale = m_localScaleFromVelocity;
			}
		}

		#endregion

		#region Energy

		private float m_currentEnergy;
		public float currentEnergy { get { return m_currentEnergy; } set { m_currentEnergy = Mathf.Max(0f, value); } }
		private float m_highestEnergy;

		private int m_currentCombo = 1;
		public int currentCombo { get { return m_currentCombo; } set { m_currentCombo = value; } }

		private float ComputeComboBonusEnergy(float gainedEnergy)
		{
			if (m_currentCombo == 1)
				return 0;
			else
				return gainedEnergy * m_currentCombo * m_settings.comboBonusEnergyRatio;
		}

		#endregion

		#region Obstacle

		[Header("Obstacle")]
		[SerializeField] private ContactFilter2D m_obstacleFilter;
		public ContactFilter2D obstacleFilter => m_obstacleFilter;

		private CircleCollider2D m_circleCollider;
		private Rigidbody2D m_rigidbody;
		public CircleCollider2D circleCollider => m_circleCollider;
		public new Rigidbody2D rigidbody => m_rigidbody;

		public Collision2DDelegate onCollisionEnter2D;
		public Collider2DDelegate onTriggerEnter2D;

		#endregion

		#region EnemyStar

		[Header("EnemyStar")]
		[SerializeField] private LayerMask m_enemyLayer;

		private void AbsorbEnemyStar(Collider2D other)
		{
			if (other.gameObject.TryGetComponent(out Enemy enemyStar))
			{
				PlayStarComboSFX();
				m_currentCombo += 1;
				m_playerChannel.onRefreshStarCombo.Invoke(m_currentCombo);

				m_currentEnergy += enemyStar.energy + ComputeComboBonusEnergy(enemyStar.energy);
				m_playerChannel.onAbsorbEnemyStar.Invoke(m_currentEnergy);

				m_animator.SetTrigger(k_triggerAbsorb);

				if (m_currentEnergy > m_highestEnergy)
				{
					m_gameChannel.onSetHighestEnergy.Invoke(m_currentEnergy);
					m_highestEnergy = m_currentEnergy;
				}

				m_playerChannel.onRefreshStarEnergy.Invoke(m_currentEnergy);
				Destroy(enemyStar.gameObject);
			}
		}

		#region Audio

		[SerializeField] private SoundSource m_soundSource;
		private const float k_basePitchValue = 1f;
		private const string k_pitchParameterName = "EnemyStarPitch";

		private void PlayStarComboSFX()
		{
			float pitch = k_basePitchValue + m_currentCombo * m_settings.pitchShift;
			pitch = Mathf.Clamp(pitch, 0, m_settings.maxPitch);
			m_soundSource.audioMixerGroup.audioMixer.SetFloat(k_pitchParameterName, pitch);
			m_soundSource.Play();
		}

		#endregion

		#endregion

		#region Animations

		private Animator m_animator;

		private const string k_triggerAbsorb = "Absorb";

		#endregion

	}
}