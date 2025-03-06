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

		// Shop
		public Action onEnterEnergyStation = () => { };
		public Action onLeaveEnergyStation = () => { };

		// Energy
		public FloatDelegate onSunIncrementEnergy;

		// Currency
		public int currencyCurrentAmount;
		public IntDelegate onIncrementCurrency;
		public IntDelegate onDecrementCurrency;

		// Mineral Nugget
		public int mineralNuggetCurrentAmount;
		public IntDelegate onIncrementMineralNugget;
		public IntDelegate onDecrementMineralNugget;

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
			onIncrementCurrency = IncrementCurrency;
			onDecrementCurrency = DecrementCurrency;

			// Mineral
			onIncrementMineralNugget = IncrementMineralNugget;
			onDecrementMineralNugget = DecrementMineralNugget;

			// Stage

			// Game
			onGameOver = (GameOverReason reason) => { };
			onGameOverScreen = (GameOverData data) => { };
			onReplay = () => { };
		}

		public void IncrementCurrency(int amount)
		{
			currencyCurrentAmount += amount;
		}

		public void DecrementCurrency(int amount)
		{
			currencyCurrentAmount -= amount;
			currencyCurrentAmount = Math.Max(currencyCurrentAmount, 0);
		}

		public bool HasEnoughCurrency(float neededCurrency)
		{
			return neededCurrency < currencyCurrentAmount;
		}

		public void IncrementMineralNugget(int amount)
		{
			mineralNuggetCurrentAmount += amount;
		}

		public void DecrementMineralNugget(int amount)
		{
			mineralNuggetCurrentAmount -= amount;
			mineralNuggetCurrentAmount = Math.Max(currencyCurrentAmount, 0);
		}

	}
}