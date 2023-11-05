using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace PierreMizzi.Gameplay.Players
{
	public class ShipStateEmergencyPower : ShipState
	{
		public ShipStateEmergencyPower(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)ShipStateType.EmergencyPower;
		}

		protected override void DefaultEnter()
		{
			base.DefaultEnter();
			Debug.Log("ShipStateEmergencyPower");
		}

		public override void Update()
		{
			base.Update();

			m_this.DepleateEmergencyEnergy();

			if (m_this.emergencyEnergy <= 0)
				ChangeState((int)ShipStateType.NoPower);

			else if (m_this.star.isOnShip)
				ChangeState((int)ShipStateType.StarPower);

		}

	}
}