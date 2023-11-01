using System;
using System.Collections;
using System.Collections.Generic;
using PierreMizzi.Useful;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

	#region Base

	private void Start()
	{
		InitializeSpawners();
		StartSpawning();
	}

	#endregion

	#region Spawning

	[Header("Spawning")]
	[SerializeField] private float m_spawnFrequency = 1f;
	[SerializeField] private List<EnemySpawner> m_enemySpawners = null;

	private IEnumerator m_spawningCoroutine;

	private void InitializeSpawners()
	{
		foreach (EnemySpawner enemySpawners in m_enemySpawners)
			enemySpawners.Initialize(this);
	}

	private void StartSpawning()
	{
		if (m_spawningCoroutine == null)
		{
			m_spawningCoroutine = SpawningCoroutine();
			StartCoroutine(m_spawningCoroutine);
		}
	}

	private IEnumerator SpawningCoroutine()
	{
		while (true)
		{
			SpawnEnemyGroup();
			yield return new WaitForSeconds(m_spawnFrequency);
		}
	}

	private void SpawnEnemyGroup()
	{
		EnemySpawner spawner = UtilsClass.PickRandomInList(m_enemySpawners);
		spawner.SpawnEnemyGroup();
	}

	private bool CheckValidSpawnPosition(Vector3 position)
	{
		foreach (EnemyGroup group in m_enemyGroups)
			if (group.CheckInsideArea(position))
				return false;

		return true;
	}

	#endregion

	#region Enemy Groups

	[SerializeField]
	private List<EnemyGroup> m_enemyGroups;

	public void AddEnemyGroup(EnemyGroup enemyGroup)
	{
		m_enemyGroups.Add(enemyGroup);
	}

	public void RemoveEnemyGroup(EnemyGroup enemyGroup)
	{
		if (m_enemyGroups.Contains(enemyGroup))
			m_enemyGroups.Add(enemyGroup);
	}

	#endregion

}