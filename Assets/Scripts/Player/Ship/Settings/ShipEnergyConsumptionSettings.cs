using StarWielder.Gameplay.Player;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipEnergySettings", menuName = "StarWielder/Player/ShipEnergySettings", order = 0)]
public class ShipEnergyConsumptionSettings : ScriptableObject
{
	public Ship.EnergyConsumptionMode mode;

	[Header("Star Power")]

	[Tooltip("Star's energy depleate speed when docked to the hip")]
	public float dockedEnergyDepleateSpeed = 2.5f;


	[Header("Emergency Power")]
	public float maxEmergencyEnergy = 10f;

	// [Tooltip("How fast the Ship draws energy from the Star when docked")]
	[Tooltip("How fast the Ship consume it's emergency energy")]
	public float emergencyEnergyDepleatRate = 2f;

	[Tooltip("NO INFLUENCE ! Calculated from maxEmergencyEnergy & emergencyEnergyDepleatRate")]
	public float emergencyEnergyDuration;

	[Header("UI")]
	[Header("Fight Mode")]
	public string labelName = "FIGHT";
	public Color labelColor = Color.red;

	private void OnValidate()
	{
		emergencyEnergyDuration = maxEmergencyEnergy / emergencyEnergyDepleatRate;
	}

}