using System.Collections.Generic;
using PierreMizzi.SoundManager;
using StarWielder.Gameplay.Player;
using UnityEngine;

namespace StarWielder.Gameplay.Enemies
{
	/// <summary>
	/// Linked to EnemyGroups. ShipStar absorbs energy from EnemyStar
	/// </summary>
	public class EnemyStar : MonoBehaviour
	{

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
			CreateCurrency();
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
		[SerializeField] private Currency m_currencyPrefab;

		private void CreateCurrency()
		{
			Currency currency = Instantiate(m_currencyPrefab, transform.position, Quaternion.identity);
			currency.Collect();
		}

		#endregion


	}
}