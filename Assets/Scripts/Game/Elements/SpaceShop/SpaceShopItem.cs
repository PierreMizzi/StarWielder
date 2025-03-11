
using System;
using UnityEngine;

namespace StarWielder.Gameplay.Elements
{
	[Serializable]
	public class SpaceShopItem
	{
		[SerializeField] protected string m_name;
		[SerializeField] protected string m_description;
		[SerializeField] protected int m_price;
		[SerializeField] protected Sprite m_sprite;
		[SerializeField] protected GameObject m_physicalItem;

		protected Type ItemType { get { return Type.None; } }
		public GameObject PhysicalItem => m_physicalItem;

		public enum Type
		{
			None,
			Module,
		}
		
	}

	public class SpaceShopItemModule : SpaceShopItem
	{
		protected new Type ItemType { get { return Type.Module; } }
	}
}
