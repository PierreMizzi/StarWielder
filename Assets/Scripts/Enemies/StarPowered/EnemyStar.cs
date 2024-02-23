using System.Collections.Generic;
using PierreMizzi.SoundManager;
using StarWielder.Gameplay.Player;
using UnityEngine;
using PierreMizzi.Useful.PoolingObjects;
using System;


namespace StarWielder.Gameplay.Enemies
{
	/// <summary>
	/// Linked to EnemyGroups. ShipStar absorbs energy from EnemyStar
	/// </summary>
	public class EnemyStar : MonoBehaviour
	{
		[SerializeField] private PlayerChannel m_playerChannel;

		private EnemyGroup m_group;

		private List<string> m_destroyedSoundIDs = new List<string>();

		[SerializeField] private StarAbsorbable m_starAbsorbable;


		public void Initialize(EnemyGroup group)
		{
			m_group = group; // Replaceable : Awake ?
		}

		public void Appear()
		{
			m_animator.SetTrigger(k_triggerAppear);
		}

		public void Kill()
		{
			SoundManager.PlayRandomSFX(m_destroyedSoundIDs);
			m_group.EnemyStarKilled(this);
			CreateCurrencies();
			m_animator.SetTrigger(k_triggerKill);
		}

		#region Animation

		[SerializeField] private Animator m_animator;
		private const string k_triggerAppear = "Appear";
		private const string k_triggerKill = "Kill";

		#endregion

		#region MonoBehaviour

		private void Awake()
		{
			m_destroyedSoundIDs = new List<string>
			{
				SoundDataID.ENEMY_STAR_DESTROYED_01,
				SoundDataID.ENEMY_STAR_DESTROYED_02,
				SoundDataID.ENEMY_STAR_DESTROYED_03,
				SoundDataID.ENEMY_STAR_DESTROYED_04,
				SoundDataID.ENEMY_STAR_DESTROYED_05,
			};
		}

		private void Start()
		{
			if (m_starAbsorbable != null)
				m_starAbsorbable.onAbsorb += Kill;
		}

		private void OnDestroy()
		{
			if (m_starAbsorbable != null)
				m_starAbsorbable.onAbsorb -= Kill;
		}

		#endregion

		#region Currency

		[Header("Currency")]
		[SerializeField] protected PoolingChannel m_poolingChannel;
		[SerializeField] private Currency m_currencyPrefab;

		private void CreateCurrencies()
		{
			for (int i = 0; i < m_playerChannel.currentCombo; i++)
				CreateCurrency();
		}

		private void CreateCurrency()
		{
			Currency currency = m_poolingChannel.onGetFromPool.Invoke(m_currencyPrefab.gameObject).GetComponent<Currency>();
			currency.transform.position = transform.position;
			currency.Collect();
		}

		#endregion

		#region Beam

		[SerializeField] private LineRenderer m_beam;
		public void SetBeamConnection()
		{
			m_beam.SetPosition(0, Vector3.zero);
			m_beam.SetPosition(1, transform.InverseTransformPoint(transform.parent.position));
		}

		#endregion

	}
}