using StarWielder.Gameplay.Elements;
using UnityEngine;

namespace StarWielder.Gameplay.Modules
{
	/// <summary>
	/// Parent settings class of a Module
	/// </summary>
	[CreateAssetMenu(fileName = "ModuleSettings_", menuName = "StarWielder/Modules/BaseModuleSettings", order = 0)]
	public class BaseModuleSettings : ScriptableObject
	{
		[Header("Base Module Settings")]
		[SerializeField] protected ModuleType m_type;
		public ModuleType Type => m_type;

		[SerializeField] private bool m_autoEnable;
		public bool AutoEnable => m_autoEnable;

		private void OnEnable()
		{
			m_shopItem.moduleType = m_type;
		}

		#region Shop Item
		
		[Header("Shop Item")]
		[SerializeField] protected SpaceShopItemModule m_shopItem;
		public SpaceShopItemModule ShopItem => m_shopItem;

		#endregion
	}
}