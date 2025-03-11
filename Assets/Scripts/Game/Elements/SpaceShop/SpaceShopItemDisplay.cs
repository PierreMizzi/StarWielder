using System;
using StarWielder.Gameplay.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StarWielder.Gameplay.Elements
{
	public class SpaceShopItemDisplay : MonoBehaviour
	{

		#region Behaviour

		private SpaceShop m_shop;

		private SpaceShopItem currentSpaceShopItem;

		[SerializeField] private Transform m_physicalItemAnchor;

		public void DisplayItem(SpaceShopItem item)
		{
			if (item == null)
			{
				return;
			}
			
			currentSpaceShopItem = item;

			GameObject phyisicalItem = Instantiate(currentSpaceShopItem.PhysicalItem, m_physicalItemAnchor);
			phyisicalItem.transform.localPosition = Vector3.zero;
			phyisicalItem.transform.localRotation = Quaternion.identity;
		}

		public void Clear()
		{
			currentSpaceShopItem = null;

		}

		#endregion

		#region Orbiter
		[SerializeField] private Orbiter m_orbiter;
		[SerializeField] private Vector2 m_orbiterMinMaxAngleTop;
		[SerializeField] private Vector2 m_orbiterMinMaxAngleBot;

		public void RandomizeOrbit()
		{
			bool topOrBot = Random.Range(0, 2) == 0;
			float rndAngle;

			if (topOrBot)
			{
				rndAngle = Mathf.Lerp(m_orbiterMinMaxAngleTop.x, m_orbiterMinMaxAngleTop.y, Random.Range(0, 1f));
				m_orbiter.SetTime(rndAngle);
				m_orbiter.isClockwise = true;
			}		
			else
			{
				rndAngle = Mathf.Lerp(m_orbiterMinMaxAngleBot.x, m_orbiterMinMaxAngleBot.y, Random.Range(0, 1f));
				m_orbiter.SetTime(rndAngle);
				m_orbiter.isClockwise = false;
			}
		}
			
		#endregion

		#region MonoBehaviour

		private void Start()
		{
			// RandomizeOrbit();
			if (name != null)
			{
				m_sunSocket.onSocket += CallbackSocket;
				m_sunSocket.onUnsocket += CallbackUnsocket;
			}
		}

		private void OnDestroy()
		{
			if (name != null)
			{
				m_sunSocket.onSocket -= CallbackSocket;
				m_sunSocket.onUnsocket -= CallbackUnsocket;
			}
		}

		#endregion

		#region Sun Socket

		[SerializeField] private SunSocket m_sunSocket;

		private void CallbackSocket(Star sun)
		{

		}

		private void CallbackUnsocket(Star sun)
		{

		}

		#endregion
	}
}