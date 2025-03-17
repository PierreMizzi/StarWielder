using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PierreMizzi.Useful.PoolingObjects;
using StarWielder.Gameplay.Player;
using UnityEngine;

namespace StarWielder.Gameplay.Modules
{

	public class HomingLazer : MonoBehaviour
	{

		#region Behaviour

		[Header("Behaviour")]

		[SerializeField] private PoolingChannel m_poolingChannel;
		private HomingLazerModule m_module;
		[SerializeField] private HomingLazerModuleSettings m_settings;
		[SerializeField] private TrailRenderer m_trailTraveling;
		[SerializeField] private TrailRenderer m_trailRemaining;

		private Color m_trailStartFrom;
		private Color m_trailStartTo;
		private Color m_trailEndFrom;
		private Color m_trailEndTo;

		private float time;
		private float progress;

		public void Fire(HomingLazerModule module, Vector3 startPosition, Vector3 endPosition)
		{
			m_module = module;
			InitializeColor();

			FireRaycast(startPosition, endPosition);
			StartCoroutine(FireBehaviour(startPosition, endPosition));
		}

		private IEnumerator FireBehaviour(Vector3 startPosition, Vector3 endPosition)
		{
			time = 0;
			progress = 0;

			float translateDuration = (startPosition - endPosition).magnitude / m_settings.LazerSpeed;


			// Initialize Trail position
			Vector3 tmpPosition = startPosition;
			m_trailTraveling.transform.position = tmpPosition;
			m_trailRemaining.transform.position = tmpPosition;

			// Reset Trails color
			m_trailRemaining.startColor = m_trailStartFrom;
			m_trailRemaining.endColor = m_trailEndFrom;
			yield return null;

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

			if (m_module != null)
			{
				m_module.ConvertAbsorbedEnergyIntoHealth(m_absorbedEnergy);
			}

			// Delay
			yield return new WaitForSeconds(m_settings.LazerTrailDelay);

			// Remaining
			time = 0;
			progress = 0;
			while (time < m_settings.LazerTrailLifetime)
			{
				time += Time.deltaTime;
				progress = time / m_settings.LazerTrailLifetime;
				m_trailRemaining.startColor = Color.Lerp(m_trailStartFrom, m_trailStartTo, progress);
				m_trailRemaining.endColor = Color.Lerp(m_trailEndFrom, m_trailEndTo, progress);
				yield return null;
			}

			// Reset Trails
			m_trailRemaining.Clear();
			m_trailTraveling.Clear();

			yield return null;

			if (m_poolingChannel != null)
			{
				m_poolingChannel.onReleaseToPool(gameObject);
			}
			yield return null;
		}

		protected void InitializeColor()
		{
			m_trailStartFrom = m_settings.TrailBaseColor;
			m_trailStartTo = m_trailStartFrom;
			m_trailStartTo.a = 0f;

			m_trailEndFrom = m_settings.TrailBaseColor;
			m_trailEndTo = m_trailEndFrom;
			m_trailEndTo.a = 0f;
		}

		#endregion

		#region Energy

		[SerializeField] private LayerMask m_raycastLayerMask;

		private float m_absorbedEnergy = 0;

		private void FireRaycast(Vector3 startPosition, Vector3 endPosition)
		{
			List<StarAbsorbable> rightStars = RaycastStarAbsorbables(startPosition, endPosition, true);
			List<StarAbsorbable> leftStars = RaycastStarAbsorbables(startPosition, endPosition, false);

			rightStars = rightStars.Union(leftStars).ToList();

			foreach (StarAbsorbable star in rightStars)
			{
				m_absorbedEnergy += star.energy;
				star.onAbsorb.Invoke();
			}
		}

		private List<StarAbsorbable> RaycastStarAbsorbables(Vector3 startPosition, Vector3 endPosition, bool rightOrLeft)
		{
			List<StarAbsorbable> stars = new List<StarAbsorbable>();

			Vector3 startToEnd = endPosition - startPosition;
			Vector3 right = Vector3.Cross(startToEnd.normalized, Vector3.forward).normalized;

			startPosition += right * (rightOrLeft ? 1 : -1) * m_settings.LazerWidth / 2f;

			// Debug.DrawLine(startPosition, startPosition + startToEnd, Color.red, 2f);

			RaycastHit2D[] hits =  Physics2D.RaycastAll(startPosition, startToEnd.normalized, startToEnd.magnitude, m_raycastLayerMask);

			foreach (RaycastHit2D hit in hits)
			{
				if (hit.collider.gameObject.TryGetComponent(out StarAbsorbable absorbable))
				{
					stars.Add(absorbable);
				}
			}

			return stars;
		}

		#endregion

	}

}