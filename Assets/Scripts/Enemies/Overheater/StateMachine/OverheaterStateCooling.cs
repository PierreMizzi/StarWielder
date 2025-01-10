using UnityEngine;
using PierreMizzi.Useful.StateMachines;
using StarWielder.Gameplay.Player;
using System;

namespace StarWielder.Gameplay.Enemies
{

	public class OverheaterStateCooling : OverheaterState
	{
		public OverheaterStateCooling(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)OverheaterStateType.Cooling;
		}

		protected override void DefaultEnter()
		{
			base.DefaultEnter();

			if (m_this.SunSocket != null)
			{
				m_this.SunSocket.onSocket += CallbackSocket;
			}
		}

		public override void Update()
		{
			base.Update();

			m_this.transform.rotation *= Quaternion.Euler(Vector3.forward * m_this.rotationSpeed * Time.deltaTime);

			m_this.currentEnergy -= m_this.energyCoolingSpeed * Time.deltaTime;

			m_this.SetHeatProgress(m_this.currentEnergyNormalized);

			if (m_this.currentEnergy <= 0)
			{
				ChangeState((int)OverheaterStateType.Idle);
			}
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