using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace StarWielder.Gameplay
{

	public class TutorialStageState : StageState
	{
		public TutorialStageState(IStateMachine stateMachine) : base(stateMachine)
		{
			type = (int)StageStateType.Tutorial;
			m_manager = m_this.GetStageManager<TutorialStageManager>();
			m_manager.onStageEnded += m_this.CallbackStageEnded;
		}

		private new TutorialStageManager m_manager;

		public override void Enter(StageSettings settings)
		{
			base.Enter(settings);
			m_manager.StartStage();
		}
	}
}