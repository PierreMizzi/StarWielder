using System;
using PierreMizzi.Gameplay.Players;
using TMPro;
using UnityEngine;

public class EnergyUI : MonoBehaviour
{
	[SerializeField] private PlayerChannel m_playerChannel = null;

	#region Energy

	[Header("Energy")]
	[SerializeField] private TextMeshProUGUI m_starEnergyLabel;


	private void CallbackRefreshStarEnergy(float value)
	{
		m_starEnergyLabel.text = String.Format("{0:0.0}", value) + "<size=40%>K</size>";
	}

	#endregion


	#region MonoBehaviour

	private void Start()
	{
		if (m_playerChannel != null)
		{
			m_playerChannel.onRefreshStarEnergy += CallbackRefreshStarEnergy;
		}
	}

	private void OnDestroy()
	{
		if (m_playerChannel != null)
		{
			m_playerChannel.onRefreshStarEnergy += CallbackRefreshStarEnergy;
		}
	}


	#endregion

}