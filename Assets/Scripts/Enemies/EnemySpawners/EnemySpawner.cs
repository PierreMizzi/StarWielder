using System;
using System.Collections;
using System.Collections.Generic;
using PierreMizzi.Useful.PoolingObjects;
using PierreMizzi.Useful;
using UnityEngine;

namespace StarWielder.Gameplay.Enemies
{
	/// <summary>
	/// Mother class from different type of spawner
	/// </summary>
	public class EnemySpawner : MonoBehaviour
	{

		protected EnemyManager m_manager;

		#region Spawning

		[Header("Spawning")]
		[SerializeField] private PoolingChannel m_poolingChannel;
		[SerializeField] protected Transform m_enemyGroupContainer;
		[SerializeField] protected List<Enemy> m_enemyPrefabs;
		[SerializeField] protected int m_validSpawnAttempts = 10;

		public virtual void Initialize(EnemyManager manager)
		{
			m_manager = manager;
		}

		public void SpawnEnemy()
		{

			Enemy enemyPrefab = m_enemyPrefabs.PickRandom();
			Enemy pooledEnemy = m_poolingChannel.onGetFromPool.Invoke(enemyPrefab.gameObject).GetComponent<Enemy>();
			pooledEnemy.Initialize(m_manager);
			m_manager.AddSpawnedEnemy(pooledEnemy);

			StartCoroutine(CheckOverlapingCoroutine(pooledEnemy));
		}

		/// <summary>
		/// Checks if the newly spawned enemy doesn't appear on top off another
		/// </summary>
		/// <param name="newEnemy">newly spawned enemy group</param>
		public IEnumerator CheckOverlapingCoroutine(Enemy newEnemy)
		{
			for (int i = 0; i <= m_validSpawnAttempts; i++)
			{
				SetEnemyTransform(newEnemy);

				// Wait a small amount of time for the physics engine to move the BoxColldier (EnemyGroup.m_area)
				yield return new WaitForSeconds(0.01f);

				if (newEnemy.IsOverlaping())
				{
					// There is a limited amount of positioning attempts to prevent infinite looping
					if (i == m_validSpawnAttempts)
					{
						Debug.Log("Unvalid spawn position");
						Destroy(newEnemy.gameObject);
						yield break;
					}
					continue;
				}
				else
					break;
			}

			newEnemy.Appear();
		}

		protected virtual void SetEnemyTransform(Enemy newEnemy)
		{
			newEnemy.transform.position = GetRandomPosition();
			newEnemy.transform.rotation = GetRandomRotation();
		}

		protected virtual Vector3 GetRandomPosition() { return Vector3.zero; }
		protected virtual Quaternion GetRandomRotation() { return Quaternion.identity; }
		protected virtual Quaternion GetRandomRotation(Vector3 enemyGroupPosition) { return Quaternion.identity; }

		#endregion

		#region Visualization

		/*
			Small debugging utilities using Gizmos to visualize the spawning area
		*/

		[Header("Visualization")]
		[SerializeField] protected bool m_displayVisualization = true;
		[SerializeField] protected Color m_colorVisualization = Color.red;

		protected virtual void DrawVisualization() { }

		#endregion
		#region MonoBehaviour

		private void OnDrawGizmos()
		{
			DrawVisualization();
		}

		#endregion

	}
}