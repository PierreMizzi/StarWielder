using UnityEngine;
using PierreMizzi.Useful.StateMachines;

namespace StarWielder.Gameplay.Enemies
{

	public class OverheaterStateActive : OverheaterState
	{
		public OverheaterStateActive(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)OverheaterStateType.Active;
		}

        protected override void DefaultEnter()
        {
            base.DefaultEnter();
			m_this.SetHeatProgress(0);
		}

		public override void Update()
        {
            base.Update();
			m_this.transform.rotation *= Quaternion.Euler(Vector3.forward * m_this.rotationSpeed * Time.deltaTime);
		}

	}
}