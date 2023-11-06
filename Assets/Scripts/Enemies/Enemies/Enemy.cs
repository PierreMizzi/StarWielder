using System;
using System.Collections.Generic;
using PierreMizzi.SoundManager;
using PierreMizzi.Useful;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	private EnemyGroup m_group;

	[SerializeField] private float m_energy = 1f;

	[SerializeField] private List<string> m_destroyedSoundIDs = null;

	public float energy => m_energy;

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
		SoundManager.PlaySFX(UtilsClass.PickRandom(m_destroyedSoundIDs));
		m_group.EnemyDestroyed(this);
	}

	public void Initialize(EnemyGroup group)
	{
		m_group = group;
	}
}