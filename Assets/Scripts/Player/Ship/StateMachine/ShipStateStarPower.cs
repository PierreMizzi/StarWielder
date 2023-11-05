using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace PierreMizzi.Gameplay.Players
{
	public class ShipStateStarPower : ShipState
	{
		public ShipStateStarPower(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)ShipStateType.StarPower;
		}

		protected override void DefaultEnter()
		{
			base.DefaultEnter();
			Debug.Log("ShipStateStarPower");
			m_this.controller.enabled = true;
			m_this.animator.SetBool(Ship.k_triggerHasEnergy, true);
		}

		public override void Update()
		{
			base.Update();

			if (!m_this.star.isOnShip)
				ChangeState((int)ShipStateType.EmergencyPower);

		}
	}
}