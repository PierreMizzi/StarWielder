using System.ComponentModel;
using PierreMizzi.Useful;
using UnityEngine;

public class EnemySpawnerEdge : EnemySpawner
{

	#region MonoBehaviour

	private void OnDrawGizmos()
	{
		DrawEdgesGizmos();
	}

	#endregion

	#region Position

	[Header("Position")]
	// TODO : Make sure min can't go above max, and max can't go below min
	[SerializeField] private float m_minHorizontal = 5f;
	[SerializeField] private float m_maxHorizontal = 7f;
	[SerializeField] private float m_minVertical = 4f;
	[SerializeField] private float m_maxVertical = 4.5f;

	[SerializeField] private Color m_edgesColor;

	private Vector3 minEdgesCube;
	private Vector3 maxEdgesCube;

	private void DrawEdgesGizmos()
	{
		Gizmos.color = m_edgesColor;
		minEdgesCube.Set(m_minHorizontal, m_minVertical, 0f);
		maxEdgesCube.Set(m_maxHorizontal, m_maxVertical, 0f);

		minEdgesCube *= 2f;
		maxEdgesCube *= 2f;

		Gizmos.DrawWireCube(Vector3.zero, minEdgesCube);
		Gizmos.DrawWireCube(Vector3.zero, maxEdgesCube);
	}

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

	protected override Quaternion GetRandomRotation()
	{
		float angle = 0f;
		Vector3 directionToCenter = -transform.position.normalized;
		angle = Mathf.Atan2(directionToCenter.y, directionToCenter.x) * Mathf.Rad2Deg;

		int plusOrMinus = (Random.Range(0, 2) == 0) ? 1 : -1;

		angle += plusOrMinus * Random.Range(0f, m_maxRandomAngle);
		Vector3 randomEuler = new Vector3(0f, 0f, angle);
		return Quaternion.Euler(randomEuler);
	}

	#endregion







}