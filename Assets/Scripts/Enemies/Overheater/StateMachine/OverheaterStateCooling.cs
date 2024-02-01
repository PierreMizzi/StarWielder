using UnityEngine;
using PierreMizzi.Useful.StateMachines;

namespace StarWielder.Gameplay.Enemies
{

	public class OverheaterStateCooling : OverheaterState
	{
		public OverheaterStateCooling(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)OverheaterStateType.Cooling;
		}

		public override void Update()
		{
			base.Update();

			m_this.currentEnergy -= m_this.energyCoolingSpeed * Time.deltaTime;

			if (m_this.currentEnergy <= 0)
			{
				ChangeState((int)OverheaterStateType.Idle);
			}
		}

	}
}