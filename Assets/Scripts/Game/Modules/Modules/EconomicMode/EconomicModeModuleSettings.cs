using UnityEngine;

namespace StarWielder.Gameplay.Modules
{
	[CreateAssetMenu(fileName = "ModuleSettings_", menuName = "StarWielder/Modules/EconomicModeModuleSettings", order = 0)]
	public class EconomicModeModuleSettings : BaseModuleSettings
	{

		[Header("Behaviour")]
		[SerializeField] private float m_immobileDelay = 1.0f;
		public float ImmobileDelay => m_immobileDelay;

		[SerializeField] private ShipEnergyConsumptionSettings m_energyConsumptionSettings;
	}
}