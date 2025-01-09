using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace StarWielder.Gameplay.Player
{
	/// <summary>
	/// Star's state when the game is over. No more bouncing
	/// </summary>
	public class StarStateDying : StarState
	{

		public StarStateDying(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)StarStateType.Dying;
		}

        protected override void DefaultEnter()
        {
            base.DefaultEnter();
			m_this.transform.SetParent(null);
			m_this.rigidbody.velocity = Vector2.zero;
        }

    }
}