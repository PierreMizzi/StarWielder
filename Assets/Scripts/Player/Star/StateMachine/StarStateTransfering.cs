namespace PierreMizzi.Gameplay.Players
{
	using PierreMizzi.Useful.StateMachines;
	using DG.Tweening;
	using UnityEngine;
	using System;
	using UnityEngine.InputSystem;
	using PierreMizzi.SoundManager;

	public class StarStateTransfering : StarState
	{
		public StarStateTransfering(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)StarStateType.Transfer;
		}

		private Tween m_transferTween;

		private SoundSource m_transferSoundSource;

		protected override void DefaultEnter()
		{
			base.DefaultEnter();

			m_this.ResetSquish();
			m_this.SetOnShip();
			m_this.cameraChannel.onStartEnergyTransfer.Invoke();
			m_transferSoundSource = SoundManager.PlaySFX(SoundDataID.SHIP_ENERGY_TRANSFER);

			TransferEnergy();

			m_this.mouseClickAction.action.performed += CallbackMouseClick;
		}

		public override void Exit()
		{
			base.Exit();

			m_this.mouseClickAction.action.performed -= CallbackMouseClick;
			m_this.cameraChannel.onStopEnergyTransfer.Invoke();
			m_transferSoundSource.FadeOut(0.1f);

			KillTransfer();
		}

		private void TransferEnergy()
		{
			float transferedEnergy = m_this.ship.GetMaxTransferableEnergy(m_this.currentEnergy);
			float transferDuration = GetTransferDuration();

			float fromShipEnergy = m_this.ship.emergencyEnergy;
			float toShipEnergy = fromShipEnergy + transferedEnergy;

			float fromStarEnergy = m_this.currentEnergy;
			float toStarEnergy = fromStarEnergy - transferedEnergy;

			m_transferTween = DOVirtual
			.Float(
				0f,
				1f,
				transferDuration,
				(float value) =>
				{
					m_this.currentEnergy = Mathf.Lerp(fromStarEnergy, toStarEnergy, value);
					m_this.playerChannel.onRefreshStarEnergy.Invoke(m_this.currentEnergy);

					m_this.ship.emergencyEnergy = Mathf.Lerp(fromShipEnergy, toShipEnergy, value);
				}
			)
			.SetEase(Ease.Linear)
			.OnComplete(CallbackTransferCompleted);


		}

		private float GetTransferDuration()
		{
			float duration;
			duration = m_this.settings.transferBaseDuration;
			duration += m_this.currentEnergy / m_this.settings.baseEnergy * m_this.settings.transferDurationRatio;
			return duration;
		}

		private void CallbackTransferCompleted()
		{
			ChangeState((int)StarStateType.Docked);
		}

		private void CallbackMouseClick(InputAction.CallbackContext context)
		{
			KillTransfer();

			ChangeState((int)StarStateType.Free);
		}

		private void KillTransfer()
		{
			if (m_transferTween != null && m_transferTween.IsPlaying())
				m_transferTween.Kill();
		}

	}

}