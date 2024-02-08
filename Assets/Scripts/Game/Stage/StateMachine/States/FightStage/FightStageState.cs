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
		[SerializeField] private GameChannel m_gameChannel;

		public FightStageState(IStateMachine stateMachine) : base(stateMachine)
		{
			type = (int)StageStateType.Fight;
			m_manager = m_this.GetStageManager<FightStageManager>();
			Debug.Log(type + " : " + m_manager != null);

			if (m_this.gameChannel != null)
				m_this.gameChannel.onFightStageEnd += CallbackFightStageEnd;
		}

		#region Behaviour

		private int fightStageIndex = 0;

		protected override void DefaultEnter()
		{
			base.DefaultEnter();
			FightStageData data = (FightStageData)m_this.fightStageSettings.datas[fightStageIndex].Clone();
			m_this.gameChannel.onFightStageStart.Invoke(data);

			fightStageIndex++;
		}

		#endregion

		private void CallbackFightStageEnd()
		{
			Debug.Log("Stage ended !");
		}

	}
}