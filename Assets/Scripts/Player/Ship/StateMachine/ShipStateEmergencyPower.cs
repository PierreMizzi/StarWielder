using PierreMizzi.Useful.StateMachines;

namespace StarWielder.Gameplay.Player
{
	/// <summary>
	/// Ship's state when the Star is no longer docked and is consuming it's emergency power
	/// </summary>
	public class ShipStateEmergencyPower : ShipState
	{
		public ShipStateEmergencyPower(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)ShipStateType.EmergencyPower;
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