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

	private void Awake()
	{
		Debug.Log("Awake");
		m_line.material = new Material(m_line.material);
		m_line.material.name = "TonFils";
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
		ComputeFollowing();
		UpdatePollenContainer();
	}

	#endregion

	#region Shine Strength

	/*
		A conversion is needed between ShineStrength and Temperature
		I just realized I should have used temperature since the beginning 

		m_temperatureControl = smoothlezrep
	*/

	[SerializeField] private AnimationCurve m_temperatureControl;

	public void SetTemperature(float temperature)
	{
		m_line.material.SetFloat("_Temperature", m_temperatureControl.Evaluate(temperature));
	}

	#endregion

	#region Following

	private void ComputeFollowing()
	{
		Vector3 localTargetPos = transform.InverseTransformPoint(m_target.position);

		if (Vector3.Dot(transform.up, localTargetPos) < 0.33)
			return;

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