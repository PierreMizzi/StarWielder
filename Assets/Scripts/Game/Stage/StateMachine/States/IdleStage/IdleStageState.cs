using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace StarWielder.Gameplay
{

	public class IdleStageState : StageState
	{
		public IdleStageState(IStateMachine stateMachine) : base(stateMachine)
		{
			type = (int)StageStateType.Idle;
			m_manager = m_this.GetStageManager<IdleStageManager>();
			m_manager.onStageEnded += m_this.CallbackStageEnded;
		}

		private new IdleStageManager m_manager;

        public override void Enter(StageSettings settings)
        {
            m_manager.StartStage(settings);
        }

        public override void Clear()
        {
			Debug.Log("Clear Idle !!");
            base.Clear();
        }

    }
}