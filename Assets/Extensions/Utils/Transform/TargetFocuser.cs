using UnityEngine;

namespace PierreMizzi.Useful
{
	/// <summary>
	/// Simple behaviour to point a forward of any transform to another one
	/// </summary>
	[ExecuteInEditMode]
	public class TargetFocuser : MonoBehaviour
	{
		[SerializeField]
		private Transform _target = null;

		[SerializeField]
		private bool _activeUpdate = true;

		[SerializeField]
		private bool _xAxisLocked = false;

		[SerializeField]
		private float _xAxisLockedAngle = 0f;

		[SerializeField]
		private bool _zAxisLocked = false;

		[SerializeField]
		private float _zAxisLockedAngle = 0f;

		[SerializeField]
		private bool _isFlipped = false;

		public Transform target
		{
			get { return _target; }
			set { _target = value; }
		}
		public bool isUpdating
		{
			get { return _activeUpdate; }
			set { _activeUpdate = value; }
		}

		private Quaternion correctedRotation = new Quaternion();
		private Quaternion k_flipQuaternion = Quaternion.Euler(0f, 180f, 0f);

		private Camera _mainCamera = null;

		private void OnEnable()
		{
			_mainCamera = Camera.main;
		}

		private void Start()
		{
			_mainCamera = Camera.main;
		}

		// Update is called once per frame
		private void FixedUpdate()
		{
			if (_activeUpdate)
			{
				Apply();
			}
		}

		public void Align(Transform target)
		{
			transform.LookAt(target);

			correctedRotation = Quaternion.Euler(
				_xAxisLocked ? -_xAxisLockedAngle : transform.rotation.eulerAngles.x, // X
				transform.rotation.eulerAngles.y, // Y
				_zAxisLocked ? -_zAxisLockedAngle : transform.rotation.eulerAngles.z // Z
			);

			transform.rotation = correctedRotation;

			if (!_isFlipped)
			{
				transform.rotation *= k_flipQuaternion;
			}
		}

		public void Apply()
		{
			if (_target != null)
				Align(_target);
			else if (_mainCamera != null)
				Align(_mainCamera.transform);
		}
	}
}
