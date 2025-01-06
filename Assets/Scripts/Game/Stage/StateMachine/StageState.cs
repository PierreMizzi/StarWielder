using PierreMizzi.Useful.StateMachines;
using UnityEditor;
using UnityEngine;

namespace StarWielder.Gameplay
{
	public class StageState : AState
	{
		public StageState(IStateMachine stateMachine) : base(stateMachine)
		{
			m_this = m_stateMachine.gameObject.GetComponent<StageManager>();
		}

		public virtual void Enter(StageSettings settings)
		{

		}

		protected StageManager m_this;

		protected StageStateManager m_manager;
	}
}