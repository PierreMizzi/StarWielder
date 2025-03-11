using System.Collections.Generic;
using System.Linq;
using PierreMizzi.Useful;
using StarWielder.Gameplay.Modules;
using UnityEngine;



namespace StarWielder.Gameplay.Elements
{
	public class SpaceShop : MonoBehaviour
	{

		/*
			Une pipelette
		*/
		#region Behaviour

		[SerializeField] private ModuleChannel m_moduleChannel;

		[SerializeField] private List<SpaceShopItemDisplay> m_itemDisplays;

		public void GenerateShopContent()
		{
			List<BaseModule> buyableModules = GetBuyableModules();

			BaseModule module;
			SpaceShopItemDisplay itemDisplay;

			for (int i = 0; i < buyableModules.Count; i++)
			{
				// m_itemDisplays
				module = buyableModules[i];
				itemDisplay = m_itemDisplays[i];


			}
		}

		public void RandomizeItemDisplay()
		{
			foreach (SpaceShopItemDisplay itemDisplay in m_itemDisplays)
			{
				itemDisplay.RandomizeOrbit();
			}
		}

		#endregion

		#region MonoBehaviour

		private void Start()
		{
			RandomizeItemDisplay();
		}
			
		#endregion

		#region Module

		public List<BaseModule> GetBuyableModules()
		{
			List<BaseModule> buyableModules = (from module in m_moduleChannel.moduleManager.Modules 
											  where module.Value.IsBuyable select module.Value).ToList();

			return buyableModules.PickRandom(2, true);
		}


			
		#endregion

	}

}
