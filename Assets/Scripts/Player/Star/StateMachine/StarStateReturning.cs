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
			SoundManager.SoundManager.PlaySFX(SoundDataID.STAR_RETURNING);
			ReturnToShip();
			m_this.onTriggerEnter2D += CallbackTriggerEnter2D;
			m_this.ResetSquish();
		}

		public override void Exit()
		{
			base.Exit();
			KillReturning();
			m_this.onTriggerEnter2D -= CallbackTriggerEnter2D;

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

		private Vector3 m_previousPosition;

		private int m_previousPositionFrequency = 10;
		private int m_previousPositionFrame;


		public override void Update()
		{
			base.Update();

			m_previousPositionFrame++;
			if (m_previousPositionFrame > m_previousPositionFrequency)
			{
				m_previousPositionFrame = 0;
				m_previousPosition = m_this.transform.position;
			}
		}

		private void CallbackTriggerEnter2D(GameObject GameObject)
		{
			Debug.DrawLine(m_previousPosition, m_this.transform.position, Color.red, 10);
			KillReturning();

			m_this.transform.up *= -1;
			ChangeState((int)StarStateType.Free);
		}

	}
}