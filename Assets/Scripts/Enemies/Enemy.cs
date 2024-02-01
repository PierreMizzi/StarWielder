using System.Collections.Generic;
using UnityEngine;

namespace StarWielder.Gameplay.Enemies
{
	[RequireComponent(typeof(Collider2D))]
	[RequireComponent(typeof(Animator))]
	public class Enemy : MonoBehaviour
	{
		protected EnemyManager m_manager;
		public EnemyManager manager => m_manager;

		public virtual void Initialize(EnemyManager manager)
		{
			m_manager = manager;
		}

		protected virtual void Awake()
		{
			m_area = GetComponent<Collider2D>();
			m_animator = GetComponent<Animator>();
		}

		protected virtual void OnDestroy() { }

		#region Area & Spawning

		[Header("Area & Spawning")]
		protected Collider2D m_area;
		[SerializeField] protected ContactFilter2D m_overlapingFilter;

		[ContextMenu("CheckIsOverlaping")]
		public virtual bool IsOverlaping()
		{
			List<Collider2D> results = new List<Collider2D>();
			int resultsLength = m_area.OverlapCollider(m_overlapingFilter, results);
			// Debug.Log(resultsLength);
			return resultsLength > 0;
		}

		#endregion

		#region Animations

		protected Animator m_animator = null;

		protected const string k_triggerAppear = "Appear";
		protected const string m_triggerDestroy = "Destroy";

		public virtual void Appear()
		{
			m_animator.SetTrigger(k_triggerAppear);
		}

		public virtual void AnimEventAppearCompleted() { }

		#endregion

		public virtual void StopBehaviour() { }

	}
}