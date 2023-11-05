using DG.Tweening;
using PierreMizzi.Useful;
using UnityEngine;

namespace PierreMizzi.Gameplay
{

	public class CameraManager : MonoBehaviour
	{
		#region Base

		[Header("Base")]
		[SerializeField] private Camera m_camera = null;
		[SerializeField] private float m_transferTransitionDuration = 0.5f;

		[SerializeField] private CameraChannel m_cameraChannel = null;

		private void ResetCameraPosition()
		{
			m_camera.transform.localPosition = Vector3.zero;
		}

		#endregion

		#region MonoBehaviour

		private void Awake()
		{
			m_currentTarget = m_defaultTarget;
		}

		private void Start()
		{
			if (m_cameraChannel != null)
			{
				m_cameraChannel.onStartEnergyTransfer += CallbackStartEnergyTransfer;
				m_cameraChannel.onStopEnergyTransfer += CallbackStopEnergyTransfer;

				m_cameraChannel.onShipHurt += CallbackShipHurt;
			}
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.I))
				CallbackStartEnergyTransfer();
			else if (Input.GetKeyDown(KeyCode.O))
				CallbackStopEnergyTransfer();
		}

		private void LateUpdate()
		{
			if (m_currentTarget != null)
				ManagePosition();
		}

		private void OnDestroy()
		{
			if (m_cameraChannel != null)
			{
				m_cameraChannel.onStartEnergyTransfer -= CallbackStartEnergyTransfer;
				m_cameraChannel.onStopEnergyTransfer -= CallbackStopEnergyTransfer;

				m_cameraChannel.onShipHurt -= CallbackShipHurt;
			}
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
		[SerializeField] private ShakeTweenSettings m_energyTransferShakeSettings;

		private Tween m_shakeTween;

		private void StartShaking()
		{
			m_shakeTween = m_camera.DOShakePosition(
				m_energyTransferShakeSettings.duration,
				m_energyTransferShakeSettings.strength,
				m_energyTransferShakeSettings.vibrato,
				m_energyTransferShakeSettings.randomness,
				false)
			.SetLoops(-1);
		}

		private void StopShaking()
		{
			if (m_shakeTween != null && m_shakeTween.IsPlaying())
				m_shakeTween.Kill();

			ResetCameraPosition();
		}

		#endregion

		#region Energy Transfer Shaking

		#endregion

		#region Ship Hurt Shaking

		[SerializeField] private ShakeTweenSettings m_shipHurtShakeSettings;

		private void CallbackShipHurt()
		{
			m_camera.DOShakePosition(
				m_shipHurtShakeSettings.duration,
				m_shipHurtShakeSettings.strength,
				m_shipHurtShakeSettings.vibrato,
				m_shipHurtShakeSettings.randomness,
				false)
			.OnComplete(ResetCameraPosition);
		}

		#endregion

		#region Debug

		[ContextMenu("Set Transfer")]
		public void CallbackStartEnergyTransfer()
		{
			// m_currentTarget = m_shipTarget;
			ZoomInShip();
			StartShaking();
		}

		[ContextMenu("Default")]
		public void CallbackStopEnergyTransfer()
		{
			// m_currentTarget = m_defaultTarget;
			ZoomOutShip();
			StopShaking();
		}

		#endregion



	}
}