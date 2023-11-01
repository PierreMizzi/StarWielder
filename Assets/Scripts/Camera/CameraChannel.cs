using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraChannel", menuName = "ScriptableObjects/Channels/CameraChannel", order = 1)]
public class CameraChannel : ScriptableObject
{

	public Action onFocusShip;
	public Action onFocusDefault;

	private void OnEnable()
	{
		onFocusShip = () => { };
		onFocusDefault = () => { };
	}

}