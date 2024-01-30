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

		// Currency
		public IntDelegate onCollectCurrency;

		// Stages
		public FightStageDelegate onFightStageStart;
		public Action onFightStageEnd;

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
			onFightStageStart = (FightStageData data) => { };
			onFightStageEnd = () => { };

			// Game
			onGameOver = (GameOverReason reason) => { };
			onGameOverScreen = (GameOverData data) => { };
			onReplay = () => { };
		}

	}
}