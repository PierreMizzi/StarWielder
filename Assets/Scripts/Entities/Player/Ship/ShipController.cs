using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PierreMizzi.Gameplay.Players
{

	public class ShipController : MonoBehaviour
	{

		#region Fields

		[SerializeField]
		private PlayerSettings m_settings = null;

		private Camera m_camera;


		[SerializeField]
		private InputActionReference m_locomotionActionReference = null;

		[SerializeField]
		private InputActionReference m_mousePositionActionReference = null;

		#region Locomotion

		private Vector3 m_locomotionActionValue;
		private Vector3 m_offsetPosition;
		private Vector3 m_nextPosition;
		private Vector3 m_immediatePosition;
		private Vector3 m_currentVelocity;

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
			ReadLocomotionInputs();
			Move();
		}

		private void LateUpdate()
		{
			ReadMousePositionInputs();
			Rotate();
		}

		#endregion

		#region Locomotion

		private void Move()
		{
			m_offsetPosition = m_locomotionActionValue * m_settings.speed * Time.deltaTime;
			m_nextPosition = transform.position + m_offsetPosition;
			m_immediatePosition = transform.position + m_offsetPosition * m_settings.immediatePositionScale;

			if (true)
				transform.position = Vector3.SmoothDamp(transform.position, m_nextPosition, ref m_currentVelocity, m_settings.smoothTime);
		}

		private void ReadLocomotionInputs()
		{
			m_locomotionActionValue = m_locomotionActionReference.action.ReadValue<Vector2>();
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

	}
}