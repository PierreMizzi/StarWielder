namespace PierreMizzi.Gameplay.Players
{
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

		public override void Update()
		{
			base.Update();
			m_this.currentEnergy -= m_this.settings.energyDepleatRate * Time.deltaTime;
			m_this.playerChannel.onRefreshStarEnergy.Invoke(m_this.currentEnergy);

			if (m_this.currentEnergy <= 0f)
				m_this.gameChannel.onGameOver.Invoke(GameOverReason.StarDied);

		}

		private void DockStar()
		{
			m_this.currentCombo = 1;
			m_this.playerChannel.onRefreshStarCombo.Invoke(m_this.currentCombo);

			// Starts the game if it's the first time, no effect otherwiser
			// Check callback in GameManager.cs
			m_this.gameChannel.onFirstDocking.Invoke();
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