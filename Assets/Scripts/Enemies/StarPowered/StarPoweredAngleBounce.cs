using PierreMizzi.Useful;
using UnityEngine;

namespace StarWielder.Gameplay.Enemies
{

	public class StarPoweredAngleBounce : EnemyGroup
	{
		#region Angle

		[SerializeField] private float m_minAngle = 15f;
		[SerializeField] private float m_maxAngle = 45f;

		[SerializeField] private float m_closeRange;
		[SerializeField] private float m_farRange;

		private float m_randomAngleRadian;
		private Vector3 m_rightAngleDirection;
		private Vector3 m_leftAngleDirection;

		public override void Initialize(EnemyManager manager)
		{
			base.Initialize(manager);

			ChangeAngleBounce(Random.Range(m_minAngle, m_maxAngle));
		}

		[ContextMenu("ChangeAngleBounce")]
		private void ChangeAngleBounce(float angle)
		{
			m_randomAngleRadian = Mathf.Deg2Rad * UtilsClass.ToFullAngle(angle - 90f);

			m_rightAngleDirection = new Vector2(Mathf.Cos(m_randomAngleRadian), Mathf.Sin(m_randomAngleRadian));
			SetEnemyStarPosition(0, m_rightAngleDirection * m_closeRange);
			SetEnemyStarPosition(1, m_rightAngleDirection * m_farRange);

			m_randomAngleRadian = Mathf.Deg2Rad * (90 - angle);

			m_leftAngleDirection = new Vector2(Mathf.Cos(m_randomAngleRadian), Mathf.Sin(m_randomAngleRadian));
			SetEnemyStarPosition(2, m_leftAngleDirection * m_closeRange);
			SetEnemyStarPosition(3, m_leftAngleDirection * m_farRange);
		}

		private void SetEnemyStarPosition(int index, Vector3 localPosition)
		{
			m_enemyStars[index].transform.localPosition = localPosition;
			m_enemyStars[index].SetBeamConnection();
		}

		[ContextMenu("Test Min Angle")]
		private void TestMinAngle()
		{
			ChangeAngleBounce(m_minAngle);
		}

		[ContextMenu("Test Max Angle")]
		private void TestMaxAngle()
		{
			ChangeAngleBounce(m_maxAngle);
		}

		#endregion
	}

}