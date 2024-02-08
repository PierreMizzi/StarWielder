using UnityEngine;

namespace StarWielder.Gameplay
{
	public class ResourcesStageManager : StageStateManager
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
}
