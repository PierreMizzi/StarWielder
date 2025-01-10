namespace StarWielder.UI
{
	using System;
	using PierreMizzi.Useful.UI;
	using StarWielder.Gameplay;
	using StarWielder.Gameplay.Player;
	using TMPro;
	using UnityEngine;
	using UnityEngine.UI;

	public class EnergyStationUI : MonoBehaviour, IDisplayHideAnimator
	{
		#region Behaviour

		[SerializeField] private GameChannel m_gameChannel;
		[SerializeField] private PlayerChannel m_playerChannel;

		private void Initialize()
		{
			m_slider.value = 0;
		}

		#endregion

		#region MonoBehaviour

		private void Awake()
		{
			animator = GetComponent<Animator>();
		}

		private void Start()
		{
			if (m_gameChannel != null)
			{
				m_gameChannel.onEnterEnergyStation += CallbackEnterEnergyStation;
				m_gameChannel.onLeaveEnergyStation += CallbackLeaveEnergyStation;
			}
		}

		private void CallbackEnterEnergyStation()
		{
			((IDisplayHideAnimator)this).Display();
		}

		private void CallbackLeaveEnergyStation()
		{
			((IDisplayHideAnimator)this).Hide();
		}

		#endregion

		#region Coin To Energy UI

		[Header("Coin To Energy UI")]
		[SerializeField] private TextMeshProUGUI m_coinText;
		[SerializeField] private TextMeshProUGUI m_energyText;
		[SerializeField] private Slider m_slider;

		#endregion

		#region Confirmation buttons

		

		#endregion

		#region IDisplayHideAnimator

		public Animator animator { get; set; }

		void IDisplayHideAnimator.Hide()
		{
			Debug.Log("My own hide !");
		}

		#endregion
	}
}