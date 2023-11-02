using PierreMizzi.Useful;
using UnityEngine;

namespace PierreMizzi.Gameplay.Players
{
	[RequireComponent(typeof(ShipController))]
	public class Ship : MonoBehaviour
	{

		#region Base

		private ShipController m_controller;

		[SerializeField]
		private PlayerSettings m_settings;

		[SerializeField] private PlayerChannel m_playerChannel = null;

		[SerializeField] private Star m_star;

		private void Initialize()
		{
			m_currentHealth = m_settings.maxHealth;

			m_currentEnergy = m_settings.baseEnergy;
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
		}

		private void Update()
		{
			ManageEnergy();
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (UtilsClass.CheckLayer(m_damageLayerMask.value, other.gameObject.layer))
				HitByBullet(other.GetComponent<EnemyBullet>());
		}

		#endregion

		#region Star

		[SerializeField]
		private Transform m_starAnchor;
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

		private void HitByBullet(EnemyBullet bullet)
		{
			Destroy(bullet.gameObject);
			m_currentHealth -= bullet.damage;

			if (m_currentHealth <= 0)
			{
				Debug.LogError("SHIP IS DESTROYED !");
				Debug.Break();
			}
		}

		#endregion

	}
}
