using UnityEngine;
using System;

[ExecuteInEditMode]
public class Stem : MonoBehaviour
{

	[SerializeField] private int m_amountPoints = 2;

	private float m_percentPerPoint;

	[SerializeField] private LineRenderer m_line;

	[SerializeField] private float m_maxMagnitude;
	[SerializeField] private float m_normalMagnitude;

	#region MonoBehaviour

	private void Awake()
	{
		m_line.material = new Material(m_line.material);
		m_currentTargetPosition = m_restingTargetPosition;
	}

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
		if (m_starTransform != null)
		{
			ComputeFollowing();
			UpdatePollenContainer();
		}
	}

	#endregion

	#region Temperature

	/*
		A conversion is needed between ShineStrength and Temperature
		I just realized I should have used temperature since the beginning 

		m_temperatureControl = smoothlezrep
	*/

	public void SetTemperature(float temperature)
	{
		m_line.material.SetFloat("_Temperature", temperature);
	}

	#endregion

	#region Following

	[Header("Following")]
	[SerializeField] private float m_followingSpeed = 0.1f;
	[SerializeField] private Vector3 m_restingTargetPosition;
	[SerializeField] private float m_restingTreshold = 0.33f;
	private Vector3 m_targetPosition;
	private Vector3 m_currentTargetPosition;
	private Transform m_starTransform;

	public void SetStarTransform(Transform starTransform)
	{
		m_starTransform = starTransform;
	}

	private void ComputeFollowing()
	{
		m_targetPosition = transform.InverseTransformPoint(m_starTransform.position);

		float magnitude = m_targetPosition.magnitude;

		if (magnitude > m_maxMagnitude)
			m_targetPosition = m_targetPosition.normalized * m_maxMagnitude;
		else
			m_targetPosition *= m_normalMagnitude;

		if (Vector3.Dot(transform.up, m_targetPosition) < m_restingTreshold)
			m_targetPosition = m_restingTargetPosition;

		m_currentTargetPosition = Vector3.Lerp(m_currentTargetPosition, m_targetPosition, Time.deltaTime * m_followingSpeed);

		Vector3 interpolatedPosition;
		Vector3 upAxisPosition;
		Vector3 upAxisToInterpolated;
		float rigidity;

		for (int i = 0; i < m_amountPoints; i++)
		{
			interpolatedPosition = m_currentTargetPosition * (i * m_percentPerPoint);
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

	#region Pollen Transform

	[SerializeField] private Transform m_pollenContainer;

	public Transform pollenContainer => m_pollenContainer;

	private Vector3 m_lastPosition;
	private Vector3 m_sndLastPosition;

	public void UpdatePollenContainer()
	{
		m_lastPosition = m_line.GetPosition(m_line.positionCount - 1);
		m_sndLastPosition = m_line.GetPosition(m_line.positionCount - 2);

		m_pollenContainer.position = m_lastPosition;
		m_pollenContainer.up = (m_lastPosition - m_sndLastPosition).normalized;
	}

	#endregion

}