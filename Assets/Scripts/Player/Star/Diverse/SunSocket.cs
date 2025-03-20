using System;
using System.Collections.Generic;
using DG.Tweening;
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

		public bool IsOccupied => m_currentStar != null;

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

			Close();

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

				Open();
			}
		}

        #endregion

        #region MonoBehaviour

        protected void Awake()
        {
			InitialRing();
		}

        protected void Update()
        {
			RotateRings();

			if (Input.GetKeyDown(KeyCode.Keypad7))
			{
				Close();
			}
			else if (Input.GetKeyDown(KeyCode.Keypad8))
			{
				Open();
			}
        }

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

		#region Rings rotations

		[Header("Rings rotations")]
		[SerializeField] private float m_ringRotationSpeedOpen = 100f;
		[SerializeField] private float m_ringRotationSpeedClose = 150f;
		[SerializeField] private float m_ringSpeedOffset = 0.5f;

		[SerializeField] private List<SunSocketRing> m_rings;
		private SunSocketRing tmpRing;


		private void InitialRing()
		{
			for (int i = 0; i < m_rings.Count; i++)
			{
				tmpRing = m_rings[i];
				tmpRing.transform.rotation = UtilsClass.RandomRotation2D();
				tmpRing.Off();
			}
		}

		private void RotateRings()
		{
			for (int i = 0; i < m_rings.Count; i++)
			{
				tmpRing = m_rings[i];
				tmpRing.transform.RotateAround(transform.position, Vector3.forward, Time.deltaTime * (i + 1) * m_ringSpeedOffset * (IsOccupied ? m_ringRotationSpeedClose : m_ringRotationSpeedOpen));
			}
		}
			
		#endregion

		#region Rings Open/Close

		[SerializeField] private float m_openCloseDuration = 0.2f;

		[SerializeField] private float m_openRingAngle;
		[SerializeField] private float m_closeRingAngle;
		
		[ContextMenu("Open")]
		private void Open()
		{
			m_sunHologram.SetActive(true);

			DOVirtual
			.Float(
				0f,
				1f,
				m_openCloseDuration,
				(float value) =>
				{
					foreach (SunSocketRing ring in m_rings)
					{
						ring.Pivot.localRotation = Quaternion.Euler(Mathf.Lerp(m_closeRingAngle, m_openRingAngle, value), 0, 0);
					}
				}
			)
			.SetEase(Ease.OutCubic)
			.OnComplete(Opened);
		}

        private void Opened()
        {
			foreach (SunSocketRing ring in m_rings)
			{
				ring.Off();
			}
		}

        [ContextMenu("Close")]
		private void Close()
		{
			m_sunHologram.SetActive(false);

			DOVirtual
			.Float(
				0f,
				1f,
				m_openCloseDuration,
				(float value) =>
				{
					foreach (SunSocketRing ring in m_rings)
					{
						ring.Pivot.localRotation = Quaternion.Euler(Mathf.Lerp(m_openRingAngle, m_closeRingAngle, value), 0, 0);
					}
				}
			)
			.SetEase(Ease.OutCubic)
			.OnComplete(Closed);
		}

		private void Closed()
		{
			foreach (SunSocketRing ring in m_rings)
			{
				ring.On();
			}
		}

		#endregion

		#region Sun Hologranm

		[SerializeField] private GameObject m_sunHologram;
			
		#endregion

	}
}