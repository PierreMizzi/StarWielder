using System.Collections.Generic;
using UnityEngine;

namespace StarWielder.Gameplay.Enemies
{
	/// <summary>
	/// Base class for enemies. EnemyGroup are composed of EnemyStars, EnemyShips & EnemyTurrets
	/// </summary>
	public class EnemyGroup : MonoBehaviour
	{

		#region Main

		private List<EnemyStar> m_enemyStars = new List<EnemyStar>();
		private EnemyManager m_manager;
		public EnemyManager manager => m_manager;

		public void Initialize(EnemyManager manager)
		{
			m_manager = manager;

			Awake();

			foreach (Transform child in transform)
			{
				if (child.TryGetComponent(out EnemyStar star))
				{
					star.Initialize(this);
					m_enemyStars.Add(star);
				}
				else if (child.TryGetComponent(out EnemyTurret turret))
				{
					turret.Initialize(this);
					m_turrets.Add(turret);
				}
			}
		}

		public void EnemyStarDestroyed(EnemyStar enemyStar)
		{
			if (m_enemyStars.Contains(enemyStar))
				m_enemyStars.Remove(enemyStar);

			if (m_enemyStars.Count == 0)
				Destroy(gameObject);
		}

		#endregion

		#region MonoBehaviour

		private void Awake()
		{
			m_area = GetComponent<Collider2D>();
			m_animator = GetComponent<Animator>();
		}

		private void OnDestroy()
		{
			m_manager.RemoveEnemyGroup(this);
		}

		#endregion

		#region Area & Spawning

		[Header("Area & Spawning")]
		private Collider2D m_area;
		[SerializeField] private ContactFilter2D m_overlapingFilter;


		[ContextMenu("CheckIsOverlaping")]
		public bool IsOverlaping()
		{
			List<Collider2D> results = new List<Collider2D>();
			int resultsLength = m_area.OverlapCollider(m_overlapingFilter, results);
			// Debug.Log(resultsLength);
			return resultsLength > 0;
		}

		#endregion

		#region Turrets

		private List<EnemyTurret> m_turrets = new List<EnemyTurret>();
		private void ActivateTurrets()
		{
			foreach (EnemyTurret turret in m_turrets)
				turret.Activate();
		}

		public void DeactivateTurrets()
		{
			foreach (EnemyTurret turret in m_turrets)
				turret.Deactivate();
		}

		#endregion

		#region Animations

		private Animator m_animator = null;

		private const string k_triggerAppear = "Appear";
		private const string m_triggerDestroy = "Destroy";

		public void Appear()
		{
			m_animator.SetTrigger(k_triggerAppear);
		}

		public void AnimEventAppearCompleted()
		{
			ActivateTurrets();
		}

		#endregion


	}
}