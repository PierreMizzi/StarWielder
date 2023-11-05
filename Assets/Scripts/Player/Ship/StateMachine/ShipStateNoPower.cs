using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace PierreMizzi.Gameplay.Players
{
	public class ShipStateNoPower : ShipState
	{
		public ShipStateNoPower(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)ShipStateType.NoPower;
		}

		protected override void DefaultEnter()
		{
			base.DefaultEnter();
			Debug.Log("ShipStateNoPower");
			SoundManager.SoundManager.PlaySFX(SoundDataID.SHIP_POWER_DOWN);
			m_this.controller.enabled = false;
			m_this.animator.SetBool(Ship.k_triggerHasEnergy, false);
		}

		public override void Update()
		{
			base.Update();

			if (m_this.star.isOnShip)
				ChangeState((int)ShipStateType.StarPower);

		}
	}
}