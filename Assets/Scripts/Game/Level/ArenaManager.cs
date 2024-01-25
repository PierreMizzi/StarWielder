using UnityEngine;

namespace StarWielder.Gameplay
{

	public class ArenaManager : MonoBehaviour
	{
		[SerializeField] private Camera m_mainCamera;

		public Vector2 topRightCorner { get; private set; }
		public Vector2 botLeftCorner { get; private set; }

		private Vector2 m_cameraExtents;

		[Header("Settings")]
		[SerializeField] private float m_botEmptySpace = 1.0f;
		[SerializeField] private float m_baseSize = 100;
		[SerializeField] private float m_extraSize = 50;

		[Header("Bounds")]
		[SerializeField] private ArenaEdge m_edgeLeft = null;
		[SerializeField] private ArenaEdge m_edgeRight = null;
		[SerializeField] private ArenaEdge m_edgeTop = null;
		[SerializeField] private ArenaEdge m_edgeBot = null;

		[ContextMenu("Awake")]
		public void Awake()
		{
			m_cameraExtents = new Vector2(m_mainCamera.orthographicSize * m_mainCamera.aspect, m_mainCamera.orthographicSize);

			ComputeArenaBounds();

			// Right
			MatchScreenHeight(m_edgeRight);
			PositionScreenRight(m_edgeRight);

			// Left
			MatchScreenHeight(m_edgeLeft);
			PositionScreenLeft(m_edgeLeft);

			// Top
			MatchScreenWidth(m_edgeTop);
			PositionScreenTop(m_edgeTop);

			// Bot
			MatchScreenWidth(m_edgeBot);
			PositionScreenBot(m_edgeBot);
		}

		private void ComputeArenaBounds()
		{
			topRightCorner = new Vector2(m_cameraExtents.x, m_cameraExtents.y);
			botLeftCorner = new Vector2(-m_cameraExtents.x, -m_cameraExtents.y * m_botEmptySpace);
		}

		private void MatchScreenWidth(ArenaEdge bound)
		{
			bound.SetSize(m_cameraExtents.x * 2f + m_extraSize, m_baseSize);
		}

		private void MatchScreenHeight(ArenaEdge bound)
		{
			bound.SetSize(m_baseSize, m_cameraExtents.y * 2f + m_extraSize);
		}

		private void PositionScreenLeft(ArenaEdge bound)
		{
			bound.transform.position = new Vector3(-(m_cameraExtents.x + m_baseSize / 2f), 0, 0);
		}

		private void PositionScreenRight(ArenaEdge bound)
		{
			bound.transform.position = new Vector3(m_cameraExtents.x + m_baseSize / 2f, 0, 0);
		}

		private void PositionScreenTop(ArenaEdge bound)
		{
			bound.transform.position = new Vector3(0, m_cameraExtents.y + m_baseSize / 2f, 0);
		}

		private void PositionScreenBot(ArenaEdge bound)
		{
			float botYPos = -(m_cameraExtents.y * m_botEmptySpace + m_baseSize / 2f);
			bound.transform.position = new Vector3(0, botYPos, 0);
		}

	}
}