using PierreMizzi.Useful.StateMachines;

namespace PierreMizzi.Gameplay.Players
{
	public class ShipState : AState
	{
		public ShipState(IStateMachine stateMachine)
			: base(stateMachine)
		{
			m_this = m_stateMachine.gameObject.GetComponent<Ship>();
		}

		protected Ship m_this;
	}
}