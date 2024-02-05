using PierreMizzi.Rendering;
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
			if (m_star != null)
			{
				ManageBloomingProgress();
			}
		}

		#endregion

		#region Blooming

		[Header("Blooming")]
		[SerializeField] private float m_bloomingSpeed = 0.1f;
		[SerializeField] private float m_minShineStrength = 0.75f;
		[SerializeField] private float m_maxShineStrength = 1.5f;

		private const string k_floatBloomingProgress = "BloomingProgress";
		private const string k_floatShineStrength = "ShineStrength";
		private const string k_floatMinShineStrength = "MinStarShine";
		private const string k_floatMaxShineStrength = "MaxShineStrength";

		private float m_currentShineStrength;
		private float m_currentBloomingProgress;

		private void ManageBloomingProgress()
		{
			m_currentShineStrength = m_star.GetShineStrength(transform);

			if (m_minShineStrength < m_currentShineStrength && m_currentShineStrength < m_maxShineStrength)
			{
				m_currentBloomingProgress += m_bloomingSpeed * Time.deltaTime;
				m_currentBloomingProgress = Mathf.Clamp(m_currentBloomingProgress, 0f, 1f);
				m_animator.SetFloat(k_floatBloomingProgress, m_currentBloomingProgress);
			}
		}



		#endregion


	}
}