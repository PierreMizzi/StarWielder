using System;
using UnityEngine;

namespace StarWielder.Gameplay
{

	[CreateAssetMenu(fileName = "GameChannel", menuName = "StarWielder/Channels/GameChannel", order = 0)]
	public class GameChannel : ScriptableObject
	{

		// Title Screen
		public Action onFirstDocking;
		public Action onStartGame;

		// Score
		[Obsolete]
		public FloatDelegate onRefreshTimer;

		[Obsolete]
		public FloatDelegate onSetHighestEnergy;



		// Stage


		// Currency
		public IntDelegate onCollectCurrency;

		// Mineral Nugget
		public IntDelegate onCollectMineralNugget;

		// Game Over
		public GameOverDelegate onGameOver;
		public GameOverScreenDelegate onGameOverScreen;
		public Action onReplay;

		private void OnEnable()
		{
			// Title Screen
			onStartGame = () => { };

			// Score
			onRefreshTimer = (float time) => { };
			onSetHighestEnergy = (float highestEnergy) => { };



			// Currency
			onCollectCurrency = (int amount) => { };

			// Mineral
			onCollectMineralNugget = (int amount) => { };

			// Stage

			// Game
			onGameOver = (GameOverReason reason) => { };
			onGameOverScreen = (GameOverData data) => { };
			onReplay = () => { };
		}

	}
}