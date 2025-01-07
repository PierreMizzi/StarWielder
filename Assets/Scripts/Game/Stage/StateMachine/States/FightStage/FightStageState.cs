using PierreMizzi.Useful.StateMachines;
using UnityEngine;

/*
	- Pick 3 types of enemies based on difficulty
	- Gives them to the EnemyManager
	- Stage starts, spawning starts
	- When all enemies have been killed -> Stage completed
*/

namespace StarWielder.Gameplay
{

	public class FightStageState : StageState
	{

		public FightStageState(IStateMachine stateMachine) : base(stateMachine)
		{
			type = (int)StageStateType.Fight;
			m_manager = m_this.GetStageManager<FightStageManager>();

			m_manager.onStageEnded += m_this.CallbackStageEnded;
		}

		#region Behaviour

		private new FightStageManager m_manager;

        public override void Enter(StageSettings settings)
        {
			m_manager.StartStage((FightStageSettings)settings);
		}

        // 🟥 : Implement clear ResourcesStage and Manager for debug purposes
        public override void Clear()
        {
            base.Clear();
        }

        #endregion
    }
}