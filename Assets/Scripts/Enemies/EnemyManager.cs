using System.Collections;
using System.Collections.Generic;
using StarWielder.Gameplay.Player;
using PierreMizzi.Useful;
using UnityEngine;
using System;

namespace StarWielder.Gameplay.Enemies
{

	/// <summary>
	/// Controls EnemySpawners
	/// </summary>
	public class EnemyManager : MonoBehaviour
	{

		#region Behaviour

		[Header("Main")]
		private FightStageManager m_fightStageManager;

		private FightStageSettings m_currentSettings;

		// TODO : Kinda weird
		[SerializeField] private Ship m_ship;
		public Ship ship => m_ship;

		private int m_spawnedEnemiesCount;
		private int m_killedEnemiesCount;

		public void Initialize(FightStageManager fightStageManager)
		{
			m_fightStageManager = fightStageManager;
		}

		public void SetupStageSettings(FightStageSettings settings)
		{
			m_currentSettings = settings;

			m_spawnedEnemiesCount = m_currentSettings.stageEnemiesCount + m_currentSettings.beginningEnemiesCount;
			m_killedEnemiesCount = m_spawnedEnemiesCount;
		}

		public void StartStage()
		{
			if (m_currentSettings.beginningEnemiesCount > 0)
				SpawnEnemyGroups(m_currentSettings.beginningEnemiesCount);

			if (m_currentSettings.stageEnemiesCount > 0)
			{
				StartSpawning();
			}
		}

		public void CallbackGameOver()
		{
			SpawnedEnemiesStopBehaviour();
			StopSpawning();
		}

		#endregion

		#region MonoBehaviour
//　彼が最悪の事態を覚悟していれ
		private void Start()
		{
			InitializeSpawners();
		}

		#endregion

		#region Spawning

		[Header("Spawning")]
		[SerializeField] private List<EnemySpawner> m_enemySpawners = null;

		private float m_spawnFrequency;

		private IEnumerator m_spawningCoroutine;

		private void InitializeSpawners()
		{
			if (m_isDebugging)
				SetDebugging();

			Debug.Log("InitializeSpawners" + gameObject.GetInstanceID());
			foreach (EnemySpawner enemySpawners in m_enemySpawners)
				enemySpawners.Initialize(this);
		}

		public void StartSpawning()
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
				m_spawnFrequency = GetRandomSpawnDelay();

				yield return new WaitForSeconds(m_spawnFrequency);

				SpawnEnemyGroup();
			}
		}

		private float GetRandomSpawnDelay()
		{
			return UnityEngine.Random.Range(m_currentSettings.minSpawnDelay, m_currentSettings.maxSpawnDelay);
		}

		private void SpawnEnemyGroup()
		{
			m_spawnedEnemiesCount--;
			EnemySpawner spawner = m_enemySpawners.PickRandom();
			spawner.SpawnEnemy();

			if (m_spawnedEnemiesCount == 0)
				StopSpawning();
		}

		private void SpawnEnemyGroups(int count)
		{
			for (int i = 0; i < count; i++)
				SpawnEnemyGroup();
		}

		#endregion

		#region Enemy Groups

		private List<Enemy> m_spawnedEnemies = new List<Enemy>();

		public void SpawnedEnemiesStopBehaviour()
		{
			foreach (Enemy enemy in m_spawnedEnemies)
				enemy.StopBehaviour();
		}

		public void AddSpawnedEnemy(Enemy enemy)
		{
			m_spawnedEnemies.Add(enemy);
		}

		public void RemoveSpawnedEnemy(Enemy enemy)
		{
			if (m_spawnedEnemies.Contains(enemy))
			{
				m_spawnedEnemies.Remove(enemy);
				m_killedEnemiesCount--;

				if (m_killedEnemiesCount == 0)
					m_fightStageManager.StopStage();
			}
		}

		#endregion

		#region Bullets

		[Header("Bullets")]
		[SerializeField] private Transform m_bulletsContainer;
		public Transform bulletsContainer => m_bulletsContainer;

		#endregion

		#region Debug

		[Header("Debug")]

		[SerializeField] private bool m_isDebugging;

		[SerializeField] private List<EnemySpawner> m_debugEnemySpawners;

		private void SetDebugging()
		{
			m_enemySpawners = new List<EnemySpawner>(m_debugEnemySpawners);
		}

		#endregion

	}
}