using StarWielder.Gameplay.Player;
using UnityEngine;

public class ShipStats
{
	public ShipStats(Ship ship)
	{
		this.maxHealth = ship.settings.maxHealth;
		// 🟥 : Fix maxEmergencyEnergy stats
		this.maxEmergencyEnergy = ship.CurrentEnergyConsumptionMode.maxEmergencyEnergy;
		this.speed = ship.settings.speed;
		this.dashCooldownDuration = ship.settings.dashCooldownDuration;
	}

	public float maxHealth;
	public float maxEmergencyEnergy;
	public float speed;
	public float dashCooldownDuration;
}