using System;
using UnityEngine;

namespace PierreMizzi.Gameplay.Players
{


	[CreateAssetMenu(fileName = "PlayerChannel", menuName = "ScriptableObjects/Channels/PlayerChannel", order = 1)]
	public class PlayerChannel : ScriptableObject
	{
		/*
			Ship
		*/
		public FloatDelegate onRefreshShipEnergy;

		public Action onUseDash;
		public FloatDelegate onRefreshCooldownDash;
		public Action onRechargeDash;

		/*
			Star
		*/
		// public Action onStarReleased;
		// public Action onStarDocked;

		public FloatDelegate onRefreshStarEnergy;
		public IntDelegate onRefreshStarCombo;


		public void OnEnable()
		{
			onRefreshShipEnergy = (float energy) => { };

			onUseDash = () => { };
			onRefreshCooldownDash = (float value) => { };
			onRechargeDash = () => { };

			// onStarReleased = () => { };
			// onStarReleased = () => { };

			onRefreshStarEnergy = (float energy) => { };
			onRefreshStarCombo = (int combo) => { };
		}

	}
}