using System;
using System.Collections.Generic;
using System.Linq;
using PierreMizzi.Useful;
using StarWielder.Gameplay.Modules;
using UnityEngine;


namespace StarWielder.Gameplay.Elements
{

	public delegate void ShopItemDelegate(ShopItem shopItem);

	public class SpaceShop : MonoBehaviour
	{

		#region Behaviour

		[SerializeField] private int m_amount;

		[SerializeField] private ModuleChannel m_moduleChannel;
		[SerializeField] private GameChannel m_gameChannel;

		[SerializeField] private List<ShopItemDisplay> m_itemDisplays;

		public void GenerateShopContent()
		{
			List<BaseModule> buyableModules = GetRandomBuyableModules(m_amount);

			BaseModule module;
			ShopItemDisplay itemDisplay;

			for (int i = 0; i < buyableModules.Count; i++)
			{
				module = buyableModules[i];
				itemDisplay = m_itemDisplays[i];
				itemDisplay.AssignShopIten(module.Settings.ShopItem);
			}
		}

		public void RandomizeItemDisplay()
		{
			List<PlanetConfig> selectedPlanetConfigs = m_planetConfigs.PickRandom(3);
			ShopItemDisplay itemDisplay;

			for (int i = 0; i < m_itemDisplays.Count; i++)
			{
				itemDisplay = m_itemDisplays[i];
				itemDisplay.AssignPlanetConfig(selectedPlanetConfigs[i]);
			}
		}

		#endregion

		#region MonoBehaviour

		private void Start()
		{
			RandomizeItemDisplay();
			GenerateShopContent();
		}

		#endregion

		#region Module

		public List<BaseModule> GetRandomBuyableModules(int amount)
		{
			List<BaseModule> buyableModules = (from module in m_moduleChannel.moduleManager.Modules
											   where module.Value.IsBuyable
											   select module.Value).ToList();

			return buyableModules.PickRandom(amount, true);
		}

		#endregion

		#region Planets

		[Header("Planets")]

		[SerializeField] private List<PlanetConfig> m_planetConfigs;

		[Serializable]
		public struct PlanetConfig
		{
			public string name;
			public Sprite sprite;
			[ColorUsage(false, true)] public Color orbitColor;
			[ColorUsage(false, true)] public Color trailColor;
		}

		#endregion

		#region Debug

		[Header("Debug")]
		[SerializeField] private List<PlanetConfig> m_savedPlanetConfigs;

		[ContextMenu("Call ApplyPlanetConfigs")]
		public void ApplyPlanetConfigs()
		{
			if (m_itemDisplays.Count != m_planetConfigs.Count)
			{
				return;
			}

			ShopItemDisplay itemDisplay;
			PlanetConfig planetConfig;

			for (int i = 0; i < m_itemDisplays.Count; i++)
			{
				itemDisplay = m_itemDisplays[i];
				planetConfig = m_planetConfigs[i];

				itemDisplay.AssignPlanetConfig(planetConfig);
			}
		}

		[ContextMenu("Call SavePlanetConfigs")]
		public void SavePlanetConfigs()
		{
			m_savedPlanetConfigs.Clear();

			int index = 0;
			foreach (ShopItemDisplay itemDisplay in m_itemDisplays)
			{
				if (itemDisplay == null ||
					itemDisplay.OrbitSprite == null ||
					itemDisplay.PlanetSpriteRenderer == null)
				{
					continue;
				}

				PlanetConfig config = new ()
				{
					name = "Planet_" + index,
					sprite = itemDisplay.PlanetSpriteRenderer.sprite,
					orbitColor = itemDisplay.OrbitSprite.GetProperty(ShopItemDisplay.colorOrbitColor).colorValue,
					trailColor = itemDisplay.OrbitSprite.GetProperty(ShopItemDisplay.colorTrailColor).colorValue,
				};

				m_savedPlanetConfigs.Add(config);
				index++;
			}
		}		

		#endregion

	}

}
