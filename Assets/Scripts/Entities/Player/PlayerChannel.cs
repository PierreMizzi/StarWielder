using System;
using UnityEngine;

namespace PierreMizzi.Gameplay.Players
{


	[CreateAssetMenu(fileName = "PlayerChannel", menuName = "ScriptableObjects/Channels/PlayerChannel", order = 1)]
	public class PlayerChannel : ScriptableObject
	{

		public Action onStarReleased;
		public Action onStarDocked;

		public FloatDelegate onRefreshShipEnergy;

		public void OnEnable()
		{
			onStarReleased = () => { };
			onStarReleased = () => { };

			onRefreshShipEnergy = (float energy) => { };
		}

	}
}