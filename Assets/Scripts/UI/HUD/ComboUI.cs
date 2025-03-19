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

		[Header("Combo Break Settings")]
		[SerializeField] private ShakeTweenSettings m_shakeSettingsComboBreak;

		[SerializeField] private float m_baseFontSize = 90f;
		[SerializeField] private Color m_baseFontColor = Color.white;
		[SerializeField] private float m_comboBreakFontSize = 250f;
		[SerializeField] private Color m_comboBreakFontColor = Color.red;

		[Header("Combo Increment Settings")]
		[SerializeField] private ShakeTweenSettings m_shakeSettingsComboIncrement;
		[SerializeField] private float m_comboIncrementDuration = 0.33f;
		[SerializeField] private float m_comboIncrementFontSize = 500f;

		private const string k_multiplyText = "<size=50%>x</size>";

		[ContextMenu("Call CallbackComboBreak")]
		private void CallbackComboBreak()
		{
			m_comboLabel.text = k_multiplyText + m_playerChannel.currentCombo.ToString();
			m_comboLabel.fontSize = m_comboBreakFontSize;
			m_comboLabel.color = m_comboBreakFontColor;

			m_shakeSettingsComboBreak
				.PlayPositionShake(m_comboLabel.transform)
				.OnComplete(()=>{
					m_comboLabel.fontSize = m_baseFontSize;
					m_comboLabel.color = m_baseFontColor;
				});
		}

		[ContextMenu("Call CallbackComboIncrement")]
		public void CallbackComboIncrement()
		{
			m_comboLabel.text = k_multiplyText + m_playerChannel.currentCombo.ToString();

			Sequence sequence = DOTween.Sequence();

			sequence
			.Append // Scale down on font
			(
				DOVirtual
				.Float(
					0f,
					Mathf.PI,
					m_comboIncrementDuration,
					(float value) =>
					{
						value = Mathf.Sin(value);
						m_comboLabel.fontSize = Mathf.Lerp(m_baseFontSize, m_comboIncrementFontSize, value);
					}
				)
				.SetEase(Ease.Linear)
			)
			.Append // Impact
			(
				m_shakeSettingsComboIncrement.PlayPositionShake(m_comboLabel.transform)
			);
		}

		#endregion

		#region MonoBehaviour

		private void Start()
		{
			if (m_playerChannel != null)
			{
				m_playerChannel.onIncrementCombo += CallbackComboIncrement;
				m_playerChannel.onComboBreak += CallbackComboBreak;
			}
		}

		private void OnDestroy()
		{
			if (m_playerChannel != null)
			{
				m_playerChannel.onIncrementCombo -= CallbackComboIncrement;
				m_playerChannel.onComboBreak -= CallbackComboBreak;
			}
		}

		private void Update()
		{
			UpdateComboBar();

			if (Input.GetKeyDown(KeyCode.Keypad9))
			{
				CallbackComboIncrement();
			}

		}

		#endregion

		#region Combo Bar

		[Header("Combo Bar")]
		[SerializeField] private Image m_fillImage;
		[SerializeField] private Gradient m_gradient;

		[Header("Combo Bar Settings")]
		[SerializeField] private float m_minFillValue = 0.1f;
		[SerializeField] private float m_maxFillValue = 0.9f;
		[SerializeField] private float m_noiseAmplitude = 0.05f;
		[SerializeField] private float m_noiseFrequency = 15f;

		private float m_fillAmount = 0;
		private float m_noiseSeed;
		private float m_noiseValue;

        private void UpdateComboBar()
		{
			m_noiseSeed += Time.deltaTime * m_noiseFrequency;
			m_noiseValue = Mathf.PerlinNoise(m_noiseSeed, 0);
			m_noiseValue = UtilsClass.ZeroPlusToMinusPlus(m_noiseValue);

			m_fillAmount = NormalizedToFill(m_playerChannel.currentNormalizedCombo) + m_noiseValue * m_noiseAmplitude;
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