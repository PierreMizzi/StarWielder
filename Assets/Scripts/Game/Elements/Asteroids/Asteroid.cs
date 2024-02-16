using System.Collections.Generic;
using UnityEngine;
using PierreMizzi.Useful.PoolingObjects;

namespace StarWielder.Gameplay.Elements
{

	[RequireComponent(typeof(Rigidbody2D))]
	public class Asteroid : MonoBehaviour
	{

		[SerializeField] private PoolingChannel m_poolingChannel;
		[SerializeField] private SpriteRenderer m_spriteRenderer;

		private Rigidbody2D m_rigidbody;

		private AsteroidSpawnerManager m_manager;

		private void Awake()
		{
			m_rigidbody = GetComponent<Rigidbody2D>();
		}

		private void Update()
		{
			// TODO : 🟥 Make it better, maybe ?
			if (transform.position.x > 15)
			{
				Kill();
			}
		}

		public void Initialize(AsteroidSpawnerManager manager, Vector3 direction, Color color)
		{
			m_manager = manager;
			m_rigidbody.velocity = direction;
			m_spriteRenderer.color = color;
		}

		public void Kill()
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