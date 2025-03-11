using System;
using StarWielder.Gameplay.Player;
using UnityEngine;

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

		#region MonoBehaviour

		private void Start()
		{
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