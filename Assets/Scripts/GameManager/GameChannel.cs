using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameChannel", menuName = "ScriptableObjects/Channels/GameChannel", order = 0)]
public class GameChannel : ScriptableObject
{

	public FloatDelegate onRefreshTimer;

	#region Game Over

	public FloatDelegate onSetHighestEnergy;

	public Action onGameOver;

	public Action onReplay;

	#endregion

	private void OnEnable()
	{
		onRefreshTimer = (float time) => { };

		// Score
		onSetHighestEnergy = (float highestEnergy) => { };

		onGameOver = () => { };
		onReplay = () => { };
	}

}