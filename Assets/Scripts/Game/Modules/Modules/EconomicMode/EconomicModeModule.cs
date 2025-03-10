using System;
using System.Collections;
using StarWielder.Gameplay.Player;
using UnityEngine;

namespace StarWielder.Gameplay.Modules
{

	// Note : Should only enter after a given time
	// Note : Should only enter while in Fight Stage
	public class EconomicModeModule : BaseModule //MonoBehaviour, IModuleHandler
	{


		#region BaseModule

		public override void Enable()
		{
			base.Enable();
		}

		public override void Disable()
		{
			base.Disable();
		}

		#endregion

		#region Behaviour
		
		[Header("Behaviour")]
		[SerializeField] private Ship m_ship;
		[SerializeField] private GameChannel m_gameChannel;
		[SerializeField] private PlayerChannel m_playerChannel;

		public EconomicModeModuleSettings settings => m_settings as EconomicModeModuleSettings;

		#endregion

		#region Immobile Delay

		public IEnumerator m_immobileCoroutine;
		private float m_immobileTime;

		private void CallbackIsImmobile(bool value)
		{
			if (m_gameChannel.currentStagetype != StageStateType.Fight)
			{
				return;
			}

			if (value)
			{
				StartImmobileCoroutine();
			}
			else
			{
				m_ship.SetEnergyConsumptionMode(Ship.EnergyConsumptionMode.Fight);
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
			while (m_immobileTime < settings.ImmobileDelay)
			{
				m_immobileTime += Time.deltaTime;
				yield return null;
			}

			m_ship.SetEnergyConsumptionMode(Ship.EnergyConsumptionMode.Eco);
			StopImmobileCoroutine();
		}

		#endregion

		#region MonoBehaviour

		private void Start()
		{
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