using PierreMizzi.SoundManager;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace QGamesTest.Gameplay.Player
{
	public class ShipStateNoPower : ShipState
	{
		/// <summary>
		/// Ship's state when he consumed all it's emergency power. Cannot move !
		/// </summary>
		public ShipStateNoPower(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)ShipStateType.NoPower;
		}

		protected override void DefaultEnter()
		{
			base.DefaultEnter();
			SoundManager.PlaySFX(SoundDataID.SHIP_POWER_DOWN);
			m_this.controller.enabled = false;
			m_this.animator.SetBool(Ship.k_boolHasEnergy, false);
		}

		public override void Update()
		{
			base.Update();

			if (m_this.star.isOnShip)
				ChangeState((int)ShipStateType.StarPower);

		}
	}
}