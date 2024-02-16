using UnityEngine;
using System;

[ExecuteInEditMode]
public class Stem : MonoBehaviour
{

	[SerializeField] private Transform m_target;

	[SerializeField] private int m_amountPoints = 2;

	private float m_percentPerPoint;

	[SerializeField] private LineRenderer m_line;

	[SerializeField] private float m_maxMagnitude;
	[SerializeField] private float m_normalMagnitude;

	#region MonoBehaviour

	private void OnEnable()
	{
		m_percentPerPoint = 1f / m_amountPoints;
		m_line.positionCount = m_amountPoints;
	}

	private void OnValidate()
	{
		if (m_line.positionCount != m_amountPoints)
			m_line.positionCount = m_amountPoints;
	}

	private void Update()
	{
		ComputeFollowing();
	}

	#endregion

	#region Following



	private void ComputeFollowing()
	{
		Vector3 localTargetPos = transform.InverseTransformPoint(m_target.position);
		float magnitude = localTargetPos.magnitude;
		Vector3 interpolatedPosition;
		Vector3 upAxisPosition;
		Vector3 upAxisToInterpolated;
		float rigidity;

		if (magnitude > m_maxMagnitude)
			localTargetPos = localTargetPos.normalized * m_maxMagnitude;
		else
			localTargetPos *= m_normalMagnitude;


		for (int i = 0; i < m_amountPoints; i++)
		{
			interpolatedPosition = localTargetPos * (i * m_percentPerPoint);
			upAxisPosition = new Vector3(0, interpolatedPosition.y, 0);
			upAxisToInterpolated = interpolatedPosition - upAxisPosition;

			rigidity = m_rigidityCurve.Evaluate(i * m_percentPerPoint);
			m_line.SetPosition(i, upAxisPosition + upAxisToInterpolated * rigidity);
		}

	}

	#endregion

	#region Rigidity
	[Header("Rigidity")]

	[SerializeField] private AnimationCurve m_rigidityCurve;

	#endregion

	#region Name

	#endregion

}