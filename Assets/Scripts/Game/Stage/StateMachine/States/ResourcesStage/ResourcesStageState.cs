using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace StarWielder.Gameplay
{

	public class ResourcesStageState : StageState
	{
		public ResourcesStageState(IStateMachine stateMachine) : base(stateMachine)
		{
			type = (int)StageStateType.Resources;
			m_manager = m_this.GetStageManager<ResourcesStageManager>();
			m_manager.onStageEnded += m_this.CallbackStageEnded;
		}

 		private new ResourcesStageManager m_manager;

        public override void Enter(StageSettings settings)
        {
            base.Enter(settings);
			// m_manager.StartStage((ResourcesStageSettings)settings);
			m_manager.StartStage();
		}

		// 🟥 : Implement clear ResourcesStage and Manager for debug purposes
        public override void Clear()
        {
            base.Clear();
        }

    }
}