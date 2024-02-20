using PierreMizzi.Useful.StateMachines;

namespace StarWielder.Gameplay.Player
{
	/// <summary>
	/// Ship's state when the Star is docked and powering the Ship
	/// </summary>
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
			m_this.controller.enabled = true;
			m_this.animator.SetBool(Ship.k_boolHasEnergy, true);
			m_this.gameChannel.onComboBreak.Invoke();
		}

		public override void Update()
		{
			base.Update();

			if (!m_this.star.isOnShip)
				ChangeState((int)ShipStateType.EmergencyPower);

		}
	}
}