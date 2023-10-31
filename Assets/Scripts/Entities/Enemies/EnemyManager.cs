using System;
using System.Collections;
using PierreMizzi.Useful;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

	#region Base

	private void Start()
	{
		StartSpawning();
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, m_spawningBounds.size);
	}

	#endregion

	#region Spawning

	[Header("Spawning")]
	[SerializeField] private Bounds m_spawningBounds;
	[SerializeField] private GameObject m_enemyGroupPrefab = null;
	[SerializeField] private float m_spawnFrequency = 1f;

	private IEnumerator m_spawningCoroutine;

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
		GameObject newEnemyGroup = Instantiate(m_enemyGroupPrefab, transform);

		newEnemyGroup.transform.rotation = UtilsClass.RandomRotation2D();

		newEnemyGroup.transform.position = transform.position + UtilsClass.RandomInBound(m_spawningBounds);
	}


	#endregion

}