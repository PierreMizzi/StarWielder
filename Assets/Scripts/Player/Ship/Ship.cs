using System;
using System.Collections.Generic;
using PierreMizzi.SoundManager;
using PierreMizzi.Useful;
using PierreMizzi.Useful.StateMachines;
using StarWielder.Gameplay.Enemies;
using TMPro;
using UnityEngine;

namespace StarWielder.Gameplay.Player
{
	/// <summary>
	/// Ship's class and all it's gameplay capabilities.
	/// For movement related behaviours, see ShipController.cs
	/// </summary>
	[RequireComponent(typeof(ShipController))]
	public class Ship : MonoBehaviour, IStateMachine
	{

		#region Channels

		[Header("Channels")]
		[SerializeField] private PlayerChannel m_playerChannel = null;
		[SerializeField] private GameChannel m_gameChannel = null;

		public GameChannel gameChannel => m_gameChannel;

		#endregion

		#region Main

		[Header("Main")]
		[SerializeField] private PlayerSettings m_settings;

		private ShipStats m_stats;
		public ShipStats stats { get { return m_stats; } }

		private ShipController m_controller;
		public ShipController controller => m_controller;

		private void CallbackGameOver(GameOverReason reason)
		{
			m_controller.enabled = false;
		}

		#endregion

		#region StateMachine

		[SerializeField] private ShipStateType m_initialState = ShipStateType.NoPower;
		public List<AState> states { get; set; } = new List<AState>();
		public AState currentState { get; set; }

		public void InitializeStates()
		{
			states = new List<AState>()
			{
				new ShipStateNoPower(this),
				new ShipStateStarPower(this),
				new ShipStateEmergencyPower(this),
				new ShipStateDestroyed(this),
			};

			ChangeState(m_initialState);
		}

		public void UpdateState()
		{
			currentState?.Update();
		}

		public void ChangeState(ShipStateType nextState, ShipStateType previousState = ShipStateType.None)
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

		#endregion

		#region MonoBehaviour

		private void Awake()
		{
			m_stats = new ShipStats(m_settings);
			m_currentHealth = m_stats.maxHealth;

			m_controller = GetComponent<ShipController>();
			m_animator = GetComponent<Animator>();

			InitializeStates();
		}

		private void Start()
		{
			if (m_gameChannel != null)
				m_gameChannel.onGameOver += CallbackGameOver;
		}

		private void Update()
		{
			UpdateState();

#if UNITY_EDITOR

			//To test GameOverScreen's animation
			if (Input.GetKeyDown(KeyCode.M))
				m_gameChannel.onGameOver.Invoke(GameOverReason.ShipDestroyed);

#endif

		}

		private void LateUpdate()
		{
			UpdateCountdownTransform();
		}

		private void OnDestroy()
		{
			if (m_gameChannel != null)
				m_gameChannel.onGameOver -= CallbackGameOver;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			CheckIsHealthModifier(other);
		}

		#endregion

		#region Star

		[Header("Star")]
		[SerializeField] private Star m_star;

		/// <summary>
		/// Container transform of the Star when docked to the ship
		/// </summary>
		[SerializeField] private Transform m_starAnchor;

		public Star star => m_star;

		public Transform starAnchor => m_starAnchor;

		#endregion

		#region Energy

		public float normalizedEmergencyEnergy
		{
			get
			{
				return m_emergencyEnergy / m_stats.maxEmergencyEnergy;
			}
		}
		private float m_emergencyEnergy;
		public float emergencyEnergy
		{
			get { return m_emergencyEnergy; }
			set
			{
				m_emergencyEnergy = Mathf.Clamp(value, 0f, m_stats.maxEmergencyEnergy);
				m_playerChannel.onRefreshEmergencyEnergy.Invoke(normalizedEmergencyEnergy);
			}
		}

		public void DepleateEmergencyEnergy()
		{
			emergencyEnergy -= m_settings.emergencyEnergyDepleatRate * Time.deltaTime;
		}

		public float GetMaxTransferableEnergy(float starEnergy)
		{
			float transferableEnergy;
			if (emergencyEnergy + starEnergy > m_stats.maxEmergencyEnergy)
				transferableEnergy = m_stats.maxEmergencyEnergy - emergencyEnergy;
			else
				transferableEnergy = starEnergy;

			return transferableEnergy;
		}

		#endregion

		#region Countdown

		/*
			Countdown before all emergency energy has been consumed.
			Only visible when Star is not docked
		*/

		[Header("Countdown")]
		[SerializeField] private TextMeshPro m_countdownLabel;
		[SerializeField] private Vector3 m_countdownTransformOffset;
		private float m_countdown;

		[Obsolete]
		private void ComputeCountdown()
		{
			m_countdown = m_emergencyEnergy / m_settings.emergencyEnergyDepleatRate;
			m_countdownLabel.text = String.Format("{0:0.0}", m_countdown) + "<size=50%>s</size>";
			m_animator.SetFloat(k_floatCountdown, m_countdown);
		}

		[Obsolete]
		private void UpdateCountdownTransform()
		{
			m_countdownLabel.transform.position = transform.position + m_countdownTransformOffset;
			m_countdownLabel.transform.rotation = Quaternion.identity;
		}

		#endregion

		#region Health

		[Header("Health")]
		[SerializeField] private BoxCollider2D m_boxCollider;

		private float m_currentHealth;

		private void CheckIsHealthModifier(Collider2D other)
		{
			if (other.TryGetComponent(out ShipHealthModifier healthModifier))
			{
				healthModifier.onModify.Invoke();

				// Note : can add negative values (values etc)
				m_currentHealth += healthModifier.healthModification;
				m_currentHealth = Math.Clamp(m_currentHealth, 0, m_stats.maxHealth);
				m_playerChannel.onRefreshShipHealth.Invoke(m_currentHealth / m_settings.maxHealth);

				if (healthModifier.healthModification < 0)
					SoundManager.PlaySFX(SoundDataID.SHIP_HURT);

				if (m_currentHealth <= 0)
					ChangeState(ShipStateType.Destroyed);
			}
		}

		private void SetInvincible(bool isInvincible)
		{
			m_boxCollider.enabled = isInvincible;
		}

		#endregion

		#region Animations

		private Animator m_animator = null;
		public Animator animator => m_animator;

		public const string k_boolIsDead = "IsDead";

		public const string k_boolHasEnergy = "HasEnergy";
		public const string k_boolIsDashing = "IsDashing";

		public const string k_floatCountdown = "Countdown";
		public const string k_boolDisplayCountdown = "DisplayCountdown";

		public void SetIsDashing(bool isDashing)
		{
			SetInvincible(!isDashing);
			m_animator.SetBool(k_boolIsDashing, isDashing);
		}

		#endregion

	}
}
