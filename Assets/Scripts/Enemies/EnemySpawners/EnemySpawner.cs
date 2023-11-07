using System.Collections;
using System.Collections.Generic;
using PierreMizzi.Useful;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

	protected EnemyManager m_manager;

	#region Spawning

	[Header("Spawning")]
	[SerializeField] protected Transform m_enemyGroupContainer;
	[SerializeField] protected List<EnemyGroup> m_enemyGroupPrefabs;
	[SerializeField] protected int m_validSpawnAttempts = 10;

	public virtual void Initialize(EnemyManager manager)
	{
		m_manager = manager;
	}

	public void SpawnEnemyGroup()
	{
		EnemyGroup randomGroupPrefab = UtilsClass.PickRandomInList(m_enemyGroupPrefabs);
		EnemyGroup newEnemyGroup = Instantiate(randomGroupPrefab, m_enemyGroupContainer);
		newEnemyGroup.Initialize(m_manager);
		m_manager.AddEnemyGroup(newEnemyGroup);

		StartCoroutine(CheckOverlapingCoroutine(newEnemyGroup));
	}

	public IEnumerator CheckOverlapingCoroutine(EnemyGroup newEnemyGroup)
	{
		for (int i = 0; i <= m_validSpawnAttempts; i++)
		{
			SetEnemyGroupTransform(newEnemyGroup);

			yield return new WaitForSeconds(0.01f);

			if (newEnemyGroup.IsOverlaping())
			{
				if (i == m_validSpawnAttempts)
				{
					Debug.Log("Unvalid spawn position");
					Destroy(newEnemyGroup.gameObject);
					yield break;
				}
				continue;
			}
			else
				break;
		}

		newEnemyGroup.Appear();
	}

	protected virtual void SetEnemyGroupTransform(EnemyGroup newEnemyGroup)
	{
		newEnemyGroup.transform.position = GetRandomPosition();
		newEnemyGroup.transform.rotation = GetRandomRotation();
	}

	protected virtual Vector3 GetRandomPosition() { return Vector3.zero; }
	protected virtual Quaternion GetRandomRotation() { return Quaternion.identity; }
	protected virtual Quaternion GetRandomRotation(Vector3 enemyGroupPosition) { return Quaternion.identity; }

	#endregion

	#region Visualization

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