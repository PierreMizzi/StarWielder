using UnityEngine;
using PierreMizzi.Useful.StateMachines;
using UnityEngine.InputSystem;
using StarWielder.Gameplay.Player;
using System;

namespace StarWielder.Gameplay.Enemies
{

	public class OverheaterStateOverheating : OverheaterState
	{
		public OverheaterStateOverheating(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)OverheaterStateType.Overheating;
		}

		protected override void DefaultEnter()
		{
			base.DefaultEnter();

			m_this.SetHasStar(true);

			// m_this.star.ChangeState(StarStateType.Locked);
			// m_this.star.transform.SetParent(m_this.Core.transform);
			// m_this.star.transform.localPosition = Vector3.zero;

			// m_this.star.mouseClickAction.action.performed += CallbackMouseClickAction;

			if (m_this.SunSocket != null)
			{
				m_this.SunSocket.onUnsocket += CallbackUnsocket;
			}
		}



        public override void Update()
		{
			base.Update();

			// Star
			m_this.sun.currentEnergy -= m_this.energyDrainSpeed * Time.deltaTime;

			// Overheater
			m_this.currentEnergy += m_this.energyDrainSpeed * Time.deltaTime;

			m_this.Shake(m_this.currentEnergyNormalized);
			m_this.SetHeatProgress(m_this.currentEnergyNormalized);

			if (m_this.currentEnergy >= m_this.maxEnergy)
			{
				m_this.sun.ChangeState(StarStateType.Free);
				m_this.Kill();
			}
		}

		public override void Exit()
		{
			base.Exit();
			if (m_this.SunSocket != null)
			{
				m_this.SunSocket.onUnsocket -= CallbackUnsocket;
			}
		}

		private void CallbackUnsocket(Star sun)
		{
			m_this.sun.transform.SetParent(null);
			m_this.sun = null;
			m_this.SetHasStar(false);

			ChangeState((int)OverheaterStateType.Cooling);
		}

	}
}