using UnityEngine;
using StarWielder.Gameplay.Player;

namespace StarWielder.Gameplay
{
	public class IdleStageManager : StageStateManager
	{
        public virtual void StartStage(StageSettings settings)
        {
			base.StartStage();
			m_playerChannel.onSetEnergyConsumptionMode.Invoke(Ship.EnergyConsumptionMode.Infinite);
        }
	}
}