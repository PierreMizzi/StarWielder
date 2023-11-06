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
			onTriggerEnter2D = (GameObject other) => { };

			m_circleCollider = GetComponent<CircleCollider2D>();
			m_currentEnergy = m_settings.baseEnergy;

			InitializeStates();
		}

		private void Start()
		{
			if (m_gameChannel.onGameOver != null)
				m_gameChannel.onGameOver += CallbackGameOver;

			m_gameChannel.onSetHighestEnergy.Invoke(m_currentEnergy);
		}

		protected void Update()
		{
			UpdateState();
		}

		private void OnDestroy()
		{
			if (m_gameChannel.onGameOver != null)
				m_gameChannel.onGameOver -= CallbackGameOver;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (isOnShip)
				return;

			if (UtilsClass.CheckLayer(m_enemyLayer.value, other.gameObject.layer))
				CollideWithEnemy(other);

			if (UtilsClass.CheckLayer(m_obstacleFilter.layerMask.value, other.gameObject.layer))
				onTriggerEnter2D.Invoke(other.gameObject);
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
			transform.SetParent(ship.starAnchor);
			transform.localPosition = Vector2.zero;
			transform.localRotation = Quaternion.identity;
		}

		#endregion

		#region Speed

		public float currentSpeed => m_settings.baseSpeed + m_currentEnergy * m_settings.speedFromEnergyRatio;

		private float m_currentSquish;
		private Vector3 m_squishFromSpeed;

		public void ManageSquish()
		{
			m_currentSquish = 1f - ((currentSpeed / m_settings.baseSpeed) * m_settings.squishRatio);
			m_squishFromSpeed.Set(m_currentSquish, 1, 1);
			transform.localScale = m_squishFromSpeed;
		}

		public void ResetSquish()
		{
			transform.localScale = Vector3.one;
		}

		#endregion

		#region Energy

		private float m_currentEnergy;
		public float currentEnergy { get { return m_currentEnergy; } set { m_currentEnergy = Mathf.Max(0f, value); } }

		private float m_highestEnergy;

		public bool hasEnergy => m_currentEnergy > 0;

		#endregion

		#region Obstacle

		[Header("Obstacle")]
		[SerializeField] private ContactFilter2D m_obstacleFilter;
		public ContactFilter2D obstacleFilter => m_obstacleFilter;

		private CircleCollider2D m_circleCollider;
		public CircleCollider2D circleCollider => m_circleCollider;

		public GameObjectDelegate onTriggerEnter2D;

		#endregion

		#region Enemy

		[Header("Enemy")]
		[SerializeField] private LayerMask m_enemyLayer;

		private int m_currentCombo = 1;
		public int currentCombo { get { return m_currentCombo; } set { m_currentCombo = value; } }

		private void CollideWithEnemy(Collider2D other)
		{
			Enemy enemy;
			if (other.gameObject.TryGetComponent(out enemy))
			{
				m_currentCombo += 1;
				m_playerChannel.onRefreshStarCombo.Invoke(m_currentCombo);
				PlayStarComboSFX();

				m_currentEnergy += enemy.energy + ComputeComboBonusEnergy(enemy.energy);

				if (m_currentEnergy > m_highestEnergy)
				{
					m_gameChannel.onSetHighestEnergy.Invoke(m_currentEnergy);
					m_highestEnergy = m_currentEnergy;
				}

				m_playerChannel.onRefreshStarEnergy.Invoke(m_currentEnergy);
				Destroy(enemy.gameObject);
			}
		}

		private float ComputeComboBonusEnergy(float gainedEnergy)
		{
			if (m_currentCombo == 1)
				return 0;
			else
				return gainedEnergy * m_currentCombo * m_settings.comboBonusEnergyRatio;
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

	}
}