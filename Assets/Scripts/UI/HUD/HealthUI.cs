using DG.Tweening;
using StarWielder.Gameplay.Player;
using UnityEngine;
using UnityEngine.UI;


namespace StarWielder.UI
{
	public class HealthUI : MonoBehaviour
	{

		[Header("Main")]
		[SerializeField] private PlayerChannel m_playerChannel = null;

		[Header("Sliders")]
		[SerializeField] private Slider m_leftForegroundSlider;
		[SerializeField] private Slider m_rightForegroundSlider;
		[SerializeField] private Slider m_leftBackgroundSlider;
		[SerializeField] private Slider m_rightBackgroundSlider;

		private float m_foregroundProgress = 1f;

		private void CallbackRefreshHealth(float normalizedHealth)
		{
			m_foregroundProgress = normalizedHealth;

			m_leftForegroundSlider.value = m_foregroundProgress;
			m_rightForegroundSlider.value = m_foregroundProgress;

			ManageBalance();
		}

		#region Balance

		[Header("Balance")]
		[SerializeField] private float m_delayBeforeBalance = 2f;
		[SerializeField] private float m_balanceRate = 0.2f;

		private float m_backgroundProgress = 1f;

		private Sequence m_balanceSequence = null;

		private void ManageBalance()
		{

			float duration = (m_backgroundProgress - m_foregroundProgress) / m_balanceRate;

			if (m_balanceSequence != null)
				m_balanceSequence.Kill();

			Tween balanceTween = DOVirtual.Float(
				m_backgroundProgress,
				m_foregroundProgress,
				duration,
				BalanceBackground
			).SetEase(Ease.Linear);

			m_balanceSequence = DOTween.Sequence();
			m_balanceSequence.AppendInterval(m_delayBeforeBalance)
							 .Append(balanceTween);
		}

		private void BalanceBackground(float value)
		{
			m_backgroundProgress = value;

			m_leftBackgroundSlider.value = m_backgroundProgress;
			m_rightBackgroundSlider.value = m_backgroundProgress;
		}



		#endregion

		#region MonoBehaviour

		private void Start()
		{
			if (m_playerChannel != null)
				m_playerChannel.onRefreshShipHealth += CallbackRefreshHealth;
		}

		private void OnDestroy()
		{
			if (m_playerChannel != null)
				m_playerChannel.onRefreshShipHealth -= CallbackRefreshHealth;
		}

		#endregion

	}
}