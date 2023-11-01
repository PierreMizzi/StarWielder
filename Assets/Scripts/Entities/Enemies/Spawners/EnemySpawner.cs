using System.Collections.Generic;
using PierreMizzi.Useful;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

	protected EnemyManager m_manager;
	[SerializeField] protected Transform m_enemyGroupContainer;
	[SerializeField] protected List<EnemyGroup> m_enemyGroupPrefabs;

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

		return newEnemyGroup;
	}

}