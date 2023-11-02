using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameChannel", menuName = "ScriptableObjects/Channels/GameChannel", order = 0)]
public class GameChannel : ScriptableObject
{

	public FloatDelegate onRefreshTimer;

	public Action onGameOver;


	private void OnEnable()
	{
		onRefreshTimer = (float time) => { };

		onGameOver = () => { };
	}

}