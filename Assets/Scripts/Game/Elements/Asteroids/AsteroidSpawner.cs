
using System;
using DG.Tweening;
using PierreMizzi.Useful;
using UnityEngine;

[RequireComponent(typeof(CyclicTimer))]
public class AsteroidSpawner : MonoBehaviour
{

	#region Spawner Settings

	[SerializeField] private float m_spawnRange = 1f;
	[SerializeField] private float m_spawnDelay = 1f;
	[SerializeField] private float m_spawnFrequency = 1f;
	// private CyclicTimer m_cyclicTimer;

	#endregion

	#region MonoBehaviour

	// private void Awake()
	// {
	// 	StartSpawning(0);
	// }

	// private void Start()
	// {
	// 	if (m_cyclicTimer.onCycleCompleted != null)
	// 		m_cyclicTimer.onCycleCompleted += SpawnAsteroid;
	// }

	// private void OnDestroy()
	// {
	// 	if (m_cyclicTimer.onCycleCompleted != null)
	// 		m_cyclicTimer.onCycleCompleted -= SpawnAsteroid;
	// }

	#endregion

	#region Spawning
	[SerializeField] private Asteroid m_asteroidPrefab;

	public void StartSpawning(float delay)
	{
		// DOVirtual.DelayedCall(delay, m_cyclicTimer.StartBehaviour);
	}

	public void SpawnAsteroid()
	{
		Vector3 randomPosition = GetRandomPosition();
		Quaternion randomRotation = UtilsClass.RandomRotation2D();
		Asteroid asteroid = Instantiate(m_asteroidPrefab, randomPosition, randomRotation);
		asteroid.Initialize(transform.up);
	}

	private Vector3 GetRandomPosition()
	{
		return transform.position + transform.right * UnityEngine.Random.Range(0, m_spawnRange);
	}

	#endregion

	#region Debug

	[Header("Debug")]
	Color defaultGizmosColor;
	private

	protected void OnDrawGizmos()
	{
		defaultGizmosColor = Gizmos.color;
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, transform.position + transform.right * m_spawnRange);
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, transform.position + transform.up * 0.5f);
		Gizmos.color = defaultGizmosColor;
	}
	#endregion

}