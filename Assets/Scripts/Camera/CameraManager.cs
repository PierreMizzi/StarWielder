using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PierreMizzi.Gameplay
{

	public class CameraManager : MonoBehaviour
	{
		#region Base

		[Header("Base")]
		[SerializeField] private Camera m_camera = null;
		[SerializeField] private float m_transferTransitionDuration = 0.5f;


		#endregion

		#region MonoBehaviour

		private void Awake()
		{
			m_currentTarget = m_defaultTarget;
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.I))
				SetTransfer();
			else if (Input.GetKeyDown(KeyCode.O))
				SetDefault();
		}

		private void LateUpdate()
		{
			if (m_currentTarget != null)
				ManagePosition();
		}

		#endregion

		#region Position

		[Header("Position")]
		[SerializeField] private Transform m_defaultTarget = null;
		[SerializeField] private Transform m_shipTarget = null;
		[SerializeField] private float m_trackingSpeed = 1.0f;

		private Transform m_currentTarget;
		private Vector3 m_nextPosition;
		private Vector3 m_currentVelocity;

		private void ManagePosition()
		{
			m_nextPosition = m_currentTarget.position;
			m_nextPosition.z = transform.position.z;
			transform.position = Vector3.SmoothDamp(transform.position, m_nextPosition, ref m_currentVelocity, m_trackingSpeed);
		}


		#endregion

		#region Zoom

		[Header("Zoom")]
		[SerializeField] private float m_defaultZoom = 5;
		[SerializeField] private float m_shipZoom = 4;


		private Tween m_zoomTween;

		private void ZoomInShip()
		{
			m_zoomTween = m_camera.DOOrthoSize(m_shipZoom, m_transferTransitionDuration);
		}

		private void ZoomOutShip()
		{
			m_zoomTween = m_camera.DOOrthoSize(m_defaultZoom, m_transferTransitionDuration);
		}

		#endregion

		#region Shaking

		[Header("Shaking")]
		[SerializeField] private float m_shakeDuration = 1f;
		[SerializeField] private float m_shakeStrength = 1f;
		[SerializeField] private int m_shakeVibrato = 10;
		[SerializeField] private float m_shakeRandomness = 90f;

		private Tween m_shakeTween;

		private void StartShaking()
		{
			m_shakeTween = m_camera.DOShakePosition(m_shakeDuration, m_shakeStrength, m_shakeVibrato, m_shakeRandomness, false).SetLoops(-1);
		}

		private void StopShaking()
		{
			if (m_shakeTween != null && m_shakeTween.IsPlaying())
				m_shakeTween.Kill();

			m_camera.transform.localPosition = Vector3.zero;
		}


		#endregion

		#region Debug

		[ContextMenu("Set Transfer")]
		public void SetTransfer()
		{
			// m_currentTarget = m_shipTarget;
			ZoomInShip();
			StartShaking();
		}

		[ContextMenu("Default")]
		public void SetDefault()
		{
			// m_currentTarget = m_defaultTarget;
			ZoomOutShip();
			StopShaking();
		}

		#endregion



	}
}