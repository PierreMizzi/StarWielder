using DG.Tweening;
using PierreMizzi.Useful.StateMachines;
using UnityEngine;

namespace PierreMizzi.Gameplay.Players
{
	public class ShipStateDestroyed : ShipState
	{
		public ShipStateDestroyed(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)ShipStateType.Destroyed;
		}

		protected override void DefaultEnter()
		{
			base.DefaultEnter();

			m_this.controller.enabled = false;
			m_this.animator.SetBool(Ship.k_boolIsDead, true);

			DOVirtual.DelayedCall(1f, () => { m_this.gameChannel.onGameOver.Invoke(GameOverReason.ShipDestroyed); });

		}
	}
}