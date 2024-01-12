using PierreMizzi.Useful.StateMachines;

namespace StarWielder.Gameplay.Player
{
	/// <summary>
	/// Main class for all Ship's sub-states
	/// </summary>
	public class ShipState : AState
	{
		public ShipState(IStateMachine stateMachine)
			: base(stateMachine)
		{
			m_this = m_stateMachine.gameObject.GetComponent<Ship>();
		}

		/// <summary>
		/// The Ship !
		/// </summary>
		protected Ship m_this;
	}
}