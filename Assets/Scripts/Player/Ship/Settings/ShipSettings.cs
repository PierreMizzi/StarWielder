using System;
using System.Collections.Generic;
using UnityEngine;

namespace StarWielder.Gameplay.Player
{
	/// <summary>
	/// All gameplay & movement related settings for the Player
	/// </summary>
	[CreateAssetMenu(fileName = "ShipSettings", menuName = "StarWielder/Player/ShipSettings", order = 0)]
	public class ShipSettings : ScriptableObject
	{

		[Header("Health")]
		public float maxHealth = 100f;

		[Header("Energy")]

		public List<ShipEnergyConsumptionSettings> energyConsumptionSettings;
		public ShipEnergyConsumptionSettings defaultEnergyConsumptionSettings;

		public ShipEnergyConsumptionSettings GetEnergyConsumptionSettingsFromMode(Ship.EnergyConsumptionMode mode)
		{
			ShipEnergyConsumptionSettings energySettings = energyConsumptionSettings.Find(setting => setting.mode == mode);

			if (energySettings != null)
				return energySettings;
			else
			{
				return defaultEnergyConsumptionSettings;
			}
		}

		[Header("Speed")]
		public float speed = 6.5f;
		public float friction = 6f;

		[Header("Dash")]
		public float dashDistance = 3f;
		public float dashDuration = 0.2f;
		public float dashCooldownDuration = 3f;




	}

}