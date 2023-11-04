using System;
using PierreMizzi.Useful;
using UnityEngine;

namespace PierreMizzi.Gameplay.Players
{
	[RequireComponent(typeof(ShipController))]
	public class Ship : MonoBehaviour
	{

		#region Main

		[SerializeField] private PlayerChannel m_playerChannel = null;
		[SerializeField] private GameChannel m_gameChannel = null;
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

		private float m_currentEnergy = 0;
		public float currentEnergy
		{
			get { return m_currentEnergy; }
			set { m_currentEnergy = Mathf.Clamp(value, 0f, m_settings.baseEnergy); }
		}

		public bool hasEnergy => m_currentEnergy > 0;

		private void ManageEnergy()
		{
			if (!m_star.isOnShip && hasEnergy)
			{
				currentEnergy -= m_settings.energyDepleatRate * Time.deltaTime;
				m_playerChannel.onRefreshShipEnergy.Invoke(m_currentEnergy);
			}

			m_controller.enabled = hasEnergy || (m_star.isOnShip && m_star.hasEnergy);
		}

		public float GetMaxTransferableEnergy(float starEnergy)
		{
			float transferableEnergy;
			if (currentEnergy + starEnergy > m_settings.baseEnergy)
				transferableEnergy = m_settings.baseEnergy - currentEnergy;
			else
				transferableEnergy = starEnergy;

			return transferableEnergy;
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

			if (m_currentHealth <= 0)
				m_gameChannel.onGameOver.Invoke(GameOverReason.ShipDestroyed);
		}

		#endregion

	}
}
