using System;
using StarWielder.Gameplay.Player;
using UnityEngine;

namespace StarWielder.Gameplay.Elements
{
	public class EnergyStation : MonoBehaviour
	{

		#region Behaviour
	
		[SerializeField] private GameChannel m_gameChannel;

		private void CallbackSunSocket(Star sun)
		{
			if (m_gameChannel != null && sun != null)
			{
				m_gameChannel.onEnterEnergyStation.Invoke();
			}
		}

		private void CallbackSunUnsocket(Star sun)
		{
			if (m_gameChannel != null && sun != null)
			{
				m_gameChannel.onLeaveEnergyStation.Invoke();
			}
		}

		#endregion

		#region Sun Socket

		[Header("Sun Socket")]
		[SerializeField] private SunSocket m_sunSocket;

		private void Start()
		{
			if (m_sunSocket != null)
			{
				m_sunSocket.onSocket += CallbackSunSocket;
				m_sunSocket.onUnsocket += CallbackSunUnsocket;
			}
		}

		private void OnDestroy()
		{
			if (m_sunSocket != null)
			{
				m_sunSocket.onSocket -= CallbackSunSocket;
				m_sunSocket.onUnsocket -= CallbackSunUnsocket;
			}
		}

		#endregion

	}
}