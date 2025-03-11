using System;
using PierreMizzi.Useful;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarWielder.Gameplay.Player
{
	public delegate void SunDelegate(Star sun);

	[RequireComponent(typeof(CircleCollider2D))]
	public class SunSocket : MonoBehaviour
	{
		[SerializeField] private LayerMask m_starLayerMask;

		#region Behaviour

		public SunDelegate onSocket = (Star sun)=> {};
		public SunDelegate onUnsocket = (Star sun) => { };

		private bool m_isEnabled = true;
		private Star m_currentStar;

		public virtual void Enable()
		{
			m_isEnabled = true;
		}

		public virtual void Disable()
		{
			m_isEnabled = false;
		}

		public virtual void Socket(Star star)
		{
			if (star == null)
			{
				return;
			}

			m_currentStar = star;
			m_currentStar.ChangeState(StarStateType.Locked);
			m_currentStar.transform.SetParent(transform);
			m_currentStar.transform.localPosition = Vector3.zero;
			m_currentStar.mouseClickAction.action.performed += CallbackMouseClickAction;

			onSocket.Invoke(star);
		}

        private void CallbackMouseClickAction(InputAction.CallbackContext context)
        {
			Unsocket();
        }

        public virtual void Unsocket()
		{
			if (m_currentStar != null)
			{
				onUnsocket.Invoke(m_currentStar);

				m_currentStar.transform.SetParent(null);
				m_currentStar.mouseClickAction.action.performed -= CallbackMouseClickAction;
				m_currentStar = null;
			}
		}
			
		#endregion

		#region MonoBehaviour

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (m_isEnabled == false || other == null)
			{
				return;
			}

			if (UtilsClass.CheckLayer(m_starLayerMask.value, other.gameObject.layer))
			{
				if (other.TryGetComponent(out Star sun))
				{
					Socket(sun);
				}
			}

		}

		#endregion

	}
}