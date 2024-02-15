using UnityEngine;
using System;

[ExecuteInEditMode]
public class Stem : MonoBehaviour
{

	[SerializeField] private Transform m_target;

	[SerializeField] private int m_amountPoints;

	private float m_percentPerPoint;

	[SerializeField] private LineRenderer m_line;

	[SerializeField] private float m_maxLength;

	#region MonoBehaviour

	private void OnEnable()
	{
		m_percentPerPoint = 1f / m_amountPoints;
	}

	private void Update()
	{
		ComputeFollowing();
	}

	#endregion

	#region Following

	private void ComputeFollowing()
	{
		// Vector3 localTargetPos = transform.InverseT(m_target.position);
		// Vector3 rootToTarget = m_target.transform.position.normalized;
		// rootToTarget *= m_maxLength;

		// Vector3 point;
		// for (int i = 0; i < m_amountPoints; i++)
		// {
		// 	m_line.SetPositions[i] = rootToTarget * (i * m_percentPerPoint);
		// }

	}

	#endregion

	#region Rigidity

	#endregion

}