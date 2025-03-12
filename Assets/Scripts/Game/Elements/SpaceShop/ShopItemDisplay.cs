using System;
using StarWielder.Gameplay.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace StarWielder.Gameplay.Elements
{
	public delegate void ShopItemDisplayDelegate(ShopItemDisplay itemDisplay);

	public class ShopItemDisplay : MonoBehaviour
	{

		#region Behaviour

		[SerializeField] private GameChannel m_gameChannel;

		private ShopItem m_currentItem;
		public ShopItem CurrentItem => m_currentItem;

		// [SerializeField] private Transform m_physicalItemAnchor;
		[SerializeField] private SpriteRenderer m_itemSpriteRenderer;

		public void AssignShopIten(ShopItem item)
		{
			if (item == null)
			{
				DisableSunSocket();
				return;
			}

			m_currentItem = item;
			m_itemSpriteRenderer.sprite = item.sprite;
			EnableSunSocket();
		}

		public void Clear()
		{
			m_currentItem = null;
			m_itemSpriteRenderer.sprite = null;
			DisableSunSocket();
		}

		#endregion

		#region MonoBehaviour

		private void Start()
		{
			if (name != null)
			{
				m_sunSocket.onSocket += CallbackSocket;
				m_sunSocket.onUnsocket += CallbackUnsocket;
			}
		}

		private void OnDestroy()
		{
			if (name != null)
			{
				m_sunSocket.onSocket -= CallbackSocket;
				m_sunSocket.onUnsocket -= CallbackUnsocket;
			}
		}

		#endregion

		#region Orbiter
		[SerializeField] private Orbiter m_orbiter;
		[SerializeField] private Vector2 m_orbiterMinMaxAngleTop;
		[SerializeField] private Vector2 m_orbiterMinMaxAngleBot;

		public void RandomizeOrbit()
		{
			bool topOrBot = Random.Range(0, 2) == 0;
			float rndAngle;

			if (topOrBot)
			{
				rndAngle = Mathf.Lerp(m_orbiterMinMaxAngleTop.x, m_orbiterMinMaxAngleTop.y, Random.Range(0, 1f));
				m_orbiter.isClockwise = true;
				m_orbiter.SetTime(rndAngle);
			}
			else
			{
				rndAngle = Mathf.Lerp(m_orbiterMinMaxAngleBot.x, m_orbiterMinMaxAngleBot.y, Random.Range(0, 1f));
				m_orbiter.isClockwise = false;
				m_orbiter.SetTime(rndAngle);
				m_orbiter.UpdatePosition();
			}
		}

		#endregion


		#region Sun Socket

		[SerializeField] private SunSocket m_sunSocket;
		[SerializeField] private SpriteRenderer m_sunSocketSpriteRenderer;

		[SerializeField] private Color m_enabledColor = Color.white;
		[SerializeField] private Color m_disabledColor = Color.grey;

		private void CallbackSocket(Star sun)
		{
			if (m_gameChannel == null)
			{
				return;
			}
			m_gameChannel.onSocketedShopItemDisplay.Invoke(this);
		}

		private void CallbackUnsocket(Star sun)
		{
			if (m_gameChannel == null)
			{
				return;
			}
			m_gameChannel.onUnsocketedShopItemDisplay.Invoke(this);
		}

		public void EnableSunSocket()
		{
			m_sunSocket.Enable();
			m_sunSocketSpriteRenderer.color = m_enabledColor;
		}

		public void DisableSunSocket()
		{
			m_sunSocket.Disable();
			m_sunSocketSpriteRenderer.color = m_disabledColor;
		}

		#endregion

		#region Visual

		[Header("Visual")]

		[SerializeField] private SpriteRenderer m_planetSpriteRenderer;

		[SerializeField] private Vector2 m_minMaxScale;

		public void SetSprite(Sprite sprite)
		{
			m_planetSpriteRenderer.sprite = sprite;
		}

		public void RandomizeScale()
		{
			float rndScale = Random.Range(m_minMaxScale.x, m_minMaxScale.y);
			m_planetSpriteRenderer.transform.localScale = new Vector3(rndScale, rndScale, 1f);
		}

		#endregion
	}
}