using PierreMizzi.Useful.StateMachines;
using UnityEngine;

/*
	- Pick 3 types of enemies based on difficulty
	- Gives them to the EnemyManager
	- Stage starts, spawning starts
	- When all enemies have been killed -> Stage completed
*/

namespace StarWielder.Gameplay
{
	public class FightStageState : StageState
	{
		[SerializeField] private GameChannel m_gameChannel;

		public FightStageState(IStateMachine stateMachine) : base(stateMachine)
		{
			type = (int)StageStateType.Fight;

			if (m_this.gameChannel != null)
				m_this.gameChannel.onFightStageEnd += CallbackFightStageEnd;
		}

		#region Behaviour

		protected override void DefaultEnter()
		{
			base.DefaultEnter();
			m_this.gameChannel.onFightStageStart.Invoke(1);
		}

		#endregion

		private void CallbackFightStageEnd()
		{
			Debug.Log("Stage ended !");
		}

	}
}