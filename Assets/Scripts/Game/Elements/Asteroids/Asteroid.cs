using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{
	#region Movements

	private Rigidbody2D m_rigidbody;

	private void Awake()
	{
		m_rigidbody = GetComponent<Rigidbody2D>();
	}

	public void Initialize(Vector3 direction)
	{
		m_rigidbody.velocity = direction;
	}

	#endregion
}