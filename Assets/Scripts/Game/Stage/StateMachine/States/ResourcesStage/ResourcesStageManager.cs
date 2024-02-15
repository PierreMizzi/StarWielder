using UnityEngine;

namespace StarWielder.Gameplay
{

	public class ResourcesStageManager : StageStateManager
	{

		public override void StartStage()
		{
			m_asteroidSpawnerManager.CreateAsteroidStorm();
		}


		#region Asteroid Tempest

		[SerializeField] private AsteroidSpawnerManager m_asteroidSpawnerManager;

		#endregion

	}
}
