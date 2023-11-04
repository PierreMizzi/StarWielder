using System.Collections;
using System.Collections.Generic;
using PierreMizzi.Gameplay.Players;
using PierreMizzi.Useful;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

	#region Main

	[Header("Main")]
	[SerializeField] private GameChannel m_gameChannel;

	[SerializeField] private Ship m_ship;
	public Ship ship => m_ship;

	[SerializeField] private bool m_autoSpawn = true;

	private void CallbackGameOver(GameOverReason reason)
	{
		DeactivateEnemyGroups();
		StopSpawning();
	}

	#endregion

	#region MonoBehaviour

	private void Start()
	{
		InitializeSpawners();

		if (m_autoSpawn)
			StartSpawning();

		if (m_gameChannel != null)
			m_gameChannel.onGameOver += CallbackGameOver;
	}

	private void OnDestroy()
	{
		if (m_gameChannel != null)
			m_gameChannel.onGameOver -= CallbackGameOver;
	}

	#endregion

	#region Spawning

	[Header("Spawning")]
	[SerializeField] private float m_minSpawnFrequency = 2f;
	[SerializeField] private float m_maxSpawnFrequency = 5f;
	private float m_spawnFrequency;
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

	private void StopSpawning()
	{
		if (m_spawningCoroutine != null)
		{
			StopCoroutine(m_spawningCoroutine);
			m_spawningCoroutine = null;
		}
	}

	private IEnumerator SpawningCoroutine()
	{
		while (true)
		{
			SpawnEnemyGroup();
			m_spawnFrequency = Random.Range(m_minSpawnFrequency, m_maxSpawnFrequency);
			yield return new WaitForSeconds(m_spawnFrequency);
		}
	}

	[ContextMenu("SpawnEnemyGroup")]
	private void SpawnEnemyGroup()
	{
		EnemySpawner spawner = UtilsClass.PickRandomInList(m_enemySpawners);
		spawner.SpawnEnemyGroup();
	}

	#endregion

	#region Enemy Groups

	[SerializeField]
	private List<EnemyGroup> m_enemyGroups;

	public void DeactivateEnemyGroups()
	{
		foreach (EnemyGroup enemyGroup in m_enemyGroups)
			enemyGroup.Deactivate();
	}

	public void AddEnemyGroup(EnemyGroup enemyGroup)
	{
		m_enemyGroups.Add(enemyGroup);
	}

	public void RemoveEnemyGroup(EnemyGroup enemyGroup)
	{
		if (m_enemyGroups.Contains(enemyGroup))
			m_enemyGroups.Remove(enemyGroup);
	}

	#endregion

	#region Bullets

	[Header("Bullets")]
	[SerializeField] private Transform m_bulletsContainer;
	[SerializeField] private List<EnemyBullet> m_enemyBullets = null;

	public Transform bulletsContainer => m_bulletsContainer;

	#endregion

}