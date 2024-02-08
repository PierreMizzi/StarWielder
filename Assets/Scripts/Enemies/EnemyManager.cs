using System.Collections;
using System.Collections.Generic;
using StarWielder.Gameplay.Player;
using PierreMizzi.Useful;
using UnityEngine;
using System;

namespace StarWielder.Gameplay.Enemies
{

	/// <summary>
	/// Controls EnemySpawners. The spawning speed is increased over time for balancing reasons
	/// </summary>
	public class EnemyManager : StageStateManager
	{

		#region Main

		[Header("Main")]
		[SerializeField] private GameChannel m_gameChannel;

		// TODO : Kinda weird
		[SerializeField] private Ship m_ship;
		public Ship ship => m_ship;

		private int m_spawnedEnemiesCount;
		private int m_killedEnemiesCount;

		private void CallbackStartFightStage(FightStageData data)
		{
			m_spawnedEnemiesCount = data.enemiesCount;
			m_killedEnemiesCount = data.enemiesCount;

			StartSpawning();
		}

		[Obsolete]
		private void CallbackStartGame()
		{
			Debug.Log("CallbackStartGame : " + gameObject.GetInstanceID());
			StartSpawning();
		}

		[Obsolete]
		private void CallbackGameOver(GameOverReason reason)
		{
			SpawnedEnemiesStopBehaviour();
			StopSpawning();
		}

		#endregion

		#region MonoBehaviour

		private void Start()
		{
			InitializeSpawners();

			if (m_gameChannel != null)
			{
				// TODO : Start called by LevelManager
				m_gameChannel.onFightStageStart += CallbackStartFightStage;


				// m_gameChannel.onStartGame += CallbackStartGame;
				// m_gameChannel.onGameOver += CallbackGameOver;
			}
		}

		private void OnDestroy()
		{
			if (m_gameChannel != null)
			{
				m_gameChannel.onFightStageStart -= CallbackStartFightStage;

				// m_gameChannel.onStartGame -= CallbackStartGame;
				// m_gameChannel.onGameOver -= CallbackGameOver;
			}
		}

		#endregion

		#region Spawning

		[Header("Spawning")]
		[SerializeField] private List<EnemySpawner> m_enemySpawners = null;
		[SerializeField] private AnimationCurve m_minSpawnDelayCurve;
		[SerializeField] private AnimationCurve m_maxSpawnDelayCurve;

		private float m_spawnTimer;
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

				m_spawnTimer += Time.deltaTime;
				m_spawnFrequency = GetRandomSpawnDelay();
				yield return new WaitForSeconds(m_spawnFrequency);
			}
		}

		private float GetRandomSpawnDelay()
		{
			float min = m_minSpawnDelayCurve.Evaluate(m_spawnTimer);
			float max = m_minSpawnDelayCurve.Evaluate(m_spawnTimer);
			return UnityEngine.Random.Range(min, max);
		}

		[ContextMenu("SpawnEnemyGroup")]
		private void SpawnEnemyGroup()
		{
			m_spawnedEnemiesCount--;
			EnemySpawner spawner = UtilsClass.PickRandomInList(m_enemySpawners);
			spawner.SpawnEnemy();

			if (m_spawnedEnemiesCount == 0)
				StopSpawning();
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
					m_gameChannel.onFightStageEnd.Invoke();
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