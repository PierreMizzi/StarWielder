using System;
using UnityEngine;

namespace StarWielder.Gameplay.Player
{


	[CreateAssetMenu(fileName = "PlayerChannel", menuName = "ScriptableObjects/Channels/PlayerChannel", order = 1)]
	public class PlayerChannel : ScriptableObject
	{
		#region Energy

		public FloatDelegate onAbsorbEnemyStar;
		public FloatDelegate onRefreshStarEnergy;
		public Action onStarDocked;
		public Action onStarFree;
		public Action onStartEnergyTransfer;
		public Action onStopEnergyTransfer;

		#endregion

		#region Emergency Energy

		public FloatDelegate onRefreshEmergencyEnergy;

		#endregion

		#region Health

		public FloatDelegate onShipHurt;

		#endregion

		#region Combo

		public IntDelegate onRefreshStarCombo;

		#endregion

		public void OnEnable()
		{
			onAbsorbEnemyStar = (float energy) => { };
			onRefreshStarEnergy = (float energy) => { };
			onStarDocked = () => { };
			onStarFree = () => { };
			onStartEnergyTransfer = () => { };
			onStopEnergyTransfer = () => { };

			onRefreshEmergencyEnergy = (float energy) => { };

			onShipHurt = (float normalizedHealth) => { };

			onRefreshStarCombo = (int combo) => { };
		}
	}
}