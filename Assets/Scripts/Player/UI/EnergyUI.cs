using System;
using PierreMizzi.Gameplay.Players;
using TMPro;
using UnityEngine;

public class EnergyUI : MonoBehaviour
{
	[SerializeField] private PlayerChannel m_playerChannel = null;

	#region Energy

	[Header("Energy")]
	[SerializeField] private TextMeshProUGUI m_shipEnergyLabel;
	[SerializeField] private TextMeshProUGUI m_starEnergyLabel;
	[SerializeField] private TextMeshProUGUI m_comboLabel;

	private void CallbackRefreshShipEnergy(float value)
	{
		m_shipEnergyLabel.text = String.Format("{0:0.0}", value);
	}

	private void CallbackRefreshStarEnergy(float value)
	{
		m_starEnergyLabel.text = String.Format("{0:0.0}", value);
	}

	private void CallbackRefreshStarCombo(int value)
	{
		m_comboLabel.text = $"<size=\"33%\">x</size>{value.ToString()}";
	}

	#endregion


	#region MonoBehaviour

	private void Start()
	{
		if (m_playerChannel != null)
		{
			m_playerChannel.onRefreshStarEnergy += CallbackRefreshStarEnergy;
			m_playerChannel.onRefreshShipEnergy += CallbackRefreshShipEnergy;
			m_playerChannel.onRefreshStarCombo += CallbackRefreshStarCombo;
		}
	}

	private void OnDestroy()
	{
		if (m_playerChannel != null)
		{
			m_playerChannel.onRefreshShipEnergy -= CallbackRefreshShipEnergy;
			m_playerChannel.onRefreshStarEnergy -= CallbackRefreshStarEnergy;
			m_playerChannel.onRefreshStarCombo -= CallbackRefreshStarCombo;
		}
	}


	#endregion

}