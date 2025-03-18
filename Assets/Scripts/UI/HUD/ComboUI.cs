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

		[SerializeField] private ShakeTweenSettings m_shakeSettingsComboBreak;
		[SerializeField] private ShakeTweenSettings m_shakeSettingsComboIncrement;

		private const string k_multiplyText = "<size=50%>x</size>";

		[ContextMenu("Call CallbackComboBreak")]
		private void CallbackComboBreak()
		{
			m_comboLabel.text = k_multiplyText + m_playerChannel.currentCombo.ToString();
			m_shakeSettingsComboBreak.PlayPositionShake(m_comboLabel.transform);
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
					1f,
					0.25f,
					(float value) =>
					{
						m_comboLabel.fontSize = Mathf.Lerp(300, 90, value);
					}
				)
				.SetEase(Ease.OutCubic)
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
		}

		#endregion

		#region Combo Bar

		[Header("Combo Bar")]
		[SerializeField] private Image m_fillImage;
		[SerializeField] private Gradient m_gradient;

		[Header("Settings")]
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