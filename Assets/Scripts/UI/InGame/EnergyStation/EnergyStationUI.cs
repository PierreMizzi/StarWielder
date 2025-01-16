namespace StarWielder.UI
{
	using System;
	using PierreMizzi.Useful.UI;
	using StarWielder.Gameplay;
	using StarWielder.Gameplay.Player;
	using TMPro;
	using UnityEngine;
	using UnityEngine.EventSystems;
	using UnityEngine.UI;

	public class EnergyStationUI : MonoBehaviour, IDisplayHideAnimator, ICancelHandler
	{
		#region Behaviour

		[SerializeField] private GameChannel m_gameChannel;
		[SerializeField] private PlayerChannel m_playerChannel;

		[SerializeField] private EnergyStationSettings m_settings;

		private int m_coinValue;
		private int m_energyValue;

		private void CallbackEnterEnergyStation()
		{
			((IDisplayHideAnimator)this).Display();
		}

		private void CallbackLeaveEnergyStation()
		{
			((IDisplayHideAnimator)this).Hide();
		}

		#endregion

		#region MonoBehaviour

		private void Awake()
		{
			animator = GetComponent<Animator>();
			if (m_slider != null)
			{
				m_slider.onValueChanged.AddListener(CallbackSliderValueChanged);
			}
		}

		private void Start()
		{
			if (m_gameChannel != null)
			{
				m_gameChannel.onEnterEnergyStation += CallbackEnterEnergyStation;
				m_gameChannel.onLeaveEnergyStation += CallbackLeaveEnergyStation;
			}

			if (animator == null)
			{
				animator = GetComponent<Animator>();
			}

			m_slider.value = 0;
		}

		private void OnDestroy()
		{
			if (m_gameChannel != null)
			{
				m_gameChannel.onEnterEnergyStation -= CallbackEnterEnergyStation;
				m_gameChannel.onLeaveEnergyStation -= CallbackLeaveEnergyStation;
			}

			if (m_slider != null)
			{
				m_slider.onValueChanged.RemoveListener(CallbackSliderValueChanged);
			}
		}

		#endregion

		#region Coin To Energy UI

		[Header("Coin To Energy UI")]
		[SerializeField] private TextMeshProUGUI m_coinText;
		[SerializeField] private TextMeshProUGUI m_energyText;
		[SerializeField] private Slider m_slider;


		/// <summary>
		/// Linked to "Slider" UI "OnValueChanged"'s callback
		/// </summary>
		public void CallbackSliderValueChanged(float value)
		{
			if (m_settings == null)
			{
				return;
			}

			m_coinValue = Mathf.CeilToInt(m_settings.SpentCoin.Evaluate(value));
			m_coinText.text = m_coinValue.ToString();

			m_energyValue = Mathf.CeilToInt(m_settings.ReceivedEnergy.Evaluate(value));
			m_energyText.text = m_energyValue.ToString();
		}

		#endregion

		#region Buttons

		public void CallbackConfirmButton()
		{
			IDisplayHide.Hide();

			m_gameChannel.onDecrementCurrency.Invoke(m_coinValue);
			m_gameChannel.onIncrementMineralNugget.Invoke(m_coinValue);
		}

		public void CallbackCancelButton()
		{
			IDisplayHide.Hide();
		}

		#endregion

		#region IDisplayHideAnimator

		public IDisplayHideAnimator IDisplayHide
		{
			get
			{
				return this;
			}
		}


		public Animator animator { get; set; }

		#endregion

		#region ICancelHandler

		public void OnCancel(BaseEventData eventData)
		{
			IDisplayHide.Hide();
		}

		#endregion

	}
}