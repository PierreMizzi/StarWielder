using UnityEngine;
using System;

[ExecuteInEditor]
public class Stem : MonoBehaviour
{

	[SerializeField] private Transform m_target;

	[SerializeField] private int m_amountPoints;

	private float m_percentPerPoint;

	[SerializeField] private Trail m_trail;

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
		Vector3 localTargetPos = transform.Inverse(m_target.position);
		Vector3 rootToTarget = m_target.transform.position.normalized;
		rootToTarget *= m_maxLength;

		Vector3 point;
		for (int i = 0; i < m_amountPoints; i++)
		{
			m_trail.points[i] = rootToTarget * (i * m_percentPerPoint);
		}

	}

	#endregion

	#region Rigidity

	#endregion

}