using System;
using UnityEngine;

namespace StarWielder.Gameplay
{

	[CreateAssetMenu(fileName = "GameChannel", menuName = "ScriptableObjects/Channels/GameChannel", order = 0)]
	public class GameChannel : ScriptableObject
	{

		// Title Screen
		public Action onFirstDocking;
		public Action onStartGame;

		// Score
		public FloatDelegate onRefreshTimer;
		public FloatDelegate onSetHighestEnergy;

		// Stage


		// Currency
		public IntDelegate onCollectCurrency;

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

			// Stage

			// Game
			onGameOver = (GameOverReason reason) => { };
			onGameOverScreen = (GameOverData data) => { };
			onReplay = () => { };
		}

	}
}