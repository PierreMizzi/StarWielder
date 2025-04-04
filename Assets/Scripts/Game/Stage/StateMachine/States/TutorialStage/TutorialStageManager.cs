using StarWielder.Gameplay;
using StarWielder.Gameplay.Player;
using UnityEngine;

namespace StarWielder.Gameplay
{
	public class TutorialStageManager : StageStateManager
	{

		public override void StartStage()
		{
			base.StartStage();
			m_playerChannel.onSetEnergyConsumptionMode(Ship.EnergyConsumptionMode.Infinite);
		}

	}
}