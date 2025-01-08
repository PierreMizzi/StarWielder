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

		public virtual void Enter(StageSettings settings) { }

		public virtual void Clear() { }
		public virtual void CallbackGameOver() {}

		protected StageManager m_this;

		protected StageStateManager m_manager;
	}
}