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

        public override void Update()
        {
            base.Update();
			m_this.transform.rotation *= Quaternion.Euler(Vector3.forward * m_this.rotationSpeed * Time.deltaTime);
		}

	}
}