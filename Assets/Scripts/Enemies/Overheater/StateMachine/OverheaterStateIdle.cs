using UnityEngine;
using PierreMizzi.Useful.StateMachines;

namespace StarWielder.Gameplay.Enemies
{

	public class OverheaterStateIdle : OverheaterState
	{
		public OverheaterStateIdle(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)OverheaterStateType.Idle;
		}

	}
}