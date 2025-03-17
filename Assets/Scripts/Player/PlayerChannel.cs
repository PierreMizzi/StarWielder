using System;
using UnityEngine;

namespace StarWielder.Gameplay.Player
{

	[CreateAssetMenu(fileName = "PlayerChannel", menuName = "StarWielder/Channels/PlayerChannel", order = 1)]
	public class PlayerChannel : ScriptableObject
	{
		#region Energy

		public FloatDelegate onAbsorbEnemyStar;
		public FloatDelegate onRefreshStarEnergy;
		public Action onStarDocked;
		public Action onStarFree;
		public Action onStartEnergyTransfer;
		public Action onStopEnergyTransfer;

		public Ship.EnergyConsumptionModeDelegate onSetEnergyConsumptionMode;
		public Action onSetAppropriateEnergyConsumptionMode;

		#endregion

		#region Emergency Energy

		public FloatDelegate onIncrementEmergencyEnergy = (float energy) => { };
		public FloatDelegate onRefreshEmergencyEnergy;

		#endregion

		#region Health

		public FloatDelegate onIncrementShipHealth = (float addedHealth) => { };
		public FloatDelegate onDecrementShipHealth = (float lostHealth) => { };
		public FloatDelegate onRefreshShipHealth;

		#endregion

		#region Combo

		private int m_currentCombo;
		public int currentCombo
		{
			get
			{
				return m_currentCombo;
			}
			set
			{
				m_currentCombo = Mathf.Max(0, value);
			}
		}

		public Action onComboIncrement;
		public Action onComboBreak;

		[ContextMenu("Call IncrementCombo")]
		public void IncrementCombo()
		{
			onComboIncrement.Invoke();
		}

		#endregion

		#region Controller

		public BoolDelegate onIsImmobile;

		#endregion

		public void OnEnable()
		{
			// Energy
			onAbsorbEnemyStar = (float energy) => { };
			onRefreshStarEnergy = (float energy) => { };
			onStarDocked = () => { };
			onStarFree = () => { };
			onStartEnergyTransfer = () => { };
			onStopEnergyTransfer = () => { };

			onSetEnergyConsumptionMode = (Ship.EnergyConsumptionMode mode) => { };
			onSetAppropriateEnergyConsumptionMode = () => { };

			onRefreshEmergencyEnergy = (float energy) => { };

			// Health
			onRefreshShipHealth = (float normalizedHealth) => { };

			// Combo
			onComboIncrement = () => { m_currentCombo++; };
			onComboBreak = () => { m_currentCombo = 0; };

			// Controller
			onIsImmobile = (bool value) => { };
		}
	}
}