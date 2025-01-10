using System.Collections.Generic;
using UnityEngine;
using PierreMizzi.Useful.PoolingObjects;

namespace StarWielder.Gameplay.Elements
{

	[RequireComponent(typeof(Rigidbody2D))]
	public class Asteroid : MonoBehaviour, IAsteroidStormElement
	{

		[SerializeField] private PoolingChannel m_poolingChannel;
		[SerializeField] private SpriteRenderer m_spriteRenderer;

		private AsteroidSpawnerManager m_manager;

		public void Initialize(AsteroidSpawnerManager manager, Vector3 direction, Color color)
		{
			m_manager = manager;
			rigidbody2D.velocity = direction;
			m_spriteRenderer.color = color;
		}

		private void Awake()
		{
			rigidbody2D = GetComponent<Rigidbody2D>();
		}

		private void Update()
		{
			if (CheckOutOfBounds())
			{
				DestroyOutOfBounds();
			}
		}

		#region IAsteroidStormElement

		public AsteroidSpawnerManager manager { get; set; }
		public new Rigidbody2D rigidbody2D { get; set; }

		public bool CheckOutOfBounds()
		{
			if (manager == null)
			{
				return false;
			}

			return transform.position.x > manager.currentConfig.boundLimits;
		}

		public void DestroyOutOfBounds()
		{
			m_manager.ReduceAsteroidCount();
			m_poolingChannel.onReleaseToPool.Invoke(gameObject);

			if (healthFlower != null)
			{
				m_poolingChannel.onReleaseToPool.Invoke(healthFlower.gameObject);
				healthFlower = null;
			}

			if (mineral != null)
			{
				m_poolingChannel.onReleaseToPool.Invoke(mineral.gameObject);
				mineral = null;
			}
		}

		#endregion

		#region Health Flower

		[Header("Health Flower")]
		[SerializeField] private List<Transform> m_healthFlowerAnchors = new List<Transform>();
		public List<Transform> healthFlowerAnchors { get { return m_healthFlowerAnchors; } }

		[HideInInspector] public HealthFlower healthFlower;

		#endregion

		#region Mineral

		[SerializeField] private List<Transform> m_mineralAnchors = new List<Transform>();
		public List<Transform> mineralAnchors { get { return m_mineralAnchors; } }


        [HideInInspector] public Mineral mineral;

		#endregion



	}
}