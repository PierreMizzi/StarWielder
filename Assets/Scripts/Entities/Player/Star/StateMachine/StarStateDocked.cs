namespace PierreMizzi.Gameplay.Players
{
	using PierreMizzi.Useful.StateMachines;
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
			m_this.currentCombo = 1;
			m_this.playerChannel.onRefreshStarCombo.Invoke(m_this.currentCombo);
		}

		private void CallbackMouseClick(InputAction.CallbackContext context)
		{
			SetFree();
		}

		private void SetFree()
		{
			ChangeState((int)StarStateType.Free);
		}

	}
}