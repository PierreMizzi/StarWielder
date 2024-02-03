using System;
using System.Collections;
using UnityEngine;

namespace PierreMizzi.Useful
{
	public class CyclicTimer : MonoBehaviour
	{
		[Header("Main")]
		[SerializeField] private float m_cycleDuration = 1f;

		private IEnumerator m_coroutine;
		public Action onCycleCompleted;
		private float m_cycleProgress;

		private void Awake()
		{
			onCycleCompleted = () => { };
		}

		public void StartBehaviour()
		{
			if (m_coroutine == null)
			{
				if (m_useRandomCycleDuration)
					SetRandomCycleDuration();

				m_coroutine = BehaviourCoroutine();
				StartCoroutine(m_coroutine);
			}
		}

		public void StopBehaviour()
		{
			if (m_coroutine != null)
			{
				StopCoroutine(m_coroutine);
				m_coroutine = null;
			}
		}

		private IEnumerator BehaviourCoroutine()
		{
			while (true)
			{
				m_cycleProgress += Time.deltaTime;

				if (m_cycleProgress > m_cycleDuration)
				{
					m_cycleProgress = 0;
					onCycleCompleted.Invoke();

					if (m_useRandomCycleDuration)
						SetRandomCycleDuration();
				}
				yield return null;
			}
		}

		#region Random


		[Header("Random")]
		[SerializeField] private bool m_useRandomCycleDuration = false;
		[SerializeField] private float m_minCycleDuration = 1f;
		[SerializeField] private float m_maxCycleDuration = 2f;

		private void SetRandomCycleDuration()
		{
			m_cycleDuration = UnityEngine.Random.Range(m_minCycleDuration, m_maxCycleDuration);
		}

		#endregion

	}
}