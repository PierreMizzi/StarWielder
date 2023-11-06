namespace PierreMizzi.Gameplay.Players
{
	using System;
	using System.Collections.Generic;
	using DG.Tweening;
	using PierreMizzi.SoundManager;
	using PierreMizzi.Useful;
	using PierreMizzi.Useful.StateMachines;
	using UnityEngine;
	using UnityEngine.InputSystem;

	public class StarStateFree : StarState
	{

		public StarStateFree(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)StarStateType.Free;

			m_bounceSoundIDS = new List<string>()
			{
				SoundDataID.STAR_BOUNCE_01,
				SoundDataID.STAR_BOUNCE_02,
			};
		}

		private List<string> m_bounceSoundIDS = new List<string>();

		protected override void DefaultEnter()
		{
			base.DefaultEnter();
			Move();
			m_this.SetFree();
			SoundManager.PlaySFX(SoundDataID.STAR_FREE);
			m_this.mouseClickAction.action.performed += CallbackMouseClick;
		}

		public override void Exit()
		{
			base.Exit();
			Stop();
			m_this.mouseClickAction.action.performed -= CallbackMouseClick;
		}

		public override void Update()
		{
			base.Update();
			m_this.ManageSquish();
		}

		private void CallbackMouseClick(InputAction.CallbackContext context)
		{
			ChangeState((int)StarStateType.Returning);
		}

		#region Movement

		private Tween m_moveTween = null;

		private Vector3 m_directionMoveCompleted;

		private void Move()
		{
			List<RaycastHit2D> hits = new List<RaycastHit2D>();

			if (Physics2D.Raycast(m_this.transform.position, m_this.transform.up, m_this.obstacleFilter, hits, 100f) > 0)
			{
				RaycastHit2D hit = hits[0];

				Vector3 endPosition = hit.point;
				endPosition -= m_this.transform.up * m_this.circleCollider.radius;
				Debug.DrawLine(m_this.transform.position, hit.point, Color.blue, 3);

				float duration = hit.distance / m_this.currentSpeed;

				m_moveTween = m_this.transform.DOMove(endPosition, duration)
											  .SetEase(Ease.Linear)
											  .OnComplete(MoveCompleted);

				m_directionMoveCompleted = Vector2.Reflect(m_this.transform.up, hit.normal);
			}
		}

		private void MoveCompleted()
		{
			m_this.transform.up = m_directionMoveCompleted;
			Move();
			SoundManager.PlaySFX(UtilsClass.PickRandomInList(m_bounceSoundIDS));
		}

		private void Stop()
		{
			if (m_moveTween != null && m_moveTween.IsPlaying())
				m_moveTween.Kill();
		}

		#endregion

	}
}