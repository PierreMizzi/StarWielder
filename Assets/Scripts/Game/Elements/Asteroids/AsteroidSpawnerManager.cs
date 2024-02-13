using System.Collections.Generic;
using PierreMizzi.Useful;
using StarWielder.Gameplay.Player;
using UnityEngine;

// TODO : 🟥 Binary Space Tree when spawning asteroids ???

// TODO : 🟩 Spawn HealthFlower
// TODO : 🟩 Randomized asteroids visualy
//				- Color
//				- Sprite
//				- Shape ? (BSP ?)

// TODO : 🟥 Make asteroids interactable
// 				- Minable
//				- Hurts ship

public class AsteroidSpawnerManager : MonoBehaviour
{

	public void CreateAsteroidStorm()
	{
		SpawnAsteroids();
		SpawnHealthFlowers();
	}

	#region Voronoi Asteroids Spawning

	[Header("Asteroids")]
	[SerializeField] private AsteroidSpawningConfig m_currentConfig;
	[SerializeField] private List<Asteroid> m_asteroidPrefabs = new List<Asteroid>();

	private Asteroid tempAsteroidTemplate;
	private float tempScale;

	public void SpawnAsteroids()
	{
		Debug.Log("InstantiateAsteroids");
		m_positions.Clear();
		m_livableAsteroids.Clear();

		float indexX = 0;
		float indexY = 0;
		Vector3 pos = new Vector3();
		Vector3 offset = new Vector3();

		for (int x = 0; x < m_currentConfig.lengthAmountCell; x++)
		{
			List<Vector3> column = new List<Vector3>();
			pos.x = -m_currentConfig.cellSize * indexX;
			pos.x += m_currentConfig.startingPosition.x;

			for (int y = 0; y < m_currentConfig.widthAmountCell; y++)
			{
				// Pos
				pos.y = m_currentConfig.cellSize * indexY;
				pos.y += m_currentConfig.startingPosition.y;

				//Offset
				offset = GetRandomOffset();
				pos += offset;

				if (IsSpawning()) // && m_spawn
					SpawnAsteroid(pos);

				column.Add(new Vector3(pos.x, pos.y, 0));

				indexY++;
			}
			// Add a row
			m_positions.Add(column);
			indexX++;
			indexY = 0;
		}

	}

	public void SpawnAsteroid(Vector3 pos)
	{
		tempAsteroidTemplate = m_asteroidPrefabs.PickRandom();
		Asteroid asteroid = Instantiate(tempAsteroidTemplate, pos, Quaternion.identity);
		tempScale = m_currentConfig.GetRandomScale();
		asteroid.transform.localScale = new Vector3(tempScale, tempScale, 1);

		asteroid.Initialize(GetRandomizeVelocity(), m_currentConfig.GetRandomColor());

		if (IsLivable(pos))
			m_livableAsteroids.Add(asteroid);
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
		return Random.Range(0, 1f) < m_currentConfig.spawnPercentage;
	}

	#endregion

	#region Velocity

	private float m_tempRndAngle;
	private Vector2 m_tempRndDirection;
	private float m_tempRndStrength;

	public Vector3 GetRandomizeVelocity()
	{
		m_tempRndAngle = Random.Range(-m_currentConfig.randomVelocityAngle, m_currentConfig.randomVelocityAngle);
		m_tempRndDirection = new Vector2
		{
			x = Mathf.Cos(m_tempRndAngle * Mathf.Deg2Rad),
			y = Mathf.Sin(m_tempRndAngle * Mathf.Deg2Rad)
		};
		m_tempRndStrength = Random.Range(m_currentConfig.minVelocityScalar, m_currentConfig.maxVelocityScalar);

		return m_tempRndDirection * m_tempRndStrength;
	}

	#endregion

	#region Health Flower

	// By percentage ?
	// By defined amount ?

	[Header("Health Flower")]
	[SerializeField] private HealthFlower m_healthFlowerPrefab;
	[SerializeField] private float m_livableRange = 4;
	private List<Asteroid> m_livableAsteroids = new List<Asteroid>();

	private bool IsLivable(Vector3 pos)
	{
		return -m_livableRange < pos.y && pos.y < m_livableRange;
	}

	private void SpawnHealthFlowers()
	{
		Asteroid asteroid;
		Transform anchor;

		for (int i = 0; i < m_currentConfig.amountHealthFlower; i++)
		{
			asteroid = m_livableAsteroids.PickRandom();
			anchor = asteroid.healthFlowerAnchors.PickRandom();

			HealthFlower healthFlower = Instantiate(m_healthFlowerPrefab, anchor);
			healthFlower.transform.localPosition = Vector3.zero;
			healthFlower.transform.localRotation = Quaternion.identity;

			m_livableAsteroids.Remove(asteroid);
		}
	}

	#endregion

	#region Debug

	[Header("Debug")]
	[SerializeField] private Color m_gizmosColor;
	[SerializeField] private float m_gizmosSize;
	private List<List<Vector3>> m_positions = new List<List<Vector3>>();
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

}