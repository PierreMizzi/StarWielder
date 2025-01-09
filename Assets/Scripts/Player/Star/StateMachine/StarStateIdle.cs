using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace StarWielder.Gameplay.Player
{
	/// <summary>
	/// Star's doing nothing, could be useful in the futur
	/// </summary>
	public class StarStateIdle : StarState
	{

		public StarStateIdle(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)StarStateType.Idle;
		}

		protected override void DefaultEnter()
		{
			base.DefaultEnter();
			m_this.transform.SetParent(null);
			m_this.rigidbody.velocity = Vector2.zero;
		}


	}
}