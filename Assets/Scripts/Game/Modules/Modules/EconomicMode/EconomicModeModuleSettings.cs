using UnityEngine;

namespace StarWielder.Gameplay.Modules
{
	[CreateAssetMenu(fileName = "EconomicModeModuleSettings", menuName = "StarWielder/EconomicModeModuleSettings", order = 0)]
	public class EconomicModeModuleSettings : BaseModuleSettings
	{
		[SerializeField] private float m_immobileDelay = 1.0f;
		public float ImmobileDelay => m_immobileDelay;

		[SerializeField] private ShipEnergyConsumptionSettings m_energyConsumptionSettings;
	}
}