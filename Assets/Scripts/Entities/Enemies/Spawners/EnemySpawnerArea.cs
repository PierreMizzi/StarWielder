using PierreMizzi.Useful;
using UnityEngine;

public class EnemySpawnerArea : EnemySpawner
{

	[SerializeField] private Bounds m_spawningBounds;

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, m_spawningBounds.size);
	}

	public override EnemyGroup SpawnEnemyGroup()
	{
		EnemyGroup newEnemyGroup = base.SpawnEnemyGroup();

		newEnemyGroup.transform.rotation = UtilsClass.RandomRotation2D();

		newEnemyGroup.transform.position = transform.position + UtilsClass.RandomInBound(m_spawningBounds);

		return newEnemyGroup;
	}
}