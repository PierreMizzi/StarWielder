using PierreMizzi.Useful.StateMachines;

namespace StarWielder.Gameplay.Player
{

	/// <summary>
	/// Main class for all Star's sub-states
	/// </summary>
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