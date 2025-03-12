using StarWielder.Gameplay;
using StarWielder.Gameplay.Player;
using UnityEngine;

namespace StarWielder.Gameplay
{
	public class ShopStageManager : StageStateManager
	{
        [SerializeField] private GameObject m_container;

        public override void StartStage()
        {
            base.StartStage();
			m_playerChannel.onSetEnergyConsumptionMode(Ship.EnergyConsumptionMode.Infinite);
            m_container?.SetActive(true);
        }

        public override void StopStage()
        {
            base.StopStage();
            m_container?.SetActive(false);

        } 
        
    }
}