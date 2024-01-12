using DG.Tweening;
using PierreMizzi.Useful;
using StarWielder.Gameplay.Player;
using UnityEngine;

namespace StarWielder.Gameplay
{

	public class CameraManager : MonoBehaviour
	{
		#region Main

		[Header("Main")]
		[SerializeField] private PlayerChannel m_playerChannel = null;
		[SerializeField] private Camera m_camera = null;
		[SerializeField] private float m_transferTransitionDuration = 0.5f;

		private void ResetCameraPosition()
		{
			m_camera.transform.localPosition = Vector3.zero;
		}

		#endregion

		#region MonoBehaviour

		private void Start()
		{
			if (m_playerChannel != null)
			{
				m_playerChannel.onStartEnergyTransfer += CallbackStartEnergyTransfer;
				m_playerChannel.onStopEnergyTransfer += CallbackStopEnergyTransfer;

				m_playerChannel.onShipHurt += CallbackShipHurt;
			}
		}

		private void OnDestroy()
		{
			if (m_playerChannel != null)
			{
				m_playerChannel.onStartEnergyTransfer -= CallbackStartEnergyTransfer;
				m_playerChannel.onStopEnergyTransfer -= CallbackStopEnergyTransfer;

				m_playerChannel.onShipHurt -= CallbackShipHurt;
			}
		}

		#endregion

		#region Energy Transfer

		[Header("Energy Transfer")]
		[SerializeField] private ShakeTweenSettings m_energyTransferShakeSettings;
		[SerializeField] private float m_defaultZoom = 5;
		[SerializeField] private float m_shipZoom = 4;

		private Tween m_shakeTween;

		public void CallbackStartEnergyTransfer()
		{
			m_camera.DOOrthoSize(m_shipZoom, m_transferTransitionDuration);

			m_shakeTween = m_camera.DOShakePosition(
				m_energyTransferShakeSettings.duration,
				m_energyTransferShakeSettings.strength,
				m_energyTransferShakeSettings.vibrato,
				m_energyTransferShakeSettings.randomness,
				false)
			.SetLoops(-1);
		}

		public void CallbackStopEnergyTransfer()
		{
			m_camera.DOOrthoSize(m_defaultZoom, m_transferTransitionDuration);

			if (m_shakeTween != null && m_shakeTween.IsPlaying())
				m_shakeTween.Kill();

			ResetCameraPosition();
		}

		#endregion

		#region Ship Hurt

		[Header("Ship Hurt")]
		[SerializeField] private ShakeTweenSettings m_shipHurtShakeSettings;

		private void CallbackShipHurt(float normalizedHealth)
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


	}
}