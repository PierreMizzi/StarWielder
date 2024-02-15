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

		protected override void DefaultEnter()
		{
			base.DefaultEnter();
			m_manager.StartStage();
		}



	}
}