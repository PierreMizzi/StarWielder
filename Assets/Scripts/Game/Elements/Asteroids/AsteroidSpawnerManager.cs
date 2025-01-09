using System.Collections.Generic;
using PierreMizzi.Useful;
using UnityEngine;
using PierreMizzi.Useful.PoolingObjects;
using System;

using Random = UnityEngine.Random;


// TODO : 🟥 Binary Space Tree when spawning asteroids ???

// TODO : 🟥 Make asteroids interactable
// 				- Minable
//				- Hurts ship

// Wanna add suns as well with asteroids

namespace StarWielder.Gameplay.Elements
{
	public class AsteroidSpawnerManager : MonoBehaviour
	{

		public void CreateAsteroidStorm()
		{
			CreateSpots();

			SpawnAsteroids();
			SpawnHealthFlowers();
			SpawnMinerals();

			SpawnTwinStars();
		}

		[SerializeField] private ResourcesStageManager m_manager;
		[SerializeField] private PoolingChannel m_poolingChannel;

		[SerializeField] private AsteroidStormSettings m_currentConfig;
		public AsteroidStormSettings currentConfig => m_currentConfig;

		#region Voronoi Position Spawning

		public List<AsteroidStormSpot> m_spots = new List<AsteroidStormSpot>();

		private void CreateSpots()
		{
			m_spots.Clear();

			float indexX = 0;
			float indexY = 0;
			Vector3 pos = new Vector3();
			Vector3 offset = new Vector3();

			for (int x = 0; x < m_currentConfig.lengthAmountCell; x++)
			{
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

					m_spots.Add(new AsteroidStormSpot(new Vector3(pos.x, pos.y, 0)));

					indexY++;
				}
				// Add a row
				indexX++;
				indexY = 0;
			}
		}

		public List<AsteroidStormSpot> GetAvailableSpots()
		{
			return m_spots.FindAll(item => item.isTaken == false);
		}
		
		#endregion

		#region Asteroids

		[Header("Asteroids")]
		[SerializeField] private List<Asteroid> m_asteroidPrefabs = new List<Asteroid>();
		private List<Asteroid> m_asteroids = new List<Asteroid>();
		private int m_asteroidCount;

		private Asteroid tempAsteroidTemplate;
		private float tempScale;

		public void SpawnAsteroids()
		{
			List<AsteroidStormSpot> spots = GetAvailableSpots();

			foreach (AsteroidStormSpot spot in spots)
			{
				if (CheckSpawnChance(m_currentConfig.asteroidSpawnChance))
				{
					SpawnAsteroid(spot.position);
					spot.isTaken = true;
				}
			}
		}

		public void SpawnAsteroid(Vector3 pos)
		{
			// Pool
			tempAsteroidTemplate = m_asteroidPrefabs.PickRandom();
			Asteroid asteroid = m_poolingChannel.onGetFromPool.Invoke(tempAsteroidTemplate.gameObject).GetComponent<Asteroid>();

			// Set Transform
			asteroid.transform.position = pos;
			asteroid.transform.rotation = Quaternion.identity;
			tempScale = m_currentConfig.GetRandomScale();
			asteroid.transform.localScale = new Vector3(tempScale, tempScale, 1);

			// Initialize
			asteroid.Initialize(this, GetRandomizeVelocity(), m_currentConfig.GetMineralRandomColor());

			m_asteroidCount++;
			m_asteroids.Add(asteroid);

			if (IsLivable(pos))
				m_livableAsteroids.Add(asteroid);
		}

		public void ReduceAsteroidCount()
		{
			m_asteroidCount--;

			if (m_asteroidCount == 0)
				m_manager.StopStage();
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
			return rndOffset * rndOffsetDistance;
		}

		public bool CheckSpawnChance(float spawnChance)
		{
			return Random.Range(0, 1f) < spawnChance;
		}

		#endregion

		#region Mineral Generation

		[SerializeField] private List<Mineral> m_mineralPrefabs = new List<Mineral>();

		private void SpawnMinerals()
		{
			List<Asteroid> m_copiedAsteroids = new List<Asteroid>(m_asteroids);

			int length = m_asteroids.Count;
			Asteroid asteroid;
			Transform anchor;
			GameObject mineralPrefab;

			for (int i = 0; i < length; i++)
			{
				if (CheckSpawnChance(m_currentConfig.mineralSpawnChance))
				{
					asteroid = m_copiedAsteroids.PickRandom();
					m_copiedAsteroids.Remove(asteroid);

					anchor = asteroid.mineralAnchors.PickRandom();

					mineralPrefab = m_mineralPrefabs.PickRandom().gameObject;
					Mineral mineral = m_poolingChannel.onGetFromPool.Invoke(mineralPrefab).GetComponent<Mineral>();
					mineral.transform.parent = anchor;
					mineral.transform.localPosition = Vector3.zero;
					mineral.transform.localRotation = Quaternion.identity;

					asteroid.mineral = mineral;
				}
			}
		}


		#endregion

		#region Velocity

		private float m_tempRndAngle;
		private Vector2 m_tempRndDirection;
		private float m_tempRndStrength;

		public Vector3 GetRandomizeVelocity()
		{
			m_tempRndAngle = Random.Range(-m_currentConfig.asteroidRndVelocityAngle, m_currentConfig.asteroidRndVelocityAngle);
			m_tempRndDirection = new Vector2
			{
				x = Mathf.Cos(m_tempRndAngle * Mathf.Deg2Rad),
				y = Mathf.Sin(m_tempRndAngle * Mathf.Deg2Rad)
			};
			m_tempRndStrength = Random.Range(m_currentConfig.asteroidMinVelocity, m_currentConfig.asteroidMaxVelocity);

			return m_tempRndDirection * m_tempRndStrength;
		}

		#endregion

		#region Health Flower

		[Header("Health Flower")]
		[SerializeField] private HealthFlower m_healthFlowerPrefab;
		private List<Asteroid> m_livableAsteroids = new List<Asteroid>();

		private bool IsLivable(Vector3 pos)
		{
			return -m_currentConfig.healthFlowerLivableRange < pos.y && pos.y < m_currentConfig.healthFlowerLivableRange;
		}

		private void SpawnHealthFlowers()
		{
			Asteroid asteroid;
			Transform anchor;
			HealthFlower healthFlower;

			for (int i = 0; i < m_currentConfig.healthFlowerAmount; i++)
			{
				asteroid = m_livableAsteroids.PickRandom();
				m_livableAsteroids.Remove(asteroid);

				anchor = asteroid.healthFlowerAnchors.PickRandom();

				healthFlower = m_poolingChannel.onGetFromPool.Invoke(m_healthFlowerPrefab.gameObject).GetComponent<HealthFlower>();
				healthFlower.transform.parent = anchor;
				healthFlower.transform.localPosition = Vector3.zero;
				healthFlower.transform.localRotation = Quaternion.identity;

				asteroid.healthFlower = healthFlower;
			}
		}

		#endregion

		#region Twin Stars

		[SerializeField] private TwinStars m_twinStarsprefab;

		private void SpawnTwinStars()
		{
			List<AsteroidStormSpot> spots = GetAvailableSpots();

			// int rndTwinStarsCount = 
			// 🟥 : DO THIS !

			foreach (AsteroidStormSpot spot in spots)
			{
				if (CheckSpawnChance(m_currentConfig.twinStarsSpawnChance))
				{
					SpawnTwinStars(spot.position);
					spot.isTaken = true;
				}
			}
		}

		private void SpawnTwinStars(Vector3 position)
		{
			// Pool
			GameObject pooledObject = m_poolingChannel.onGetFromPool.Invoke(m_twinStarsprefab.gameObject);

			if (pooledObject == null)
			{
				return;
			}

			if (pooledObject.TryGetComponent(out TwinStars twinStars))
			{
				twinStars.transform.position = position;
				twinStars.transform.rotation = Quaternion.identity;

				twinStars.Initialize(this, GetRandomizeVelocity());
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
}