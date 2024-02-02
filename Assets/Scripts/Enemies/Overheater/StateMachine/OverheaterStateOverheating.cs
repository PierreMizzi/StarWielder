using UnityEngine;
using PierreMizzi.Useful.StateMachines;
using UnityEngine.InputSystem;
using StarWielder.Gameplay.Player;

namespace StarWielder.Gameplay.Enemies
{

	public class OverheaterStateOverheating : OverheaterState
	{
		public OverheaterStateOverheating(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)OverheaterStateType.Overheating;
		}

		protected override void DefaultEnter()
		{
			base.DefaultEnter();

			m_this.SetHasStar(true);

			m_this.star.transform.position = m_this.transform.position;
			m_this.star.mouseClickAction.action.performed += CallbackMouseClickAction;
		}

		public override void Update()
		{
			base.Update();

			// Star
			m_this.star.currentEnergy -= m_this.energyDrainSpeed * Time.deltaTime;

			// Overheater
			m_this.currentEnergy += m_this.energyDrainSpeed * Time.deltaTime;

			if (m_this.currentEnergy >= m_this.maxEnergy)
			{
				m_this.star.ChangeState(StarStateType.Free);
				m_this.Kill();
			}
		}

		public override void Exit()
		{
			base.Exit();
			if (m_this.star != null)
				m_this.star.mouseClickAction.action.performed -= CallbackMouseClickAction;
		}

		private void CallbackMouseClickAction(InputAction.CallbackContext context)
		{
			m_this.star.mouseClickAction.action.performed -= CallbackMouseClickAction;
			m_this.star = null;
			m_this.SetHasStar(false);

			ChangeState((int)OverheaterStateType.Cooling);
		}
	}
}