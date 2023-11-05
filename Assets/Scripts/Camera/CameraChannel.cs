using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraChannel", menuName = "ScriptableObjects/Channels/CameraChannel", order = 1)]
public class CameraChannel : ScriptableObject
{

	public Action onStartEnergyTransfer;
	public Action onStopEnergyTransfer;

	public Action onShipHurt;


	private void OnEnable()
	{
		onStartEnergyTransfer = () => { };
		onStopEnergyTransfer = () => { };

		onShipHurt = () => { };
	}

}