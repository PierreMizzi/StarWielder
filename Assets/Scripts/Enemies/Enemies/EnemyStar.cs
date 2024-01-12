using System.Collections.Generic;
using PierreMizzi.SoundManager;
using UnityEngine;

namespace StarWielder.Gameplay.Enemies
{
	/// <summary>
	/// Linked to EnemyGroups. ShipStar absorbs energy from EnemyStar
	/// </summary>
	public class EnemyStar : MonoBehaviour
	{

		private EnemyGroup m_group;

		[SerializeField] private float m_absorbedEnergy = 3f;
		public float energy => m_absorbedEnergy;

		private List<string> m_destroyedSoundIDs = null;

		public void Initialize(EnemyGroup group)
		{
			m_group = group;
		}

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

		private void OnDestroy()
		{
			SoundManager.PlayRandomSFX(m_destroyedSoundIDs);
			m_group.EnemyStarDestroyed(this);
		}


	}
}