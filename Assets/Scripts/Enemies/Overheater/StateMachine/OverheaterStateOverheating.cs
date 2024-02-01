using UnityEngine;
using PierreMizzi.Useful.StateMachines;
using UnityEngine.InputSystem;

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
			// TODO Update visual

			Debug.Log("Normalized : " + m_this.currentEnergyNormalized);

			if (m_this.currentEnergy >= m_this.maxEnergy)
				Object.Destroy(m_this.gameObject);

		}

		private void CallbackMouseClickAction(InputAction.CallbackContext context)
		{
			m_this.star.mouseClickAction.action.performed -= CallbackMouseClickAction;
			m_this.UnlockStar();

			ChangeState((int)OverheaterStateType.Cooling);
		}
	}
}