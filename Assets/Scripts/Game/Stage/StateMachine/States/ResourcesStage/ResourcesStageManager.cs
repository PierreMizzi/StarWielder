using UnityEngine;
using StarWielder.Gameplay.Elements;
using StarWielder.Gameplay.Player;

namespace StarWielder.Gameplay
{

	public class ResourcesStageManager : StageStateManager
	{

		public override void StartStage()
		{
			m_playerChannel.onSetEnergyConsumptionMode.Invoke(Ship.EnergyConsumptionMode.Low);
			m_asteroidSpawnerManager.CreateAsteroidStorm();
		}

		#region Asteroid Tempest

		[SerializeField] private AsteroidSpawnerManager m_asteroidSpawnerManager;

		#endregion

	}
}
