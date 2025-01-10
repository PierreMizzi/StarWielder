using System;
using StarWielder.Gameplay.Player;
using UnityEngine;

namespace StarWielder.Gameplay.Elements
{
	public class EnergyStation : MonoBehaviour
	{

		#region Sun Socket

		[Header("Sun Socket")]
		[SerializeField] private SunSocket m_sunSocket;

		private void Start()
		{
			if (m_sunSocket != null)
			{
				m_sunSocket.onSocket += CallbackSunSocket;
				m_sunSocket.onSocket += CallbackSunUnsocket;
			}
		}

		private void CallbackSunSocket(Star sun)
		{
			if (sun != null)
			{
				// 	
				

			}
		}

		private void CallbackSunUnsocket(Star sun)
		{
			if (sun != null)
			{

			}
		}

		#endregion

	}
}