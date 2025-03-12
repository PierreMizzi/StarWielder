
using System;
using StarWielder.Gameplay.Modules;
using UnityEngine;

namespace StarWielder.Gameplay.Elements
{
	[Serializable]
	public class ShopItem
	{
		public string name;
		
		[TextArea(5, 5)] public string description;
		public int price;
		public Sprite sprite;

		public virtual Type ItemType()
		{
			return Type.None;
		}

		public enum Type
		{
			None,
			Module,
		}
		
	}

	[Serializable]
	public class SpaceShopItemModule : ShopItem
	{
		public override Type ItemType()
		{
			return Type.Module;
		}
		[HideInInspector] public ModuleType moduleType;
	}
}
