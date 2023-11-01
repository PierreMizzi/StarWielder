using System.Collections;
using System.Collections.Generic;
using PierreMizzi.Useful;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

	protected EnemyManager m_manager;
	[SerializeField] protected Transform m_enemyGroupContainer;
	[SerializeField] protected List<EnemyGroup> m_enemyGroupPrefabs;

	[SerializeField] protected int m_validSpawnAttempts = 10;

	public virtual void Initialize(EnemyManager manager)
	{
		m_manager = manager;
	}

	public virtual EnemyGroup SpawnEnemyGroup()
	{
		EnemyGroup randomGroupPrefab = UtilsClass.PickRandomInList(m_enemyGroupPrefabs);
		EnemyGroup newEnemyGroup = Instantiate(randomGroupPrefab, m_enemyGroupContainer);
		newEnemyGroup.Initialize(m_manager);
		m_manager.AddEnemyGroup(newEnemyGroup);

		StartCoroutine(Test(newEnemyGroup));


		return newEnemyGroup;
	}

	public IEnumerator Test(EnemyGroup newEnemyGroup)
	{
		Vector3 position = Vector3.zero;

		for (int i = 0; i <= m_validSpawnAttempts; i++)
		{
			newEnemyGroup.transform.position = GetRandomPosition();
			newEnemyGroup.transform.rotation = GetRandomRotation();

			yield return new WaitForEndOfFrame();

			if (newEnemyGroup.CheckIsOverlaping())
			{
				if (i == m_validSpawnAttempts - 1)
				{
					Debug.Log("Unvalid spawn position");

					Destroy(newEnemyGroup.gameObject);
					yield return null;
				}
				continue;
			}
			else
				break;

		}
		yield return null;
	}

	protected virtual Vector3 GetRandomPosition() { return Vector3.zero; }
	protected virtual Quaternion GetRandomRotation() { return Quaternion.identity; }

}