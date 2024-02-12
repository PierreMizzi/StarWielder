using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawnerManager : MonoBehaviour
{
	[SerializeField] private AsteroidSpawningConfig m_currentConfig;

	[ContextMenu("Debug Voronoi Placement")]
	public void DebugVoronoiPlacement()
	{
		m_spawn = true;
		InstantiateAsteroids();
	}

	#region Asteroids

	[HideInInspector]
	[SerializeField] private List<Asteroid> m_asteroids = new List<Asteroid>();

	private float m_tempRndAngle;
	private Vector2 m_tempRndDirection;
	private float m_tempRndStrength;

	public Vector3 GetRandomizeVelocity()
	{
		m_tempRndAngle = UnityEngine.Random.Range(-m_currentConfig.randomAngleRange, m_currentConfig.randomAngleRange);
		m_tempRndDirection = new Vector2
		{
			x = Mathf.Cos(m_tempRndAngle * Mathf.Deg2Rad),
			y = Mathf.Sin(m_tempRndAngle * Mathf.Deg2Rad)
		};
		m_tempRndStrength = UnityEngine.Random.Range(m_currentConfig.minStrength, m_currentConfig.maxStrength);

		return m_tempRndDirection * m_tempRndStrength;
	}

	#endregion

	#region Voronoi Placement

	[Header("VoronoiPlacement")]

	[SerializeField] private AsteroidSpawningConfig m_spawningConfig;

	private List<List<Vector3>> m_positions = new List<List<Vector3>>();

	[SerializeField] private Asteroid m_asteroidPrefab;
	[SerializeField] private bool m_spawn = false;

	[ContextMenu("Instantiate Asteroids")]
	public void InstantiateAsteroids()
	{
		Debug.Log("InstantiateAsteroids");
		m_positions.Clear();

		float indexX = 0;
		float indexY = 0;
		Vector3 pos = new Vector3();
		Vector3 offset = new Vector3();

		for (int x = 0; x < m_spawningConfig.lengthAmountCell; x++)
		{
			List<Vector3> column = new List<Vector3>();
			pos.x = -m_currentConfig.cellSize * indexX;
			pos.x += m_currentConfig.startingPosition.x;

			for (int y = 0; y < m_spawningConfig.widthAmountCell; y++)
			{
				// Pos
				pos.y = m_currentConfig.cellSize * indexY;
				pos.y += m_currentConfig.startingPosition.y;

				//Offset
				offset = GetRandomOffset();
				pos += offset;

				if (IsSpawning()) // && m_spawn
					InstantiateAsteroid(pos);

				column.Add(new Vector3(pos.x, pos.y, 0));

				indexY++;
			}
			// Add a row
			m_positions.Add(column);
			indexX++;
			indexY = 0;
		}
	}

	public Vector3 GetRandomOffset()
	{
		float randomAngle = UnityEngine.Random.Range(0f, 360f);
		Vector3 rndOffset = new Vector2
		{
			x = Mathf.Cos(randomAngle * Mathf.Deg2Rad),
			y = Mathf.Sin(randomAngle * Mathf.Deg2Rad)
		};

		float rndOffsetDistance = UnityEngine.Random.Range(m_currentConfig.minOffsetDistance, m_currentConfig.maxOffsetDistance);
		// Debug.Log(rndOffsetDistance);
		return rndOffset * rndOffsetDistance;
	}

	public bool IsSpawning()
	{
		return UnityEngine.Random.Range(0, 1f) < m_currentConfig.spawnPercentage;
	}

	public void InstantiateAsteroid(Vector3 pos)
	{
		Asteroid asteroid = Instantiate(m_asteroidPrefab, pos, Quaternion.identity);
		asteroid.Initialize(GetRandomizeVelocity());
	}

	public void SpawnAsteroids()
	{
	}

	#endregion

	#region Debug

	#region Debug

	[Header("Debug")]
	[SerializeField] private Color m_gizmosColor;
	[SerializeField] private float m_gizmosSize;
	Color defaultGizmosColor;

	protected void OnDrawGizmos()
	{
		defaultGizmosColor = Gizmos.color;
		Gizmos.color = m_gizmosColor;

		Vector3 pos = new Vector3();
		for (int x = 0; x < m_positions.Count; x++)
		{
			for (int y = 0; y < m_positions[x].Count; y++)
			{
				pos = m_positions[x][y];
				Gizmos.DrawWireSphere(pos, m_gizmosSize);
			}
		}
		// My Gizmos ...
		Gizmos.color = defaultGizmosColor;
	}
	#endregion

	#endregion

}