using System;
using PierreMizzi.Useful;
using TMPro;
using UnityEngine;

namespace PierreMizzi.Gameplay.Players
{
	[RequireComponent(typeof(ShipController))]
	public class Ship : MonoBehaviour
	{
		#region Channels

		[Header("Channels")]
		[SerializeField] private PlayerChannel m_playerChannel = null;
		[SerializeField] private GameChannel m_gameChannel = null;
		[SerializeField] private CameraChannel m_cameraChannel = null;

		#endregion

		#region Main

		[Header("Main")]
		[SerializeField] private PlayerSettings m_settings;
		private ShipController m_controller;

		private bool m_isActive = true;

		private void Initialize()
		{
			m_currentHealth = m_settings.maxHealth;
		}

		private void CallbackGameOver(GameOverReason reason)
		{
			m_isActive = false;
			m_controller.enabled = false;
		}

		#endregion

		#region MonoBehaviour

		private void Awake()
		{
			m_controller = GetComponent<ShipController>();

			m_animator = GetComponent<Animator>();
		}

		private void Start()
		{
			Initialize();

			if (m_gameChannel != null)
				m_gameChannel.onGameOver += CallbackGameOver;
		}

		private void Update()
		{
			if (m_isActive)
				ManageEnergy();

			if (Input.GetKeyDown(KeyCode.M))
				m_gameChannel.onGameOver.Invoke(GameOverReason.ShipDestroyed);


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
		public Transform starAnchor => m_starAnchor;

		#endregion

		#region Energy
		private float m_emergencyEnergy = 0;
		public float emergencyEnergy
		{
			get { return m_emergencyEnergy; }
			set { m_emergencyEnergy = Mathf.Clamp(value, 0f, m_settings.maxEmergencyEnergy); }
		}
		public bool hasEnergy;

		private void ManageEnergy()
		{
			if (!m_star.isOnShip && m_emergencyEnergy > 0)
			{
				emergencyEnergy -= m_settings.emergencyEnergyDepleatRate * Time.deltaTime;
				m_playerChannel.onRefreshShipEnergy.Invoke(m_emergencyEnergy);
				ComputeCountdown();
			}
			hasEnergy = m_emergencyEnergy > 0 || (m_star.isOnShip && m_star.hasEnergy);
			m_controller.enabled = hasEnergy;
			m_animator.SetBool(k_triggerHasEnergy, hasEnergy);
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
			m_countdownLabel.text = String.Format("{0:0.0}", m_countdown);
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
				HitByBullet(other.GetComponent<EnemyBullet>());
		}

		private void HitByBullet(EnemyBullet bullet)
		{
			Destroy(bullet.gameObject);
			m_currentHealth -= bullet.damage;
			m_playerChannel.onRefreshHealth.Invoke(m_currentHealth / m_settings.maxHealth);

			m_cameraChannel.onShipHurt.Invoke();

			if (m_currentHealth <= 0)
				m_gameChannel.onGameOver.Invoke(GameOverReason.ShipDestroyed);
		}

		#endregion

		#region Animations

		private Animator m_animator = null;
		private const string k_triggerHasEnergy = "HasEnergy";
		private const string k_triggerIsDashing = "IsDashing";

		public void SetIsDashing(bool isDashing)
		{
			m_animator.SetBool(k_triggerIsDashing, isDashing);
		}

		#endregion

	}
}
