using StarWielder.Gameplay.Player;
using UnityEngine;

public class ShipStats
{
	public ShipStats(PlayerSettings settings)
	{
		this.maxHealth = settings.maxHealth;
		this.maxEmergencyEnergy = settings.maxEmergencyEnergy;
		this.speed = settings.speed;
		this.dashCooldownDuration = settings.dashCooldownDuration;
	}

	public float maxHealth;
	public float maxEmergencyEnergy;
	public float speed;
	public float dashCooldownDuration;
}