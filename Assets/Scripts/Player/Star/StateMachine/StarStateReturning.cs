namespace PierreMizzi.Gameplay.Players
{
	using System;
	using PierreMizzi.Useful.StateMachines;
	using DG.Tweening;
	using UnityEngine;

	public class StarStateReturning : StarState
	{
		public StarStateReturning(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)StarStateType.Returning;
		}

		private Tween m_returningTween;

		protected override void DefaultEnter()
		{
			base.DefaultEnter();
			ReturnToShip();
		}

		public override void Exit()
		{
			base.Exit();
			KillReturning();
		}

		private void ReturnToShip()
		{
			float duration = 1;

			Vector3 fromPosition = m_this.transform.position;

			m_returningTween = DOVirtual
			.Float(
				0f,
				1f,
				duration,
				(float value) =>
				{
					m_this.transform.position = Vector3.LerpUnclamped(fromPosition, m_this.ship.starAnchor.position, value);
				}
			)
			.SetEase(Ease.InBack)
			.OnComplete(CallbackReturnToShip);
		}

		private void CallbackReturnToShip()
		{
			ChangeState((int)StarStateType.Transfer);
		}

		private void KillReturning()
		{
			if (m_returningTween != null && m_returningTween.IsPlaying())
				m_returningTween.Kill();
		}

	}
}