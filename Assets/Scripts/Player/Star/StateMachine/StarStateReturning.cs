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
		}

		// public override void Exit()
		// {
		// 	base.Exit();
		// 	// KillReturning();
		// }

		public override void Update()
		{
			base.Update();
			m_this.UpdateRotationFromVelocity();

			Vector3 direction = m_this.ship.transform.position - m_this.transform.position;
			float distance = direction.magnitude;

			if (distance > m_this.settings.arrivalDistance)
			{
				// Calcule la vitesse souhaitée en fonction de l'accélération
				float desiredSpeed = Mathf.Sqrt(2f * m_this.settings.acceleration * distance);

				// Limite la vitesse à la vitesse maximale
				float finalSpeed = Mathf.Min(desiredSpeed, m_this.settings.maxSpeed);

				// Calcule la direction de déplacement
				Vector3 velocity = direction.normalized * finalSpeed;

				// Applique la vélocité à l'objet
				m_this.rigidbody.velocity = velocity;
			}
			else
			{
				// L'objet est arrivé à destination
				Debug.Log("Arrivé à destination!");
				ChangeState((int)StarStateType.Transfer);
			}
		}

		// private void ReturnToShip()
		// {
		// 	float duration = 1;

		// 	Vector3 fromPosition = m_this.transform.position;

		// 	m_returningTween = DOVirtual
		// 	.Float(
		// 		0f,
		// 		1f,
		// 		duration,
		// 		(float value) =>
		// 		{
		// 			m_this.transform.position = Vector3.LerpUnclamped(fromPosition, m_this.ship.starAnchor.position, value);
		// 		}
		// 	)
		// 	.SetEase(Ease.InBack)
		// 	.OnComplete(CallbackReturnToShip);
		// }

		// private void CallbackReturnToShip()
		// {
		// 	m_this.rigidbody.velocity = Vector2.zero;
		// 	ChangeState((int)StarStateType.Transfer);
		// }

		// private void KillReturning()
		// {
		// 	if (m_returningTween != null && m_returningTween.IsPlaying())
		// 		m_returningTween.Kill();
		// }








	}
}