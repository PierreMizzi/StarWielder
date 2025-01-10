using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace StarWielder.Gameplay
{

	public class ShopStageState : StageState
	{
		public ShopStageState(IStateMachine stateMachine) : base(stateMachine)
		{
			type = (int)StageStateType.Shop;
			m_manager = m_this.GetStageManager<ShopStageManager>();
			m_manager.onStageEnded += m_this.CallbackStageEnded;
		}

		private new ShopStageManager m_manager;

		public override void Enter(StageSettings settings)
		{
			base.Enter(settings);
			m_manager.StartStage();
		}

		// 🟥 : Implement clear ShopStage and Manager for debug purposes
		public override void Clear()
		{
			base.Clear();
		}

	}
}