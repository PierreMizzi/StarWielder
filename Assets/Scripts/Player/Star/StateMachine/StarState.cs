using PierreMizzi.Useful.StateMachines;

namespace PierreMizzi.Gameplay.Players
{
	public class StarState : AState
	{
		public StarState(IStateMachine stateMachine)
			: base(stateMachine)
		{
			m_this = m_stateMachine.gameObject.GetComponent<Star>();
		}

		protected Star m_this;
	}
}