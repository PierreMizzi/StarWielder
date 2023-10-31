using System;
using PierreMizzi.Useful;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace PierreMizzi.Gameplay.Players
{

	public class ShipHUD : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI m_energyLabel;

		[SerializeField] private PlayerChannel m_playerChannel = null;

		#region MonoBehaviour

		private void Start()
		{
			if (m_playerChannel != null)
				m_playerChannel.onRefreshShipEnergy += CallbackRefreshShipEnergy;

		}

		private void OnDestroy()
		{
			if (m_playerChannel != null)
				m_playerChannel.onRefreshShipEnergy -= CallbackRefreshShipEnergy;
		}

		private void CallbackRefreshShipEnergy(float value)
		{
			m_energyLabel.text = String.Format("{0:0.0}", value);
		}

		#endregion

	}

}