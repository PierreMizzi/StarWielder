using DG.Tweening;
using PierreMizzi.Useful;
using UnityEngine;

namespace StarWielder.Gameplay.Player
{
	[RequireComponent(typeof(Animator))]
	public class HealthFlower : MonoBehaviour
	{

		private Star m_star;
		private Animator m_animator;

		#region MonoBehaviour

		private void Awake()
		{
			m_animator = GetComponent<Animator>();
			m_animator.SetFloat(k_floatMinShineStrength, m_minShineStrength);
			m_animator.SetFloat(k_floatMaxShineStrength, m_maxShineStrength);
		}

		private void Start()
		{
			m_star = GameObject.FindGameObjectWithTag("Star").GetComponent<Star>();
		}

		private void Update()
		{
			if (m_star != null && !m_hasBloomed)
			{
				ManageBloomingProgress();
				ManageShineStrength();
			}
		}

		#endregion

		#region Shine Strength

		[Header("Shine Strength")]
		[SerializeField] private float m_minShineStrength = 0.75f;
		[SerializeField] private float m_maxShineStrength = 1.5f;
		private const string k_floatShineStrength = "ShineStrength";
		private const string k_floatMinShineStrength = "MinShineStrength";
		private const string k_floatMaxShineStrength = "MaxShineStrength";
		private float m_currentShineStrength;

		private void ManageShineStrength()
		{
			// Shine Strength
			m_currentShineStrength = m_star.GetShineStrength(transform);
			m_animator.SetFloat(k_floatShineStrength, m_currentShineStrength);
		}

		private void SetBloomState()
		{
			m_currentShineStrength = (m_minShineStrength + m_maxShineStrength) / 2f;
			m_animator.SetFloat(k_floatShineStrength, m_currentShineStrength);
		}

		#endregion

		#region Blooming

		[Header("Blooming")]
		[SerializeField] private float m_bloomingSpeed = 0.1f;
		private const string k_floatBloomingProgress = "BloomingProgress";

		private float m_currentBloomingProgress;

		private Vector3 m_flowerToStarDirection;

		private void ManageBloomingProgress()
		{
			if (CheckIsFacingFlower() && CheckHasRightShineStrength())
			{
				m_currentBloomingProgress += m_bloomingSpeed * Time.deltaTime;
				m_currentBloomingProgress = Mathf.Clamp(m_currentBloomingProgress, 0f, 1f);
				m_animator.SetFloat(k_floatBloomingProgress, m_currentBloomingProgress);

				if (m_currentBloomingProgress == 1)
					Bloom();
			}
		}

		private bool CheckIsFacingFlower()
		{
			m_flowerToStarDirection = (m_star.transform.position - transform.position).normalized;
			return Vector3.Dot(m_flowerToStarDirection, transform.up) > 0;
		}

		private bool CheckHasRightShineStrength()
		{
			return m_minShineStrength < m_currentShineStrength && m_currentShineStrength < m_maxShineStrength;
		}

		#endregion

		#region Health Pollen

		[Header("Health Pollen")]
		[SerializeField] private HealthPollen m_healthPollenPrefab;

		private bool m_hasBloomed = false;

		private void Bloom()
		{
			m_hasBloomed = true;

			SetBloomState();

			HealthPollen pollen = Instantiate(m_healthPollenPrefab, transform.position, UtilsClass.RandomRotation2D());
			pollen.transform.DOMove(transform.position + transform.up * 3f, 1.5f);
		}

		#endregion

		#region Debug

		[Header("Debug")]
		Color defaultGizmosColor;

		protected void OnDrawGizmos()
		{
			defaultGizmosColor = Gizmos.color;

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, m_minShineStrength);

			Gizmos.color = Color.blue;
			Gizmos.DrawWireSphere(transform.position, m_maxShineStrength);

			// My Gizmos ...
			Gizmos.color = defaultGizmosColor;
		}
		#endregion

	}
}