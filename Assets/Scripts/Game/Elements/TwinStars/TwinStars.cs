using PierreMizzi.Useful.PoolingObjects;
using StarWielder.Gameplay.Enemies;
using StarWielder.Tools;
using UnityEngine;

namespace StarWielder.Gameplay.Elements
{
	public class TwinStars : OvalOrbit, IAsteroidStormElement
	{
		#region Behaviour

		[SerializeField] private PoolingChannel m_poolingChannel;

		[SerializeField] private EnemyStar m_enemyStarPrefab;

		private EnemyStar m_starA;
		private EnemyStar m_starB;

		public void Initialize(AsteroidSpawnerManager manager, Vector3 velocity)
		{
			// Orbit
			SetRandomOrbitTimeOffset();

			// Twin Stars
			this.manager = manager;

			PoolEnemyStar(m_pointA);
			PoolEnemyStar(m_pointB);

			rigidbody2D = GetComponent<Rigidbody2D>();
			rigidbody2D.velocity = velocity;
		}

		#endregion

		#region MonoBehaviour

		protected override void Update()
		{
			base.Update();

			if (CheckOutOfBounds())
			{
				DestroyOutOfBounds();
			}
		}
			
		#endregion

		#region Stars

		private EnemyStar PoolEnemyStar(Transform point)
		{
			GameObject pooledObject = m_poolingChannel.onGetFromPool(m_enemyStarPrefab.gameObject);

			if (pooledObject == null)
			{
				return null;
			}

			if (pooledObject.TryGetComponent(out EnemyStar star))
			{
				star.transform.SetParent(point);
				star.transform.localPosition = Vector3.zero;
				star.QuickAppear();
				star.SetInteractable();
				return star; 
			}
			else
			{
				return null;
			}
		}

		private void ReleaseEnemyStar(EnemyStar star)
		{
			if (star != null)
			{
				m_poolingChannel.onReleaseToPool.Invoke(star.gameObject);
				star = null;
			}
		}

		#endregion

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
			m_poolingChannel.onReleaseToPool.Invoke(gameObject);

			ReleaseEnemyStar(m_starA);
			ReleaseEnemyStar(m_starB);
		}


        #endregion
    }
}