using System;
using StarWielder.Gameplay.Player;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{

	#region MonoBehaviour

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.H))
			SpawnHealthFlower();
	}

	#endregion

	#region Health Flower

	[SerializeField] private GameObject m_asteroidWithHealthFlower;

	private void SpawnHealthFlower()
	{
		// Instantiate();

	}

	#endregion
}