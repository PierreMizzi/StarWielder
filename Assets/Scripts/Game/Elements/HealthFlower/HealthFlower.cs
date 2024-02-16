using System.Collections.Generic;
using DG.Tweening;
using PierreMizzi.Useful;
using PierreMizzi.Useful.PoolingObjects;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;
using StarWielder.Gameplay.Player;

namespace StarWielder.Gameplay.Elements
{
	[RequireComponent(typeof(Animator))]
	public class HealthFlower : MonoBehaviour, IStateMachine
	{

		private Star m_star;
		private Animator m_animator;

		[SerializeField] private PoolingChannel m_poolingChannel;

		#region MonoBehaviour

		private void Awake()
		{
			m_bloomingSpeed = 1f / m_bloomingTime;

			m_animator = GetComponent<Animator>();
			m_animator.SetFloat(k_floatMinShineStrength, m_minShineStrength);
			m_animator.SetFloat(k_floatMaxShineStrength, m_maxShineStrength);
		}

		private void Start()
		{
			m_star = GameObject.FindGameObjectWithTag("Star").GetComponent<Star>();

			m_pollen = m_poolingChannel.onGetFromPool.Invoke(m_healthPollenPrefab.gameObject).GetComponent<HealthPollen>();
			m_pollen.transform.parent = m_stem.pollenContainer;
			m_pollen.transform.localPosition = Vector3.zero;
			m_pollen.transform.localRotation = Quaternion.identity;
		}

		private void Update()
		{
			if (m_star != null && !m_hasBloomed)
			{
				//ManageBloomingProgress();
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
		[SerializeField] private float m_currentShineStrength;

		private void ManageShineStrength()
		{
			// Shine Strength
			m_currentShineStrength = m_star.GetShineStrength(transform);
			m_animator.SetFloat(k_floatShineStrength, m_currentShineStrength);

			m_stem.SetTemperature(GetTemperature());
		}

		private float GetTemperature()
		{
			// Mais unclamped !
			// return UtilsClass.Remap(m_currentShineStrength, m_maxShineStrength, m_minShineStrength, 1, 0);

			if (m_currentShineStrength > m_maxShineStrength)
				return 1;
			else if (m_currentShineStrength < m_minShineStrength)
				return 0;
			else
				return (m_currentShineStrength - m_minShineStrength) / (m_maxShineStrength - m_minShineStrength);
		}


		#endregion

		#region Blooming

		[Header("Blooming")]
		[SerializeField] private float m_bloomingTime = 0.5f;
		private float m_bloomingSpeed = 0.0f;
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

		#region Stem
		[Header("Stem")]

		[SerializeField] private Stem m_stem;

		#endregion

		#region Health Pollen

		[Header("Health Pollen")]
		[SerializeField] private HealthPollen m_healthPollenPrefab;
		private HealthPollen m_pollen;

		private bool m_hasBloomed = false;

		private void Bloom()
		{
			m_hasBloomed = true;

			m_currentShineStrength = (m_minShineStrength + m_maxShineStrength) / 2f;
			m_animator.SetFloat(k_floatShineStrength, m_currentShineStrength);

			// HealthPollen pollen = Instantiate(m_healthPollenPrefab, transform.position, UtilsClass.RandomRotation2D());
			// pollen.transform.DOMove(transform.position + transform.up * 3f, 1.5f);
		}

		#endregion

		#region StateMachine

		public List<AState> states { get; set; }
		public AState currentState { get; set; }
		public void InitializeStates()
		{
			states = new List<AState>()
			{
				// MyState
			};
		}

		public void UpdateState()
		{
			currentState?.Update();
		}

		public void ChangeState(int previousState, int nextState)
		{
			currentState?.Exit();

			currentState = states.Find((AState newState) => newState.type == nextState);
			if (currentState != null)
				currentState.Enter(previousState);
			else
			{
				Debug.LogError($"Couldn't find a new state of type : {nextState}. Going Inactive");
			}
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