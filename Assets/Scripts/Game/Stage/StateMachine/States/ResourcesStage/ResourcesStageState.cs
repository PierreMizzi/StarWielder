using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace StarWielder.Gameplay
{

	public class ResourcesStageState : StageState
	{
		public ResourcesStageState(IStateMachine stateMachine) : base(stateMachine)
		{
			type = (int)StageStateType.Resources;
			m_manager = m_this.GetStageManager<ResourcesStageManager>();
			Debug.Log(type + " : " + m_manager != null);
		}
	}
}