using System;
using System.Collections.Generic;
using PierreMizzi.Useful;
using UnityEngine;

[RequireComponent(typeof(CyclicTimer))]
public class AsteroidSpawnerManager : MonoBehaviour
{
	[SerializeField] private List<AsteroidSpawner> m_spawners = new List<AsteroidSpawner>();
	private CyclicTimer m_cyclicTimer;

	#region MonoBehaviour

	private void Awake()
	{
		m_cyclicTimer = GetComponent<CyclicTimer>();
		m_cyclicTimer.StartBehaviour();
	}

	private void Start()
	{
		if (m_cyclicTimer.onCycleCompleted != null)
			m_cyclicTimer.onCycleCompleted += SpawnAsteroid;
	}

	private void OnDestroy()
	{
		if (m_cyclicTimer.onCycleCompleted != null)
			m_cyclicTimer.onCycleCompleted -= SpawnAsteroid;
	}

	#endregion

	private void SpawnAsteroid()
	{
		AsteroidSpawner spawner = m_spawners.PickRandom();
		spawner.SpawnAsteroid();
	}

}