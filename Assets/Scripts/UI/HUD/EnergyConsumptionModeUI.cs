using System;
using System.Collections;
using StarWielder.Gameplay.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StarWielder.Gameplay
{
	public class EnergyConsumptionModeUI : MonoBehaviour
	{
		[SerializeField] private PlayerChannel m_playerChannel;
		[SerializeField] private ShipSettings m_shipSettings;

		[SerializeField] private TextMeshProUGUI m_label;
		[SerializeField] private Image m_frame;
        private RectTransform m_rectTransform;

        private void CallbackSetEnergyConsumptionMode(Ship.EnergyConsumptionMode mode)
		{
			if (m_shipSettings == null)
				return;

			StartCoroutine(SetEnergyConsumptionLabel(mode));
		}

        private IEnumerator SetEnergyConsumptionLabel(Ship.EnergyConsumptionMode mode)
        {
			ShipEnergyConsumptionSettings settings = m_shipSettings.GetEnergyConsumptionSettingsFromMode(mode);

			m_label.text = settings.labelName;
			m_label.color = settings.labelColor;
			m_frame.color = settings.labelColor;

			LayoutRebuilder.ForceRebuildLayoutImmediate(m_rectTransform);

			m_frame.rectTransform.anchoredPosition = Vector2.zero;

			yield return null;
        }

        #region MonoBehaviour

		private void Awake()
		{
			m_rectTransform = GetComponent<RectTransform>();
		}

        private void Start()
		{
			if (m_playerChannel != null)
			{
				m_playerChannel.onSetEnergyConsumptionMode += CallbackSetEnergyConsumptionMode;
			}
		}



        private void OnDestroy()
		{
			if (m_playerChannel != null)
			{
				m_playerChannel.onSetEnergyConsumptionMode -= CallbackSetEnergyConsumptionMode;
			}
		}

		#endregion
		
	}
}