using System.Collections;
using System.Collections.Generic;
using QGamesTest.Gameplay.Player;
using PierreMizzi.Useful;
using UnityEngine;


namespace QGamesTest.Gameplay.Enemies
{

	/// <summary>
	/// Controls EnemySpawners. The spawning speed is increased over time for balancing reasons
	/// </summary>
	public class EnemyManager : MonoBehaviour
	{

		#region Main

		[Header("Main")]
		[SerializeField] private GameChannel m_gameChannel;

		[SerializeField] private Ship m_ship;
		public Ship ship => m_ship;

		private void CallbackStartGame()
		{
			Debug.Log("CallbackStartGame : " + gameObject.GetInstanceID());
			StartSpawning();
		}

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

			if (m_gameChannel != null)
			{
				m_gameChannel.onStartGame += CallbackStartGame;
				m_gameChannel.onGameOver += CallbackGameOver;
			}
		}

		private void OnDestroy()
		{
			if (m_gameChannel != null)
			{
				m_gameChannel.onStartGame -= CallbackStartGame;
				m_gameChannel.onGameOver -= CallbackGameOver;
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
			return Random.Range(min, max);
		}

		[ContextMenu("SpawnEnemyGroup")]
		private void SpawnEnemyGroup()
		{
			EnemySpawner spawner = UtilsClass.PickRandomInList(m_enemySpawners);
			spawner.SpawnEnemyGroup();
		}

		#endregion

		#region Enemy Groups

		private List<EnemyGroup> m_enemyGroups = new List<EnemyGroup>();

		public void DeactivateEnemyGroups()
		{
			foreach (EnemyGroup enemyGroup in m_enemyGroups)
				enemyGroup.DeactivateTurrets();
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

		public Transform bulletsContainer => m_bulletsContainer;

		#endregion

	}
}