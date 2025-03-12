using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Orbiter : MonoBehaviour
{

	#region Settings

	[Header("Settings")]

	public Transform center;
	public float radius = 1f;
	public bool selfControlled = true;
	public bool useRandomTimeAtStart = true;
	public bool isClockwise;

	#endregion

	#region Behaviour

	protected float m_time = 0;
	protected float m_speed;
	protected Vector3 m_position;

	public float direction
	{
		get { return isClockwise ? -1 : 1; }
	}

	public virtual void UpdatePosition()
	{
		m_position.x = Mathf.Cos(m_time * direction) * radius;
		m_position.y = Mathf.Sin(m_time * direction) * radius;
		transform.position = center.position + m_position;
	}

	public virtual void UpdatePosition(float time)
	{
		m_position.x = Mathf.Cos(time * direction) * radius;
		m_position.y = Mathf.Sin(time * direction) * radius;
		transform.position = center.position + m_position;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="degrees">On Trigonometry Circle</param>
	public virtual void SetTime(float degrees)
	{
		m_time = degrees * Mathf.Deg2Rad;
	}

	protected virtual void SetRandomTime()
	{
		m_time = Random.Range(0, Mathf.PI * 2f);
	}

	#endregion

	#region MonoBehaviour

	[SerializeField, Range(0.1f, 10000)] private float m_orbitDuration = 1f;


	private void Start()
	{
		m_testAngle = false;
		m_speed = MathF.PI * 2F / m_orbitDuration;

		if (useRandomTimeAtStart)
		{
			SetRandomTime();
		}
	}

	private void Update()
	{

		if (center == null)
		{
			return;
		}

		if (selfControlled)
		{
			m_time += Time.deltaTime * m_speed;
			UpdatePosition(m_time);
		}
	}

    private void OnValidate()
    {
		if (center == null || m_testAngle == false)
		{
			return;
		}
		TestAngle();
	}

    #endregion

    #region Debug
    [Header("Debug")]
	[SerializeField] private bool m_testAngle;
	[SerializeField, Range(0, 360f)] private float m_angle;

	[ContextMenu("Call Test Angle")]
	public void TestAngle()
	{
		SetTime(m_angle);
		UpdatePosition(m_time);
	}
		
	#endregion
}