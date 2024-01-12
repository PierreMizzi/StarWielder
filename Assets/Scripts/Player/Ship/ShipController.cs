using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using PierreMizzi.SoundManager;

namespace StarWielder.Gameplay.Player
{
	/// <summary>
	/// Ship's controlling class for all it's movement capabilities.
	/// For gameplay related behaviours, see Ship.cs
	/// </summary>
	public class ShipController : MonoBehaviour
	{

		#region Main

		[SerializeField] private PlayerSettings m_settings = null;
		private Ship m_ship;
		private Camera m_camera;

		#endregion

		#region Monobehaviour

		private void Awake()
		{
			m_ship = GetComponent<Ship>();
			m_boxCollider = GetComponent<BoxCollider2D>();
		}

		private void Start()
		{
			m_camera = Camera.main;
			ComputeCorners();
		}

		private void Update()
		{
			ReadMousePositionInputs();
			Rotate();

			if (m_canDash && m_dashActionReference.action.IsPressed())
				Dash();

			if (!m_isDashing)
			{
				ReadLocomotionInputs();
				Move();
			}
		}

		private void LateUpdate()
		{
			ClampPositionInsideBoundaries();
		}

		#endregion

		#region Boundaries

		private BoxCollider2D m_boxCollider;

		private Vector2 m_topRightBoundCorner;
		private Vector2 m_botLeftBoundCorner;

		private Vector3 clampedPosition;

		public void ComputeCorners()
		{
			Vector2 extents = new Vector2(m_boxCollider.bounds.extents.x, m_boxCollider.bounds.extents.y);
			m_topRightBoundCorner = GameManager.topRightBoundCorner - extents;
			m_botLeftBoundCorner = GameManager.botLeftBoundCorner + extents;
		}

		/// <summary>
		/// Make sure the Ship can't leave Screen boundaries
		/// </summary>
		private void ClampPositionInsideBoundaries()
		{
			clampedPosition = transform.position;
			clampedPosition.x = Mathf.Clamp(clampedPosition.x, m_botLeftBoundCorner.x, m_topRightBoundCorner.x);
			clampedPosition.y = Mathf.Clamp(clampedPosition.y, m_botLeftBoundCorner.y, m_topRightBoundCorner.y);
			transform.position = clampedPosition;
		}

		#endregion

		#region Position & Rotation

		[Header("Position & Rotation")]
		[SerializeField] private InputActionReference m_locomotionActionReference = null;
		[SerializeField] private InputActionReference m_mousePositionActionReference = null;

		// Position
		private Vector3 m_locomotionActionValue;
		private Vector3 m_offsetPosition;
		private Vector3 m_nextPosition;
		private Vector3 m_currentVelocity;

		// Rotation
		private Vector2 m_mousePositionActionValue;
		private Vector2 m_screenSpacePosition;
		private Vector3 m_orientation;

		/// <summary> 
		/// Multiply the direction Ship-Mouse Cursor by this value to avoid weird jittering
		/// </summary>
		[SerializeField]
		private float m_orientationMagnitude = 2;

		private void Move()
		{
			m_offsetPosition = m_locomotionActionValue * m_settings.speed * Time.deltaTime;
			m_nextPosition = transform.position + m_offsetPosition;

			transform.position = Vector3.SmoothDamp(transform.position, m_nextPosition, ref m_currentVelocity, m_settings.smoothTime);
		}

		private void ReadLocomotionInputs()
		{
			m_locomotionActionValue = m_locomotionActionReference.action.ReadValue<Vector2>().normalized;
		}

		private void Rotate()
		{
			m_screenSpacePosition = m_camera.WorldToScreenPoint(transform.position);

			m_orientation = (m_mousePositionActionValue - m_screenSpacePosition).normalized * m_orientationMagnitude;
			transform.right = m_orientation;
		}

		private void ReadMousePositionInputs()
		{
			m_mousePositionActionValue = m_mousePositionActionReference.action.ReadValue<Vector2>();
		}

		#endregion

		#region Dash

		[Header("Dash")]
		[SerializeField] private DashStar m_dashStar;
		[SerializeField] private InputActionReference m_dashActionReference;

		private float m_dashCooldownTime;

		private bool m_isDashing = false;
		private bool m_canDash = true;

		private void Dash()
		{
			Vector3 dashDirection;
			if (m_locomotionActionValue == Vector3.zero)
				dashDirection = transform.up;
			else
				dashDirection = m_locomotionActionValue;
			Vector3 endPosition = transform.position + dashDirection * m_settings.dashDistance;

			m_canDash = false;
			m_dashCooldownTime = 0f;

			m_isDashing = true;
			m_ship.SetIsDashing(m_isDashing);

			transform.DOMove(endPosition, m_settings.dashDuration)
					 .SetEase(Ease.OutSine)
					 .OnComplete(OnCompleteDash);

			m_dashStar.Use();
			SoundManager.PlaySFX(SoundDataID.SHIP_DASH);
		}

		private void OnCompleteDash()
		{
			m_isDashing = false;
			m_ship.SetIsDashing(m_isDashing);
			StartCoroutine(DashCooldownIEnumerator());
		}

		private IEnumerator DashCooldownIEnumerator()
		{
			while (m_dashCooldownTime <= m_settings.dashCooldownDuration)
			{
				m_dashCooldownTime += Time.deltaTime;
				yield return null;
			}

			m_canDash = true;
			m_dashStar.Recharge();
		}

		#endregion



	}
}