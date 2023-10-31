using UnityEngine;

[CreateAssetMenu(fileName = "StarSettings", menuName = "ScriptableObjects/StarSettings", order = 1)]
public class StarSettings : ScriptableObject
{

	[Header("Speed")]
	public float baseSpeed;

	[Header("Energy")]
	public float baseEnergy;
	public float lostEnergyCollision;
	public float gainedEnergyEnemy;

}