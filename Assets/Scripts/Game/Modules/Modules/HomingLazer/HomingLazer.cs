using System.Collections;
using System.Runtime.Serialization.Formatters;
using DG.Tweening;
using UnityEngine;

namespace StarWielder.Gameplay.Modules
{

	public class HomingLazer : MonoBehaviour
	{

		#region Behaviour

		[Header("Settings")]
		[SerializeField] private float m_speed;
		
		private void Fire(Vector3 startPosition, Vector3 endPosition)
		{
			StartCoroutine(FireBehaviour(startPosition, endPosition));
		}

		private float time;
		private float progress;

		private IEnumerator FireBehaviour(Vector3 startPosition, Vector3 endPosition)
		{
			time = 0;
			progress = 0;

			float translateDuration = (startPosition - endPosition).magnitude / m_speed;
			Vector3 tmpPosition = new Vector3();

			// Traveling
			while (time < translateDuration)
			{
				time += Time.deltaTime;
				progress = time / translateDuration;
				tmpPosition = Vector3.Lerp(startPosition, endPosition, progress);
				m_trailTraveling.transform.position = tmpPosition;
				m_trailRemaining.transform.position = tmpPosition;
				yield return null;
			}

			// Delay
			yield return new WaitForSeconds(m_trailDelay);

			// Remaining
			time = 0;
			progress = 0;
			while (time < m_trailLifetime)
			{
				time += Time.deltaTime;
				progress = time / translateDuration;
				m_trailRemaining.startColor = Color.Lerp(m_trailStartFrom, m_trailStartTo, progress);
				m_trailRemaining.endColor = Color.Lerp(m_trailEndFrom, m_trailEndTo, progress);
			}

			yield return null;
		}

		#endregion

		#region Energy

		#endregion

		#region Trail Lifetime

		[Header("Trail")]

		[SerializeField] private float m_trailDelay = 0.1f;
		[SerializeField] private float m_trailLifetime = 1f;

		[SerializeField] private TrailRenderer m_trailTraveling;
		[SerializeField] private TrailRenderer m_trailRemaining;

		private Color m_trailStartFrom;
		private Color m_trailStartTo;
		private Color m_trailEndFrom;
		private Color m_trailEndTo;

		#endregion

		#region Debug

		[SerializeField] private Transform m_start;
		[SerializeField] private Transform m_end;


        protected void Start()
        {
			m_trailStartFrom = m_trailRemaining.startColor;
			m_trailStartTo = m_trailStartFrom;
			m_trailStartTo.a = 0f;

			m_trailEndFrom = m_trailRemaining.startColor;
			m_trailEndTo = m_trailEndFrom;
			m_trailEndTo.a = 0f;
		}

		protected void Update()
        {
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Test();
			}
        }

        [ContextMenu("Call Test")]
		public void Test()
		{
			Fire(m_start.position, m_end.position);
		}
		#endregion

	}

}