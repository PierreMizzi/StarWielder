using PierreMizzi.Useful;
using QGamesTest.Gameplay.Player;
using UnityEngine;
using UnityEngine.UI;

namespace QGamesTest.UI
{

	public class ComboUI : MonoBehaviour
	{

		#region Main

		[Header("Main")]
		[SerializeField] private PlayerChannel m_playerChannel;
		[SerializeField] private Image m_fillImage;
		[SerializeField] private int m_maxCombo = 10;

		private float m_normalizedCombo = 0;
		private float m_fillAmount = 0;
		private float m_noiseSeed;
		private float m_noiseValue;

		private float NormalizedToFill(float value)
		{
			return Mathf.Lerp(m_minFillValue, m_maxFillValue, value);
		}

		private void CallbackRefreshStarCombo(int combo)
		{
			m_normalizedCombo = combo / (float)m_maxCombo;
		}

		#endregion

		#region Settings

		[Header("Settings")]
		[SerializeField] private Gradient m_gradient;

		[SerializeField] private float m_minFillValue = 0.1f;
		[SerializeField] private float m_maxFillValue = 0.9f;
		[SerializeField] private float m_noiseAmplitude = 0.05f;
		[SerializeField] private float m_noiseFrequency = 15f;

		#endregion

		#region MonoBehaviour

		private void Start()
		{
			if (m_playerChannel != null)
				m_playerChannel.onRefreshStarCombo += CallbackRefreshStarCombo;
		}

		private void OnDestroy()
		{
			if (m_playerChannel != null)
				m_playerChannel.onRefreshStarCombo -= CallbackRefreshStarCombo;
		}

		private void Update()
		{
			m_noiseSeed += Time.deltaTime * m_noiseFrequency;
			m_noiseValue = Mathf.PerlinNoise(m_noiseSeed, 0);
			m_noiseValue = UtilsClass.ZeroPlusToMinusPlus(m_noiseValue);

			m_fillAmount = NormalizedToFill(m_normalizedCombo) + m_noiseValue * m_noiseAmplitude;
			m_fillImage.fillAmount = m_fillAmount;

			m_fillImage.color = m_gradient.Evaluate(m_fillAmount);
		}

		#endregion

	}
}