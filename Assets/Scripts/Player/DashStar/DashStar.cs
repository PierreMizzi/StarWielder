using System;
using System.Collections;
using UnityEngine;

namespace StarWielder.Gameplay.Player
{

	/// <summary>
	/// Little blue star following the player whenever he can dash
	/// </summary>
	public class DashStar : MonoBehaviour
	{

		#region Behaviour

		private Ship m_ship;
		private DashStarManager m_dashStarManager;

		[SerializeField] private bool m_canUse = true;
		public bool canUse => m_canUse ;

		private float m_dashCooldownTime = 0;

		public void Initialize(Ship ship, DashStarManager dashStarManager)
        {
			Awake();
			m_ship = ship;
			m_dashStarManager = dashStarManager;
		}

		[ContextMenu("Use")]
        public void Use()
		{
			_animator.SetTrigger(k_triggerUse);
			m_isFollowing = false;
			m_canUse = false;
			StartCoroutine(DashCooldownIEnumerator());
		}

		private IEnumerator DashCooldownIEnumerator()
		{
			m_dashCooldownTime = 0f;
			while (m_dashCooldownTime < m_ship.stats.dashCooldownDuration)
			{
				m_dashCooldownTime += Time.deltaTime;
				yield return null;
			}
			Recharge();
		}

		public void Recharge()
		{
			_animator.SetTrigger(k_triggerRecharge);
			m_canUse = true;
		}



        #endregion

        #region MonoBehaviour

        private void Awake()
		{
			_animator = GetComponent<Animator>();
		}

		private void LateUpdate()
		{
			Follow();
		}

		#endregion

		#region Anchor

		[Header("Follow")]
		[SerializeField] private float m_followSpeed = 0.2f;
		private Vector3 m_velocity;
		
		[SerializeField] private DashStarAnchor m_ownAnchor;
		[SerializeField] private DashStarAnchor m_nextAnchor;
		public DashStarAnchor nextAnchor => m_nextAnchor;

		private bool m_isFollowing = true;

		private void Follow()
		{
			if (m_ownAnchor != null && m_isFollowing)
			{
				transform.position = Vector3.SmoothDamp(transform.position, m_ownAnchor.transform.position, ref m_velocity, m_followSpeed);
			}
		}

		public void SetAnchor(DashStarAnchor ownAnchor)
		{
			m_ownAnchor = ownAnchor;
			m_nextAnchor.Set(m_ship.transform, transform);
		}

		#endregion

		#region Animations

		private Animator _animator = null;

		private const string k_triggerUse = "Use";
		private const string k_triggerRecharge = "Recharge";

		public void AnimEventFollow()
		{
			m_isFollowing = true;
		}



        #endregion


    }
}