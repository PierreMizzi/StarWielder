using PierreMizzi.SoundManager;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;
using UnityEngine.InputSystem;


namespace StarWielder.Gameplay.Player
{
	/// <summary>
	/// Star's state when it's docked on the Ship. When docked, the Ship consume the Star's energy rapidly
	/// </summary>
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
			SoundManager.PlaySFX(SoundDataID.STAR_DOCKING);
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
			m_this.currentEnergy -= m_this.settings.dockedEnergyDepleateSpeed * Time.deltaTime;

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
			ChangeState((int)StarStateType.Free);
		}

	}
}