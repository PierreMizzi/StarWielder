using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PierreMizzi.Gameplay.Players
{

	public class ShipHUD : MonoBehaviour
	{
		[SerializeField] private PlayerChannel m_playerChannel = null;

		#region Energy

		[Header("Energy")]
		[SerializeField] private TextMeshProUGUI m_energyLabel;

		private void CallbackRefreshShipEnergy(float value)
		{
			m_energyLabel.text = String.Format("{0:0.0}", value);
		}

		#endregion

		#region Dash

		[Header("Dash")]
		[SerializeField] private Image m_dashImage;
		[SerializeField] private Color m_dashAvailableColor = Color.white;
		[SerializeField] private Color m_dashUnavailableColor = Color.white;

		private void CallbackUseDash()
		{
			m_dashImage.color = m_dashUnavailableColor;
		}

		private void CallbackRefreshCooldownDash(float value)
		{
			m_dashImage.fillAmount = value;
		}

		private void CallbackRechargeDash()
		{
			m_dashImage.color = m_dashAvailableColor;
		}

		#endregion

		#region MonoBehaviour

		private void Start()
		{
			if (m_playerChannel != null)
			{
				m_playerChannel.onRefreshShipEnergy += CallbackRefreshShipEnergy;

				// Dash
				m_playerChannel.onUseDash += CallbackUseDash;
				m_playerChannel.onRefreshCooldownDash += CallbackRefreshCooldownDash;
				m_playerChannel.onRechargeDash += CallbackRechargeDash;
			}
		}

		private void OnDestroy()
		{
			if (m_playerChannel != null)
			{
				m_playerChannel.onRefreshShipEnergy -= CallbackRefreshShipEnergy;

				// Dash
				m_playerChannel.onUseDash -= CallbackUseDash;
				m_playerChannel.onRefreshCooldownDash -= CallbackRefreshCooldownDash;
				m_playerChannel.onRechargeDash -= CallbackRechargeDash;
			}
		}


		#endregion

	}

}