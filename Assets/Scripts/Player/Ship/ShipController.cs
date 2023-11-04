using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

namespace PierreMizzi.Gameplay.Players
{

	public class ShipController : MonoBehaviour
	{

		#region Fields

		[SerializeField] private PlayerSettings m_settings = null;
		[SerializeField] private PlayerChannel m_playerChannel = null;


		private Camera m_camera;


		[SerializeField]
		private InputActionReference m_locomotionActionReference = null;

		[SerializeField]
		private InputActionReference m_mousePositionActionReference = null;



		#region Locomotion

		// Locomotion
		private Vector3 m_locomotionActionValue;
		private Vector3 m_offsetPosition;
		private Vector3 m_nextPosition;
		private Vector3 m_currentVelocity;

		// Rotation
		private Vector2 m_mousePositionActionValue;
		private Vector2 m_screenSpacePosition;
		private Vector3 m_orientation;

		/// <summary> 
		/// Multiply the direction Ship-Mouse Cursor by this value to avoid weird jittering
		/// </summary>
		[SerializeField]
		private float m_orientationMagnitude = 2;

		#endregion

		#endregion

		#region Monobehaviour

		private void Start()
		{
			m_camera = Camera.main;
		}

		private void Update()
		{
			ReadMousePositionInputs();

			if (m_canDash && m_dashActionReference.action.IsPressed())
				Dash();

			if (!m_isDashing)
			{
				ReadLocomotionInputs();
				Move();
			}
		}

		private void LateUpdate()
		{
			Rotate();
		}

		#endregion

		#region Locomotion

		private void Move()
		{
			m_offsetPosition = m_locomotionActionValue * m_settings.speed * Time.deltaTime;
			m_nextPosition = transform.position + m_offsetPosition;

			if (true)
				transform.position = Vector3.SmoothDamp(transform.position, m_nextPosition, ref m_currentVelocity, m_settings.smoothTime);
		}

		private void ReadLocomotionInputs()
		{
			m_locomotionActionValue = m_locomotionActionReference.action.ReadValue<Vector2>().normalized;
		}

		private void Rotate()
		{
			m_screenSpacePosition = m_camera.WorldToScreenPoint(transform.position);

			m_orientation = (m_mousePositionActionValue - m_screenSpacePosition).normalized * m_orientationMagnitude;
			transform.right = m_orientation;
		}

		private void ReadMousePositionInputs()
		{
			m_mousePositionActionValue = m_mousePositionActionReference.action.ReadValue<Vector2>();
		}

		#endregion

		#region Dash

		[Header("Dash")]
		[SerializeField]
		private InputActionReference m_dashActionReference = null;

		private float m_dashCooldownTime;

		private bool m_isDashing = false;
		private bool m_canDash = true;

		private void Dash()
		{
			Vector3 dashDirection;
			if (m_locomotionActionValue == Vector3.zero)
				dashDirection = transform.up;
			else
				dashDirection = m_locomotionActionValue;
			Vector3 endPosition = transform.position + dashDirection * m_settings.dashDistance;

			m_isDashing = true;
			m_canDash = false;

			m_dashCooldownTime = 0f;

			transform.DOMove(endPosition, m_settings.dashDuration)
					 .SetEase(Ease.OutSine)
					 .OnComplete(OnCompleteDash);

			m_playerChannel.onUseDash.Invoke();
		}

		private void OnCompleteDash()
		{
			m_isDashing = false;
			StartCoroutine(DashCooldownIEnumerator());
		}

		private IEnumerator DashCooldownIEnumerator()
		{
			while (m_dashCooldownTime <= m_settings.dashCooldownDuration)
			{
				m_dashCooldownTime += Time.deltaTime;
				m_playerChannel.onRefreshCooldownDash.Invoke(m_dashCooldownTime / m_settings.dashCooldownDuration);
				yield return null;
			}

			m_playerChannel.onRefreshCooldownDash.Invoke(1f);
			m_playerChannel.onRechargeDash.Invoke();

			m_canDash = true;

		}

		#endregion

	}
}