using StarWielder.Gameplay.Enemies;
using StarWielder.Gameplay.Player;
using UnityEngine;

namespace StarWielder.Gameplay
{
	public class FightStageManager : StageStateManager
	{

		#region MonoBehaviour

		private void Start()
		{
			m_enemyManager.Initialize(this);
		}

		#endregion

		#region Behaviour

		[SerializeField] private EnemyManager m_enemyManager;

		public virtual void StartStage(FightStageSettings settings)
		{
			m_playerChannel.onSetEnergyConsumptionMode.Invoke(Ship.EnergyConsumptionMode.Fight);
			m_enemyManager.SetupStageSettings(settings);
			m_enemyManager.StartStage();
		}

        public override void CallbackGameOver()
        {
            base.CallbackGameOver();
			m_enemyManager.CallbackGameOver();
		}

        #endregion

    }
}
