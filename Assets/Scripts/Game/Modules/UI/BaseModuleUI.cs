using UnityEngine;
using UnityEngine.UI;
namespace StarWielder.Gameplay.Modules
{
	public class BaseModuleUI : MonoBehaviour
	{
		private BaseModule m_baseModule;
        [SerializeField] private Image m_OffImage;
		[SerializeField] private Image m_fillImage;

		public virtual void Initialize(BaseModule baseModule)
		{
			if (baseModule == null || baseModule.Settings == null)
			{
				return;
			}

			m_baseModule = baseModule;

			m_OffImage.sprite = m_baseModule.Settings.ShopItem.sprite;
			m_fillImage.sprite = m_baseModule.Settings.ShopItem.sprite;
		}

		public virtual void SetAvailable()
		{
			m_fillImage.fillAmount = 1;
		}

		public virtual void SetUnavailable()
		{
			m_fillImage.fillAmount = 0;
		}

		public virtual void SetFill(float progress)
		{
			m_fillImage.fillAmount = progress;
		}
	}
}