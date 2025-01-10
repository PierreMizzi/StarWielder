using UnityEngine;
using PierreMizzi.Useful.StateMachines;
using StarWielder.Gameplay.Player;

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
			if (m_this.SunSocket != null)
			{
				m_this.SunSocket.onSocket += CallbackSocket;
			}
		}

		public override void Update()
		{
			base.Update();
			m_this.transform.rotation *= Quaternion.Euler(Vector3.forward * m_this.rotationSpeed * Time.deltaTime);
		}

		public override void Exit()
		{
			base.Exit();

			if (m_this.SunSocket != null)
			{
				m_this.SunSocket.onSocket -= CallbackSocket;
			}
		}

		private void CallbackSocket(Star sun)
		{
			m_this.sun = sun;
			ChangeState((int)OverheaterStateType.Overheating);
		}



	}
}