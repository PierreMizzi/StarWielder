using UnityEngine;

namespace QGamesTest.Gameplay.Player
{

	/// <summary>
	/// Little blue star following the player whenever he can dash
	/// </summary>
	public class DashStar : MonoBehaviour
	{

		#region MonoBehaviour

		private void Awake()
		{
			_animator = GetComponent<Animator>();
		}

		private void Update()
		{
			Follow();
		}

		#endregion

		#region Follow

		[Header("Follow")]
		[SerializeField] private Transform m_followTransform = null;
		[SerializeField] private float m_followSpeed = 0.2f;
		private Vector3 m_velocity;

		private bool m_isFollowing = true;

		private void Follow()
		{
			if (m_followTransform != null && m_isFollowing)
				transform.position = Vector3.SmoothDamp(transform.position, m_followTransform.position, ref m_velocity, m_followSpeed);
		}

		public void AnimEventFollow()
		{
			m_isFollowing = true;
		}

		#endregion

		#region Animations

		private Animator _animator = null;

		private const string k_triggerUse = "Use";
		private const string k_triggerRecharge = "Recharge";

		public void Use()
		{
			_animator.SetTrigger(k_triggerUse);
			m_isFollowing = false;
		}

		public void Recharge()
		{
			_animator.SetTrigger(k_triggerRecharge);
		}


		#endregion


	}
}