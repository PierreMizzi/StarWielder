using PierreMizzi.SoundManager;
using PierreMizzi.Useful.StateMachines;
using UnityEngine.InputSystem;

namespace QGamesTest.Gameplay.Player
{
	/// <summary>
	/// Star's state when it's moving, bouncing and absorbing EnemyStar's energy
	/// </summary>
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
			m_this.SetFree();
			m_this.SetVelocityFromEnergy();
			SoundManager.PlaySFX(SoundDataID.STAR_FREE);
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
			m_this.UpdateRotationFromVelocity();
		}

		private void CallbackMouseClick(InputAction.CallbackContext context)
		{
			ChangeState((int)StarStateType.Returning);
		}


	}
}