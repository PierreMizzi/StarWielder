using UnityEngine;

namespace StarWielder.Gameplay.Enemies
{

	/// <summary>
	/// Spawns EnemyGroups inside the edges of a square, near the screen borders.
	/// Rotates the EnemyGroup toward the center of the screen
	/// </summary>
	public class EnemySpawnerEdge : EnemySpawner
	{

		private void Awake()
		{
			ComputeEdges();
		}

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
		[SerializeField] private Bounds m_minBounds;
		[SerializeField] private Bounds m_maxBounds;

		private Vector2 m_edgesRight;
		private Vector2 m_edgesLeft;
		private Vector2 m_edgesTop;
		private Vector2 m_edgesBot;

		protected override Vector3 GetRandomPosition()
		{
			Vector3 position = Vector3.zero;

			bool isHorizontalOrVertical = Random.Range(0, 2) == 0;
			bool isPositiveOrNegative = Random.Range(0, 2) == 0;

			// Along width
			if (isHorizontalOrVertical)
			{
				position.x = Random.Range(m_edgesLeft.y, m_edgesRight.y);

				// Top
				if (isPositiveOrNegative)
					position.y = Random.Range(m_edgesTop.x, m_edgesTop.y);
				// Bot
				else
					position.y = Random.Range(m_edgesBot.x, m_edgesBot.y);

			}
			else
			{
				position.y = Random.Range(m_edgesBot.y, m_edgesTop.y);
				// Right
				if (isPositiveOrNegative)
					position.x = Random.Range(m_edgesRight.x, m_edgesRight.y);
				// Left
				else
					position.x = Random.Range(m_edgesLeft.x, m_edgesLeft.y);

			}

			return position;

		}

		private void ComputeEdges()
		{
			m_edgesRight.x = m_minBounds.center.x + m_minBounds.extents.x;
			m_edgesRight.y = m_maxBounds.center.x + m_maxBounds.extents.x;

			m_edgesLeft.x = m_minBounds.center.x + -m_minBounds.extents.x;
			m_edgesLeft.y = m_maxBounds.center.x + -m_maxBounds.extents.x;

			m_edgesTop.x = m_minBounds.center.y + m_minBounds.extents.y;
			m_edgesTop.y = m_maxBounds.center.y + m_maxBounds.extents.y;

			m_edgesBot.x = m_minBounds.center.y + -m_minBounds.extents.y;
			m_edgesBot.y = m_maxBounds.center.y + -m_maxBounds.extents.y;

			// Debug.Log($"Right : {m_edgesRight}");
			// Debug.Log($"Left : {m_edgesLeft}");
			// Debug.Log($"Top : {m_edgesTop}");
			// Debug.Log($"Bot : {m_edgesBot}");
		}


		#endregion


		#region Rotation

		[Header("Rotation")]

		[SerializeField] private float m_maxRandomAngle;

		protected override Quaternion GetRandomRotation(Vector3 enemyGroupPosition)
		{
			float angle = 0f;
			Vector3 directionToCenter = (m_minBounds.center - enemyGroupPosition).normalized;
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
				Gizmos.DrawWireCube(m_minBounds.center, m_minBounds.size);
				Gizmos.DrawWireCube(m_maxBounds.center, m_maxBounds.size);

				return;
			}
		}

		#endregion

	}
}