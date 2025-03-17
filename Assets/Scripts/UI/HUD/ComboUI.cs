using System;
using DG.Tweening;
using PierreMizzi.Useful;
using StarWielder.Gameplay.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StarWielder.UI
{

	public class ComboUI : MonoBehaviour
	{

		#region Main

		[Header("Main")]
		[SerializeField] private PlayerChannel m_playerChannel;

		[SerializeField] private TextMeshProUGUI m_comboLabel;

		[SerializeField] private ShakeTweenSettings m_shakeSettings;

		private const string k_multiplyText = "<size=50%>x</size>";

		private void CallbackComboBreak()
		{
			m_comboLabel.text = "";
		}

		[ContextMenu("Call CallbackComboIncrement")]
		public void CallbackComboIncrement()
		{
			m_comboLabel.text = k_multiplyText + m_playerChannel.currentCombo.ToString();

			m_comboLabel.transform.DOShakePosition(
				m_shakeSettings.duration,
			 	m_shakeSettings.strength,
				m_shakeSettings.vibrato,
				m_shakeSettings.randomness,
				m_shakeSettings.snapping,
				m_shakeSettings.fadeOut,
				m_shakeSettings.randomnessMode
			);
		}

		#endregion

		#region MonoBehaviour

		private void Start()
		{
			if (m_playerChannel != null)
			{
				m_playerChannel.onComboIncrement += CallbackComboIncrement;
				m_playerChannel.onComboBreak += CallbackComboBreak;
			}
		}

		private void OnDestroy()
		{
			if (m_playerChannel != null)
			{
				m_playerChannel.onComboIncrement -= CallbackComboIncrement;
				m_playerChannel.onComboBreak -= CallbackComboBreak;
			}
		}

		private void Update()
		{
			UpdateComboBar();
		}

		#endregion

		#region Combo Bar

		[Header("Combo Bar")]
		[SerializeField] private Image m_fillImage;
		[SerializeField] private Gradient m_gradient;

		[Header("Settings")]
		[SerializeField] private int m_maxCombo = 10;
		[SerializeField] private float m_minFillValue = 0.1f;
		[SerializeField] private float m_maxFillValue = 0.9f;
		[SerializeField] private float m_noiseAmplitude = 0.05f;
		[SerializeField] private float m_noiseFrequency = 15f;

		private float m_normalizedCombo => (float)(m_playerChannel.currentCombo - 1) / (float)m_maxCombo;
		private float m_fillAmount = 0;
		private float m_noiseSeed;
		private float m_noiseValue;

		private void UpdateComboBar()
		{
			m_noiseSeed += Time.deltaTime * m_noiseFrequency;
			m_noiseValue = Mathf.PerlinNoise(m_noiseSeed, 0);
			m_noiseValue = UtilsClass.ZeroPlusToMinusPlus(m_noiseValue);

			m_fillAmount = NormalizedToFill(m_normalizedCombo) + m_noiseValue * m_noiseAmplitude;
			m_fillImage.fillAmount = m_fillAmount;

			m_fillImage.color = m_gradient.Evaluate(m_fillAmount);
		}

		private float NormalizedToFill(float value)
		{
			return Mathf.Lerp(m_minFillValue, m_maxFillValue, value);
		}

		#endregion



	}
}