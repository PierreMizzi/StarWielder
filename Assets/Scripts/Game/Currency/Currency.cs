using DG.Tweening;
using UnityEngine;

namespace StarWielder.Gameplay
{
	public class Currency : MonoBehaviour
	{
		#region Spawning

		[ContextMenu("Test")]
		public void Test()
		{
			Collect();
		}

		#endregion

		#region Collecting

		[SerializeField] private GameChannel m_gameChannel;

		[Header("Collecting")]
		[SerializeField] private float m_collectingDelay = 1f;
		[SerializeField] private float m_collectingSpeed = 1f;
		[SerializeField] private Ease m_collectingEase = Ease.OutCubic;

		[Header("Curve")]

		[SerializeField] private float m_curveAmplitude = 2f;

		private Tween m_collectingTween;

		public void Collect()
		{
			Vector3 from = transform.position;
			Vector3 to = CurrencyUI.worldPosition;
			float duration = Vector3.Distance(from, to) / m_collectingSpeed;

			Vector3 straightPosition = Vector3.zero;

			// Curve
			Vector3 curvedPosition = Vector3.zero;
			Vector3 fromToCrossDirection = Vector3.Cross((to - from).normalized, Vector3.forward);
			int curveSide = Random.Range(0, 2) == 0 ? 1 : -1;

			m_collectingTween = DOVirtual.Float(
				0,
				1,
				duration,
				(float value) =>
				{
					straightPosition = Vector3.Lerp(from, to, value);
					curvedPosition = Mathf.Sin(value * Mathf.PI) * m_curveAmplitude * curveSide * fromToCrossDirection;
					transform.position = straightPosition + curvedPosition;
				})
				.SetDelay(m_collectingDelay)
				.SetEase(m_collectingEase)
				.OnComplete(CallbackOnComplete);
		}

		private void CallbackOnComplete()
		{
			m_gameChannel.onCollectCurrency.Invoke(1);
			Destroy(gameObject);
		}

		#endregion
	}
}