using System;
using TMPro;
using UnityEngine;

namespace PierreMizzi.Gameplay.Players
{

	public class StarHUD : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI m_energyLabel;
		[SerializeField] private TextMeshProUGUI m_comboLabel;

		[SerializeField] private PlayerChannel m_playerChannel = null;

		#region MonoBehaviour

		private void Start()
		{
			if (m_playerChannel != null)
			{
				m_playerChannel.onRefreshStarEnergy += CallbackRefreshStarEnergy;
				m_playerChannel.onRefreshStarCombo += CallbackRefreshStarCombo;
			}
		}

		private void OnDestroy()
		{
			if (m_playerChannel != null)
			{
				m_playerChannel.onRefreshStarEnergy -= CallbackRefreshStarEnergy;
				m_playerChannel.onRefreshStarCombo -= CallbackRefreshStarCombo;
			}
		}

		private void CallbackRefreshStarEnergy(float value)
		{
			m_energyLabel.text = String.Format("{0:0.0}", value);
		}

		private void CallbackRefreshStarCombo(int value)
		{
			m_comboLabel.text = value.ToString();
		}

		#endregion

	}

}