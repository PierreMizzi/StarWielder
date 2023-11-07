using System;
using System.Collections.Generic;
using DG.Tweening;
using PierreMizzi.Useful;
using PierreMizzi.Useful.StateMachines;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PierreMizzi.Gameplay.Players
{
	[RequireComponent(typeof(ShipController))]
	public class Ship : MonoBehaviour, IStateMachine
	{

		#region Channels

		[Header("Channels")]
		[SerializeField] private PlayerChannel m_playerChannel = null;
		[SerializeField] private GameChannel m_gameChannel = null;
		[SerializeField] private CameraChannel m_cameraChannel = null;

		public GameChannel gameChannel => m_gameChannel;

		#endregion

		#region Main

		[Header("Main")]
		[SerializeField] private PlayerSettings m_settings;
		public PlayerSettings settings => m_settings;
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
			m_currentHealth = m_settings.maxHealth;

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

			if (Input.GetKeyDown(KeyCode.M))
				SoundManager.SoundManager.PlaySFX(SoundDataID.SHIP_DASH);
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
			CheckIsBullet(other);
		}

		#endregion

		#region Star

		[Header("Star")]
		[SerializeField] private Star m_star;
		[SerializeField] private Transform m_starAnchor;

		public Star star => m_star;
		public Transform starAnchor => m_starAnchor;

		#endregion

		#region Energy

		private float m_emergencyEnergy = 0;
		public float emergencyEnergy
		{
			get { return m_emergencyEnergy; }
			set { m_emergencyEnergy = Mathf.Clamp(value, 0f, m_settings.maxEmergencyEnergy); }
		}

		public void DepleateEmergencyEnergy()
		{
			emergencyEnergy -= m_settings.emergencyEnergyDepleatRate * Time.deltaTime;
			ComputeCountdown();
		}

		public float GetMaxTransferableEnergy(float starEnergy)
		{
			float transferableEnergy;
			if (emergencyEnergy + starEnergy > m_settings.maxEmergencyEnergy)
				transferableEnergy = m_settings.maxEmergencyEnergy - emergencyEnergy;
			else
				transferableEnergy = starEnergy;

			return transferableEnergy;
		}

		#endregion

		#region Countdown

		[Header("Countdown")]
		[SerializeField] private TextMeshPro m_countdownLabel;
		[SerializeField] private Vector3 m_countdownTransformOffset;
		private float m_countdown;

		private void ComputeCountdown()
		{
			m_countdown = m_emergencyEnergy / m_settings.emergencyEnergyDepleatRate;
			m_countdownLabel.text = String.Format("{0:0.0}", m_countdown) + "<size=50%>s</size>";
			m_animator.SetFloat(k_floatCountdown, m_countdown);
		}

		private void UpdateCountdownTransform()
		{
			m_countdownLabel.transform.position = transform.position + m_countdownTransformOffset;
			m_countdownLabel.transform.rotation = Quaternion.identity;
		}

		#endregion

		#region Health

		[Header("Health")]
		[SerializeField] private LayerMask m_damageLayerMask;

		private float m_currentHealth;

		private void CheckIsBullet(Collider2D other)
		{
			if (UtilsClass.CheckLayer(m_damageLayerMask.value, other.gameObject.layer))
				HitByBullet(other);
		}

		private void HitByBullet(Collider2D other)
		{
			if (other.TryGetComponent(out EnemyBullet bullet))
			{
				bullet.HitShip();

				m_currentHealth -= bullet.damage;
				m_playerChannel.onRefreshHealth.Invoke(m_currentHealth / m_settings.maxHealth);

				m_cameraChannel.onShipHurt.Invoke();

				if (m_currentHealth <= 0)
					ChangeState(ShipStateType.Destroyed);
			}
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
			m_animator.SetBool(k_boolIsDashing, isDashing);
		}

		#endregion

	}
}
