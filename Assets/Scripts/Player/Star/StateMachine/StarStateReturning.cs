using PierreMizzi.Useful.StateMachines;
using UnityEngine;
using System.Collections.Generic;
using PierreMizzi.Useful;
using PierreMizzi.SoundManager;

namespace QGamesTest.Gameplay.Player
{
	/// <summary>
	/// Star's state when it's coming back the Ship.
	/// Returning is interupted if the Star collides with an EnemyShip !
	/// Next step is transfering it's energy (ShipStateTransfering)
	/// When returning, the transfer can start if the Star is close to the Ship OR
	/// if the Ship triggers with the Star
	/// </summary>
	public class StarStateReturning : StarState
	{
		public StarStateReturning(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)StarStateType.Returning;
		}

		private bool m_hasCollided;

		protected override void DefaultEnter()
		{
			base.DefaultEnter();
			SoundManager.PlaySFX(SoundDataID.STAR_RETURNING);
			m_this.onCollisionEnter2D += CallbackCollisionEnter;
			m_this.onTriggerEnter2D += CallbackTriggerEnter;
		}

		public override void Exit()
		{
			base.Exit();
			m_hasCollided = false;
			m_this.onCollisionEnter2D -= CallbackCollisionEnter;
			m_this.onTriggerEnter2D -= CallbackTriggerEnter;
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
				float desiredSpeed = Mathf.Sqrt(2f * m_this.settings.acceleration * distance);
				float finalSpeed = Mathf.Min(desiredSpeed, m_this.settings.maxSpeed);
				Vector3 velocity = direction.normalized * finalSpeed;

				m_this.rigidbody.velocity = velocity;
			}
			else
				ChangeState((int)StarStateType.Transfer);
		}

		private void CallbackTriggerEnter(Collider2D other)
		{
			if (other.gameObject == m_this.ship.gameObject)
				ChangeState((int)StarStateType.Docked);
		}

		private void CallbackCollisionEnter(Collision2D other)
		{
			if (UtilsClass.CheckLayer(m_this.obstacleFilter.layerMask.value, other.gameObject.layer))
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
		}

	}
}