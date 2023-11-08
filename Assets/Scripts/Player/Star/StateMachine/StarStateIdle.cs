using PierreMizzi.Useful.StateMachines;

namespace QGamesTest.Gameplay.Player
{
	/// <summary>
	/// Star's state when the game is over. No more bouncing
	/// </summary>
	public class StarStateIdle : StarState
	{

		public StarStateIdle(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)StarStateType.Idle;
		}


	}
}