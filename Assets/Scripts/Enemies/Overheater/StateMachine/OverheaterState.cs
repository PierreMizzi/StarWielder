using UnityEngine;
using PierreMizzi.Useful.StateMachines;

namespace StarWielder.Gameplay.Enemies
{

	public class OverheaterState : AState
	{
		public OverheaterState(IStateMachine stateMachine)
			: base(stateMachine)
		{
			m_this = m_stateMachine.gameObject.GetComponent<Overheater>();
		}

		protected Overheater m_this;

	}
}