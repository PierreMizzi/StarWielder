using System.Collections.Generic;
using UnityEngine;

namespace StarWielder.Gameplay
{
	[CreateAssetMenu(fileName = "ShopStageSettings", menuName = "StarWielder/StageSettings/Shop", order = 0)]
	public class ShopStageSettings : StageSettings
	{

		public ShopStageSettings()
		{
			m_type = StageStateType.Shop;
		}

	}
}