using PierreMizzi.Gameplay.Players;
using PierreMizzi.Useful.StateMachines;

public class StarStateIdle : StarState
{

	public StarStateIdle(IStateMachine stateMachine)
		: base(stateMachine)
	{
		type = (int)StarStateType.Idle;
	}


}