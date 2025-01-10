using UnityEngine;

public class Orbiter : MonoBehaviour
{
	#region Settings

	[Header("Settings")]

	[SerializeField] private Transform m_center;
	[SerializeField] private float m_speed;
	[SerializeField] protected float m_orbitHorizontalLength = 1f;
	[SerializeField] protected float m_orbitVerticalLength = 2f;

	[SerializeField] private bool m_selfControlled = true;

	#endregion

	#region Behaviour

	protected float m_time = 0;
	protected Vector3 m_position;

	protected virtual void UpdatePointPosition(float time)
	{
		m_position.x = Mathf.Cos(time) * m_orbitHorizontalLength;
		m_position.y = Mathf.Sin(time) * m_orbitVerticalLength;
		transform.position = m_center.position + m_position;
	}

	protected virtual void SetRandomOrbitTimeOffset()
	{
		m_time = Random.Range(0, Mathf.PI * 2f);
	}

	#endregion

	#region MonoBehaviour

	private void Start()
	{
		SetRandomOrbitTimeOffset();
	}

	private void Update()
	{
		if (m_center == null)
		{
			return;
		}

		if (m_selfControlled)
		{
			m_time += Time.deltaTime * m_speed;
			UpdatePointPosition(m_time);
		}
	}

	#endregion
}