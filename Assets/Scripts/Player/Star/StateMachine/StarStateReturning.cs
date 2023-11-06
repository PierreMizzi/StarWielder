namespace PierreMizzi.Gameplay.Players
{
	using System;
	using PierreMizzi.Useful.StateMachines;
	using DG.Tweening;
	using UnityEngine;
	using System.Collections.Generic;

	public class StarStateReturning : StarState
	{
		public StarStateReturning(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)StarStateType.Returning;
		}


		protected override void DefaultEnter()
		{
			base.DefaultEnter();
			SoundManager.SoundManager.PlaySFX(SoundDataID.STAR_RETURNING);
			m_this.onCollisionEnter += CallbackCollisionEnter;
		}

		private bool m_hasCollided;

		private void CallbackCollisionEnter(GameObject gameObject)
		{
			m_hasCollided = true;
			m_this.rigidbody.velocity = Vector2.zero;

			List<RaycastHit2D> hits = new List<RaycastHit2D>();

			if (Physics2D.Raycast(m_this.transform.position, m_this.transform.up, m_this.obstacleFilter, hits, 100f) > 0)
			{
				RaycastHit2D hit = hits[0];
				Vector3 reflectedDirection = Vector2.Reflect(m_this.transform.up, hit.normal);
				m_this.transform.up = reflectedDirection;
				ChangeState((int)StarStateType.Free);
			}

		}

		public override void Exit()
		{
			base.Exit();
			m_hasCollided = false;
			m_this.onCollisionEnter -= CallbackCollisionEnter;
		}

		public override void Update()
		{
			base.Update();

			if (m_hasCollided)
				return;

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
				ChangeState((int)StarStateType.Transfer);
			}
		}

	}
}