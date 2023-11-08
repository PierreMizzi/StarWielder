using UnityEngine;

namespace QGamesTest.Gameplay.Player
{

	/// <summary>
	/// All gameplay & movement related settings for the Player
	/// </summary>
	[CreateAssetMenu(fileName = "StarSettings", menuName = "ScriptableObjects/StarSettings", order = 1)]
	public class StarSettings : ScriptableObject
	{

		[Header("Speed")]
		public float baseSpeed;

		[Tooltip("Scalar value paired with Star's energy. The higher the energy, the faster the Sun")]
		public float speedFromEnergyRatio = 0.1f;

		[Tooltip("Scalar value paired with Star's velocity. The higher the velocity, the stronger the squish")]
		public float squishRatio = 0.005f;

		public float minScaleFromVelocity = 0.5f;

		[Header("Returning")]
		public float acceleration = 15.0f;
		public float maxSpeed = 20.0f;
		public float arrivalDistance = 0.1f;

		[Header("Energy")]
		[Tooltip("Starting amount of energy")]
		public float baseEnergy;

		[Tooltip("Star's energy depleate speed when docked to the hip")]
		public float dockedEnergyDepleateSpeed = 2.5f;
		public float comboBonusEnergyRatio = 0.33f;

		[Header("Transfer")]
		public float transferBaseDuration = 0.5f;
		public float transferDurationRatio = 0.05f;

		[Header("Combo")]
		public float pitchShift = 0.1f;
		public float maxPitch = 1.7f;
	}
}