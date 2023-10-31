namespace PierreMizzi.Gameplay.Players
{
	using System;
	using PierreMizzi.Useful.StateMachines;
	using UnityEngine;
	using UnityEngine.InputSystem;

	public class StarStateFree : StarState
	{

		public StarStateFree(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)StarStateType.Free;
		}

		protected override void DefaultEnter()
		{
			base.DefaultEnter();
			Release();
			m_this.mouseClickAction.action.performed += CallbackMouseClick;
		}

		public override void Exit()
		{
			base.Exit();
			m_this.mouseClickAction.action.performed -= CallbackMouseClick;
		}

		public override void Update()
		{
			base.Update();
			Move();
			m_this.ManageSquish();
		}

		#region Movement

		private void Release()
		{
			m_this.transform.SetParent(null);
			m_this.isDocked = false;
		}

		private void Move()
		{
			m_this.transform.position += m_this.transform.up * m_this.currentSpeed * Time.deltaTime;
		}

		#endregion

		private void CallbackMouseClick(InputAction.CallbackContext context)
		{
			ChangeState((int)StarStateType.Returning);
		}




	}
}