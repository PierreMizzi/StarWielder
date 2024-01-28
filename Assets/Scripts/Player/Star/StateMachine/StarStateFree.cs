using PierreMizzi.SoundManager;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarWielder.Gameplay.Player
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

		private float m_delayBeforeGrabbable = 0.25f;
		private float m_timeBeforeGrabbale = 0;
		private bool m_isGrabbable;

		protected override void DefaultEnter()
		{
			base.DefaultEnter();
			m_this.SetFree();
			m_this.SetVelocityFromEnergy();
			SoundManager.PlaySFX(SoundDataID.STAR_FREE);
			m_this.mouseClickAction.action.performed += CallbackMouseClick;

			m_timeBeforeGrabbale = 0;
			m_isGrabbable = false;
		}

		public override void Exit()
		{
			base.Exit();
			m_this.mouseClickAction.action.performed -= CallbackMouseClick;
			m_this.onTriggerEnter2D -= CallbackTriggerEnter;

			m_timeBeforeGrabbale = 0;
			m_isGrabbable = false;
		}

		public override void Update()
		{
			base.Update();
			m_this.UpdateRotationFromVelocity();


			if (m_timeBeforeGrabbale > m_delayBeforeGrabbable && !m_isGrabbable)
			{
				m_isGrabbable = true;
				m_this.onTriggerEnter2D += CallbackTriggerEnter;
			}
			else
				m_timeBeforeGrabbale += Time.deltaTime;

		}

		private void CallbackMouseClick(InputAction.CallbackContext context)
		{
			ChangeState((int)StarStateType.Returning);
		}

		private void CallbackTriggerEnter(Collider2D other)
		{
			if (other.gameObject == m_this.ship.gameObject)
				ChangeState((int)StarStateType.Transfer);
		}


	}
}