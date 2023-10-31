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

			// onStarReleased = () => { };
			// onStarReleased = () => { };

			onRefreshStarEnergy = (float energy) => { };
			onRefreshStarCombo = (int combo) => { };
		}

	}
}