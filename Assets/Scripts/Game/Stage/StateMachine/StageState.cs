using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace StarWielder.Gameplay
{
	public class StageState : AState
	{
		public StageState(IStateMachine stateMachine) : base(stateMachine)
		{
			m_this = m_stateMachine.gameObject.GetComponent<StageManager>();
		}

		protected StageManager m_this;

		protected StageStateManager m_manager;

		protected virtual void CallbackStageEnded()
		{
			Debug.Log("Stage ended !");
			m_this.CallbackStageEnded();
		}

		// TODO : use StageStateManager -> FightStageManager (ex-EnemyManager)
		//								-> ResourcesStagaManager

	}
}