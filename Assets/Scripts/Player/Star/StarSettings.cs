using UnityEngine;

[CreateAssetMenu(fileName = "StarSettings", menuName = "ScriptableObjects/StarSettings", order = 1)]
public class StarSettings : ScriptableObject
{

	[Header("Speed")]
	public float baseSpeed;
	public float speedFromEnergyRatio = 0.1f;
	public float squishRatio = 0.9f;

	[Header("Returning")]

	public float acceleration = 15.0f; // L'accélération de l'objet
	public float maxSpeed = 20.0f; // La vitesse maximale de l'objet
	public float arrivalDistance = 0.1f; // La distance minimale pour considérer l'arrivée

	[Header("Energy")]
	public float baseEnergy;
	public float energyDepleatRate = 0.5f;
	// public float lostEnergyCollision;
	public float comboBonusEnergyRatio = 0.33f;
	public float transferBaseDuration = 0.5f;
	public float transferDurationRatio = 0.05f;

	[Header("Enemy")]
	public float pitchShift = 0.1f;
	public float maxPitch = 1.7f;
}