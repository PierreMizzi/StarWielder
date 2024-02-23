using System;
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

		private void CallbackComboBreak()
		{
			m_comboLabel.text = "";
		}

		private void CallbackComboIncrement()
		{
			m_comboLabel.text = m_playerChannel.currentCombo.ToString();
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

		private float m_normalizedCombo => m_playerChannel.currentCombo / (float)m_maxCombo;
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