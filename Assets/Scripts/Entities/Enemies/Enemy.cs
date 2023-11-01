using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	private EnemyGroup m_group;

	[SerializeField] private float m_energy = 1f;

	public float energy => m_energy;

	private void OnDestroy()
	{
		m_group.EnemyDestroyed(this);
	}

	public void Initialize(EnemyGroup group)
	{
		m_group = group;
	}
}