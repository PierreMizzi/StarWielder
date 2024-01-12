using PierreMizzi.Useful;
using UnityEngine;

namespace StarWielder.Gameplay.Enemies
{
	public class EnemySpawnerArea : EnemySpawner
	{

		[SerializeField] private Bounds m_spawningBounds;

		protected override void DrawVisualization()
		{
			if (m_displayVisualization)
			{
				Gizmos.color = m_colorVisualization;
				Gizmos.DrawCube(transform.position, m_spawningBounds.size);
			}
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
}