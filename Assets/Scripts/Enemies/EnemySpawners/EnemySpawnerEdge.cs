using UnityEngine;

namespace QGamesTest.Gameplay.Enemies
{

	/// <summary>
	/// Spawns EnemyGroups inside the edges of a square, near the screen borders.
	/// Rotates the EnemyGroup toward the center of the screen
	/// </summary>
	public class EnemySpawnerEdge : EnemySpawner
	{

		#region EnemySpawner

		protected override void SetEnemyGroupTransform(EnemyGroup newEnemyGroup)
		{
			Vector3 position = GetRandomPosition();

			newEnemyGroup.transform.position = position;
			newEnemyGroup.transform.rotation = GetRandomRotation(position);
		}

		#endregion

		#region Position

		[Header("Position")]
		// TODO : Make sure min can't go above max, and max can't go below min
		[SerializeField] private float m_minHorizontal = 5f;
		[SerializeField] private float m_maxHorizontal = 7f;
		[SerializeField] private float m_minVertical = 4f;
		[SerializeField] private float m_maxVertical = 4.5f;

		protected override Vector3 GetRandomPosition()
		{
			Vector3 position = Vector3.zero;

			bool isHorizontalOrVertical = Random.Range(0, 2) == 0;
			bool isPositiveOrNegative = Random.Range(0, 2) == 0;

			if (isHorizontalOrVertical)
			{
				position.x = Random.Range(-m_maxHorizontal, m_maxHorizontal);
				position.y = Random.Range(m_minVertical, m_maxVertical) * (isPositiveOrNegative ? 1 : -1);
			}
			else
			{
				position.x = Random.Range(m_minHorizontal, m_maxHorizontal) * (isPositiveOrNegative ? 1 : -1);
				position.y = Random.Range(-m_maxVertical, m_maxVertical);
			}

			return position;
		}

		#endregion


		#region Rotation

		[Header("Rotation")]

		[SerializeField] private float m_maxRandomAngle;

		protected override Quaternion GetRandomRotation(Vector3 enemyGroupPosition)
		{
			float angle = 0f;
			Vector3 directionToCenter = -enemyGroupPosition.normalized;
			angle = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;

			int plusOrMinus = (Random.Range(0, 2) == 0) ? 1 : -1;

			angle += plusOrMinus * Random.Range(0f, m_maxRandomAngle);
			Vector3 randomEuler = new Vector3(0f, 0f, angle);
			return Quaternion.Euler(randomEuler);
		}

		#endregion

		#region Visualization

		protected override void DrawVisualization()
		{
			if (m_displayVisualization)
			{
				Gizmos.color = m_colorVisualization;
				Vector3 minEdgesCube = new Vector3(m_minHorizontal, m_minVertical, 0f);
				Vector3 maxEdgesCube = new Vector3(m_maxHorizontal, m_maxVertical, 0f);

				minEdgesCube *= 2f;
				maxEdgesCube *= 2f;

				Gizmos.DrawWireCube(Vector3.zero, minEdgesCube);
				Gizmos.DrawWireCube(Vector3.zero, maxEdgesCube);
			}
		}

		#endregion

	}
}