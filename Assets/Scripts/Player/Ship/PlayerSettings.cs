using UnityEngine;

namespace QGamesTest.Gameplay.Player
{

	[CreateAssetMenu(fileName = "ShipSettings", menuName = "ScriptableObjects/Settings/ShipSettings", order = 0)]
	public class PlayerSettings : ScriptableObject
	{

		[Header("Health")]
		public float maxHealth = 300f;

		[Header("Emergency Energy")]
		public float maxEmergencyEnergy = 40f;

		// [Tooltip("How fast the Ship draws energy from the Star when docked")]
		[Tooltip("How fast the Ship consume it's emergency energy")]
		public float emergencyEnergyDepleatRate = 0.25f;

		[Tooltip("NO INFLUENCE ! Calculated from maxEmergencyEnergy & emergencyEnergyDepleatRate")]
		public float emergencyEnergyDuration;

		[Header("Speed")]
		public float speed = 80f;
		public float smoothTime = 0.5f;

		[Header("Dash")]
		public float dashDistance = 1f;
		public float dashDuration = 0.2f;
		public float dashCooldownDuration = 3f;

		private void OnValidate()
		{
			emergencyEnergyDuration = maxEmergencyEnergy / emergencyEnergyDepleatRate;
		}


	}

}