using System;
using PierreMizzi.Useful;
using PierreMizzi.Useful.PoolingObjects;
using StarWielder.Gameplay.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarWielder.Gameplay.Modules
{

	// 🟥 : HomingLazerModule : Add Homing Lazer when enabled
	public class HomingLazerModule : BaseModule
	{

		#region BaseModule

		public HomingLazerModuleSettings settings => m_settings as HomingLazerModuleSettings;

		public override void Enable()
		{
			base.Enable();

			if (m_inputAction != null)
				m_inputAction.action.performed += CallbackFireInput;

			if (m_playerChannel.onStarDocked != null)
				m_playerChannel.onStarDocked += CallbackStarDocked;
		}


		public override void Disable()
		{
			base.Disable();

			if (m_inputAction != null)
				m_inputAction.action.performed -= CallbackFireInput;

			if (m_playerChannel.onStarDocked != null)
				m_playerChannel.onStarDocked -= CallbackStarDocked;
		}

		#endregion

		#region Behaviour

		[Header("Behaviour")]
		[SerializeField] private PlayerChannel m_playerChannel;
		[SerializeField] private InputActionReference m_inputAction;
		[SerializeField] private Ship m_ship;
		[SerializeField] private Star m_sun;


		private bool hasFired = false;

		private void CallbackFireInput(InputAction.CallbackContext context)
		{
			if (SafetyChecks() && CanFire())
			{
				Fire();
			}
		}

		private void Fire()
		{
			GameObject lazerGameObject = m_poolingChannel.onGetFromPool?.Invoke(m_poolConfig.prefab.gameObject);

			if (lazerGameObject == null)
			{
				return;
			}

			if (lazerGameObject.TryGetComponent(out HomingLazer lazer))
			{
				lazer.Fire(this, m_sun.transform.position, m_ship.transform.position);
				hasFired = true;
			}
		}

		private bool SafetyChecks()
		{
			return m_poolingChannel != null &&
				   m_ship != null &&
				   m_sun != null;
		}

		private bool CanFire()
		{
			return hasFired == false &&
				   (m_sun.IsState(StarStateType.Free) || m_sun.IsState(StarStateType.Returning));
		}

		public void ConvertAbsorbedEnergyIntoHealth(float absorbedEnergy)
		{
			if (m_playerChannel == null || m_cameraChannel == null)
			{
				return;
			}

			m_playerChannel.onIncrementShipHealth?.Invoke(absorbedEnergy * settings.EnergyToHealthRatio);
			m_cameraChannel.onShakeCameraPosition?.Invoke(m_shakeTweenSettings);
		}

		private void CallbackStarDocked()
		{
			hasFired = false;
		}

		#endregion


		[Header("Pooling Manager")]
		[SerializeField] private PoolingChannel m_poolingChannel;
		[SerializeField] private PoolConfig m_poolConfig;
		
		[Header("Camera")]
		[SerializeField] private CameraChannel m_cameraChannel;
		[SerializeField] private ShakeTweenSettings m_shakeTweenSettings;


	}
}