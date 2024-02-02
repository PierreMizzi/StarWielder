using System;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarWielder.Gameplay.Player
{
	/// <summary>
	/// Star's state when it's docked on the Ship. When docked, the Ship consume the Star's energy rapidly
	/// </summary>
	public class StarStateLocked : StarState
	{
		public StarStateLocked(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)StarStateType.Locked;
		}

		private Vector2 m_savedVelocity;

		protected override void DefaultEnter()
		{
			base.DefaultEnter();
			m_savedVelocity = m_this.rigidbody.velocity;
			m_this.rigidbody.velocity = Vector2.zero;

			m_this.mouseClickAction.action.performed += CallbackMouseClick;
		}

		public override void Update()
		{
			base.Update();
		}

		public override void Exit()
		{
			base.Exit();
			m_this.rigidbody.velocity = m_savedVelocity;
			m_this.mouseClickAction.action.performed -= CallbackMouseClick;
		}

		private void CallbackMouseClick(InputAction.CallbackContext context)
		{
			ChangeState((int)StarStateType.Returning);
		}
	}
}