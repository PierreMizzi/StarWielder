using System;
using PierreMizzi.Useful.UI;
using StarWielder.Gameplay;
using StarWielder.Gameplay.Elements;
using StarWielder.Gameplay.Modules;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StarWielder.UI
{
	public class SpaceShopUI : MonoBehaviour, IDisplayHideAnimator, ICancelHandler
	{
		#region Behaviour

		[SerializeField] private GameChannel m_gameChannel;
		[SerializeField] private ModuleChannel m_moduleChannel;

		private ShopItemDisplay m_currentitemDisplay;

		private void CallbackSocketedShopItemDisplay(ShopItemDisplay itemDisplay)
		{
			if (itemDisplay == null)
			{
				return;
			}

			m_currentitemDisplay = itemDisplay;

			m_title.text = m_currentitemDisplay.CurrentItem.name;
			m_description.text = m_currentitemDisplay.CurrentItem.description;
			m_preview.sprite = m_currentitemDisplay.CurrentItem.sprite;
			m_price.text = m_currentitemDisplay.CurrentItem.price.ToString();

			m_buyButton.interactable = m_gameChannel.HasEnoughMineralNugget(m_currentitemDisplay.CurrentItem.price);

			(this as IDisplayHideAnimator).Display();
		}

		private void CallbackUnsocketedShopItemDisplay(ShopItemDisplay itemDisplay)
		{
			(this as IDisplayHideAnimator).Hide();
		}

		#endregion

		#region MonoBehaviour
		private void Awake()
		{
			if (TryGetComponent(out Animator animator))
			{
				this.animator = animator;
			}
		}

		private void Start()
		{
			if (m_gameChannel != null)
			{
				m_gameChannel.onSocketedShopItemDisplay += CallbackSocketedShopItemDisplay;
				m_gameChannel.onUnsocketedShopItemDisplay += CallbackUnsocketedShopItemDisplay;
			}
		}

		void OnDestroy()
		{
			if (m_gameChannel != null)
			{
				m_gameChannel.onSocketedShopItemDisplay -= CallbackSocketedShopItemDisplay;
				m_gameChannel.onUnsocketedShopItemDisplay -= CallbackUnsocketedShopItemDisplay;
			}
		}

		#endregion

		#region UI

		[Header("UI")]
		[SerializeField] private TextMeshProUGUI m_title;
		[SerializeField] private TextMeshProUGUI m_description;
		[SerializeField] private Image m_preview;
		[SerializeField] private TextMeshProUGUI m_price;
		[SerializeField] private Button m_buyButton;

		public void CallbackClickBuyButton()
		{
			if (m_gameChannel == null || m_moduleChannel == null)
			{
				return;
			}

			if (m_gameChannel.HasEnoughMineralNugget(m_currentitemDisplay.CurrentItem.price))
			{
				m_gameChannel.onDecrementMineralNugget(m_currentitemDisplay.CurrentItem.price);

				switch (m_currentitemDisplay.CurrentItem.ItemType())
				{
					case ShopItem.Type.Module:
						SpaceShopItemModule moduleShopItem = m_currentitemDisplay.CurrentItem as SpaceShopItemModule;
						m_moduleChannel.onEnableModuleType?.Invoke(moduleShopItem.moduleType);
						break;
					default:
						break;
				}

				(this as IDisplayHideAnimator).Hide();
				m_currentitemDisplay.Clear();
			}
		}

		public void CallbackClickCloseButton()
		{
			(this as IDisplayHideAnimator).Hide();
		}

		#endregion

		#region IDisplayHideAnimator

		public Animator animator { get; set; }

		#endregion

		#region ICancelHandler

		public void OnCancel(BaseEventData eventData)
		{
			(this as IDisplayHideAnimator).Hide();
		}

		#endregion
	}

}