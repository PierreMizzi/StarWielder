namespace PierreMizzi.Gameplay.Players
{
	using System;
	using PierreMizzi.Useful.StateMachines;
	using UnityEngine;
	using UnityEngine.InputSystem;

	public class StarStateDocked : StarState
	{
		public StarStateDocked(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)StarStateType.Docked;
		}

		protected override void DefaultEnter()
		{
			base.DefaultEnter();
			DockStar();

			m_this.mouseClickAction.action.performed += CallbackMouseClick;
		}

		public override void Exit()
		{
			base.Exit();
			m_this.mouseClickAction.action.performed -= CallbackMouseClick;
		}

		private void DockStar()
		{
			m_this.transform.SetParent(m_this.ship.starAnchor);
			m_this.transform.localPosition = Vector2.zero;
			m_this.transform.localRotation = Quaternion.identity;

			m_this.isDocked = true;
			m_this.ResetSquish();
			m_this.playerChannel.onStarDocked?.Invoke();
		}

		private void CallbackMouseClick(InputAction.CallbackContext context)
		{
			Release();
		}

		private void Release()
		{
			ChangeState((int)StarStateType.Free);
		}

	}
}