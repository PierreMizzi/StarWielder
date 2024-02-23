using System.Collections.Generic;
using PierreMizzi.SoundManager;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarWielder.Gameplay.Player
{

	/// <summary>
	/// Star's class for all movement and gameplay behaviours
	/// </summary>
	public class Star : MonoBehaviour, IStateMachine
	{

		#region Channels

		[Header("Channels")]
		[SerializeField] public PlayerChannel m_playerChannel;
		[SerializeField] private GameChannel m_gameChannel;

		public PlayerChannel playerChannel => m_playerChannel;
		public GameChannel gameChannel => m_gameChannel;

		#endregion

		#region Main

		[Header("Main")]
		[SerializeField] private StarSettings m_settings;
		public StarSettings settings => m_settings;

		[SerializeField] private InputActionReference m_mouseClickAction;
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
				new StarStateLocked(this),
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

			m_bounceSoundIDS = new List<string>()
			{
				SoundDataID.STAR_BOUNCE_01,
				SoundDataID.STAR_BOUNCE_02,
			};

			m_currentEnergy = m_settings.baseEnergy;
			// SetVelocityFromEnergy();

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
			CheckEnergy();
			ManageScaleFromVelocity();
		}

		private void OnDestroy()
		{
			if (m_gameChannel.onGameOver != null)
				m_gameChannel.onGameOver -= CallbackGameOver;
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			PlayBounceSound();

			onCollisionEnter2D.Invoke(other);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (isOnShip)
				return;

			onTriggerEnter2D.Invoke(other);

			if (other.gameObject.TryGetComponent(out StarAbsorbable absorbable))
				AbsorbEnergy(absorbable);
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
			m_circleCollider.enabled = true;
			m_playerChannel.onStarFree.Invoke();
		}

		public void SetDocked()
		{
			isOnShip = true;

			m_rigidbody.velocity = Vector2.zero;

			m_circleCollider.enabled = false;

			transform.SetParent(ship.starAnchor);
			transform.localPosition = Vector2.zero;
			transform.localRotation = Quaternion.identity;

			m_playerChannel.onStarDocked.Invoke();
		}

		#endregion

		#region Speed

		public float currentSpeed => m_settings.baseSpeed; //+ m_currentEnergy * m_settings.speedFromEnergyRatio;

		private float m_scaleFromVelocity;
		private Vector3 m_localScaleFromVelocity;

		/// <summary>
		/// The faster the Sun, the more it's squished by it's velocity
		/// </summary>
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
		public float currentEnergy
		{
			get { return m_currentEnergy; }
			set
			{
				m_currentEnergy = Mathf.Max(0f, value);
				playerChannel.onRefreshStarEnergy.Invoke(m_currentEnergy);
			}
		}

		private void CheckEnergy()
		{
			if (m_currentEnergy <= 0f)
				gameChannel.onGameOver.Invoke(GameOverReason.StarDied);
		}

		#endregion

		#region Physics

		[Header("Physics")]
		[SerializeField] private ContactFilter2D m_obstacleFilter;
		private Rigidbody2D m_rigidbody;

		public ContactFilter2D obstacleFilter => m_obstacleFilter;
		public new Rigidbody2D rigidbody => m_rigidbody;

		private CircleCollider2D m_circleCollider;
		public Collision2DDelegate onCollisionEnter2D;
		public Collider2DDelegate onTriggerEnter2D;

		private List<string> m_bounceSoundIDS = new List<string>();

		public void SetVelocityFromEnergy()
		{
			m_rigidbody.velocity = transform.up * currentSpeed;
		}

		public void UpdateRotationFromVelocity()
		{
			if (rigidbody.velocity != Vector2.zero)
				transform.up = rigidbody.velocity.normalized;
		}

		private void PlayBounceSound()
		{
			SoundManager.PlayRandomSFX(m_bounceSoundIDS);
		}

		#endregion

		#region StarAbsorbable

		[Header("EnemyStar")]
		[SerializeField] private LayerMask m_enemyLayer;

		private void AbsorbEnergy(StarAbsorbable absorbable)
		{
			// TODO : 🟥 Use this again 
			PlayEnemyStarPitch();
			absorbable.onAbsorb.Invoke();

			// Energy
			m_currentEnergy += absorbable.energy;
			m_playerChannel.onAbsorbEnemyStar.Invoke(m_currentEnergy);

			// Animation
			m_animator.SetTrigger(k_triggerAbsorb);
		}

		#region Audio

		[SerializeField] private SoundSource m_soundSource;
		private const float k_basePitchValue = 1f;
		private const string k_pitchParameterName = "EnemyStarPitch";

		/// <summary>
		/// The higher the combo, the higher the pitch when absorbing an EnemyStar
		/// </summary>
		private void PlayEnemyStarPitch()
		{
			float pitch = k_basePitchValue + m_playerChannel.currentCombo * m_settings.pitchShift;
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

		#region Shine Strength

		public float GetShineStrength(Transform other)
		{
			return Vector3.Distance(transform.position, other.position);
		}

		#endregion

	}
}