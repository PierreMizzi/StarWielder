namespace PierreMizzi.Gameplay.Players
{
	using PierreMizzi.Useful.StateMachines;
	using DG.Tweening;
	using UnityEngine;
	using System;
	using UnityEngine.InputSystem;

	public class StarStateTransfering : StarState
	{
		public StarStateTransfering(IStateMachine stateMachine)
			: base(stateMachine)
		{
			type = (int)StarStateType.Transfer;
		}

		private Tween m_transferTween;

		protected override void DefaultEnter()
		{
			base.DefaultEnter();

			m_this.ResetSquish();

			m_this.SetOnShip();

			m_this.cameraChannel.onStartEnergyTransfer.Invoke();

			TransferEnergy();

			m_this.mouseClickAction.action.performed += CallbackMouseClick;
		}

		public override void Exit()
		{
			base.Exit();

			m_this.mouseClickAction.action.performed -= CallbackMouseClick;
			m_this.cameraChannel.onStopEnergyTransfer.Invoke();
			KillTransfer();
		}

		private void TransferEnergy()
		{
			float transferedEnergy = m_this.ship.GetMaxTransferableEnergy(m_this.currentEnergy);
			float transferDuration = GetTransferDuration();

			float fromShipEnergy = m_this.ship.currentEnergy;
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

					m_this.ship.currentEnergy = Mathf.Lerp(fromShipEnergy, toShipEnergy, value);
					m_this.playerChannel.onRefreshShipEnergy.Invoke(m_this.ship.currentEnergy);

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