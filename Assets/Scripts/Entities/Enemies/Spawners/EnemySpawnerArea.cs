using PierreMizzi.Useful;
using UnityEngine;

public class EnemySpawnerArea : EnemySpawner
{

	[SerializeField] private Bounds m_spawningBounds;

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position, m_spawningBounds.size);
	}

	protected override Vector3 GetRandomPosition()
	{
		return transform.position + UtilsClass.RandomInBound(m_spawningBounds);
	}

	protected override Quaternion GetRandomRotation()
	{
		return UtilsClass.RandomRotation2D();
	}

}