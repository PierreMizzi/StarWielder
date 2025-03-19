using System;
using System.Collections;
using StarWielder.Gameplay.Player;
using UnityEngine;

namespace StarWielder.Gameplay.Modules
{

	public class EconomicModeModule : BaseModule
	{

		#region Behaviour

		[Header("Behaviour")]
		[SerializeField] private GameChannel m_gameChannel;
		[SerializeField] private PlayerChannel m_playerChannel;

		public EconomicModeModuleSettings settings => m_settings as EconomicModeModuleSettings;

        public override void Disable()
        {
            base.Disable();
			m_playerChannel.onSetAppropriateEnergyConsumptionMode.Invoke();
		}

		#endregion

		#region Immobile Delay

		public IEnumerator m_immobileCoroutine;
		private float m_immobileTime;
		private float m_immobileProgress;

		private void CallbackIsImmobile(bool isImmobile)
		{
			if (m_isEnabled == false)
			{
				return;
			}

			if (m_gameChannel.currentStagetype != StageStateType.Fight)
			{
				return;
			}

			if (isImmobile)
			{
				StartImmobileCoroutine();
			}
			else
			{
				SetUnavailable();
				m_playerChannel.onSetAppropriateEnergyConsumptionMode.Invoke();
				StopImmobileCoroutine();
			}
		}

		private void StartImmobileCoroutine()
		{
			if (m_immobileCoroutine == null)
			{
				m_immobileCoroutine = Immobilebehaviour();
				StartCoroutine(m_immobileCoroutine);
			}
		}

		private void StopImmobileCoroutine()
		{
			if (m_immobileCoroutine != null)
			{
				StopCoroutine(m_immobileCoroutine);
				m_immobileCoroutine = null;
			}
		}

		private IEnumerator Immobilebehaviour()
		{
			m_immobileTime = 0;
			m_immobileProgress = 0;
			while (m_immobileTime < settings.ImmobileDelay)
			{
				m_immobileTime += Time.deltaTime;
				m_immobileProgress = m_immobileTime / settings.ImmobileDelay;
				m_moduleUI.SetFill(m_immobileProgress);
				yield return null;
			}
			SetAvailable();
			m_playerChannel.onSetEnergyConsumptionMode(Ship.EnergyConsumptionMode.Eco);
			StopImmobileCoroutine();
		}

		#endregion

		#region MonoBehaviour

		protected override void Start()
		{
			base.Start();

			if (m_playerChannel != null)
				m_playerChannel.onIsImmobile += CallbackIsImmobile;
		}

        private void OnDestroy()
		{
			if (m_playerChannel != null)
				m_playerChannel.onIsImmobile -= CallbackIsImmobile;
		}
			
		#endregion


	}

}