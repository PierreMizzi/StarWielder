using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameChannel", menuName = "ScriptableObjects/Channels/GameChannel", order = 0)]
public class GameChannel : ScriptableObject
{

	public FloatDelegate onRefreshTimer;

	public FloatDelegate onSetHighestEnergy;

	#region Game Over

	public GameOverDelegate onGameOver;

	public GameOverScreenDelegate onGameOverScreen;

	public Action onReplay;

	#endregion

	private void OnEnable()
	{
		onRefreshTimer = (float time) => { };

		// Score
		onSetHighestEnergy = (float highestEnergy) => { };

		onGameOver = (GameOverReason reason) => { };
		onGameOverScreen = (GameOverData data) => { };
		onReplay = () => { };
	}

}