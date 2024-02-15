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

	public delegate void FightStageDelegate(FightStageData data);

	public class FightStageState : StageState
	{

		public FightStageState(IStateMachine stateMachine) : base(stateMachine)
		{
			type = (int)StageStateType.Fight;
			m_manager = m_this.GetStageManager<FightStageManager>();

			m_manager.onStageEnded += m_this.CallbackStageEnded;
			// Debug.Log(type + " : " + m_manager != null);
		}

		#region Behaviour

		private new FightStageManager m_manager;

		private int fightStageIndex = 0;

		protected override void DefaultEnter()
		{
			base.DefaultEnter();
			FightStageData data = (FightStageData)m_this.fightStageSettings.datas[fightStageIndex].Clone();
			m_manager.StartStage(data);

			fightStageIndex++;
		}

		#endregion
	}
}