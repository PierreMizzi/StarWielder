using UnityEngine;

namespace StarWielder.Tools
{
	public class OvalOrbit : MonoBehaviour
	{

		#region Behaviour

		[SerializeField] protected Transform m_pointA;
		[SerializeField] protected Transform m_pointB;

		#endregion

		#region MonoBehaviour

		protected virtual void Update()
		{
			// Rotation
			transform.rotation *= Quaternion.Euler(Vector3.forward * m_rotationSpeed * Time.deltaTime);

			// Orbit
			if (m_pointA == null || m_pointB == null)
			{
				return;
			}
		
			m_orbitTime += (Time.deltaTime * m_orbitSpeed);
			UpdatePointPosition(m_pointA, m_orbitTime);
			UpdatePointPosition(m_pointB, m_orbitTime + Mathf.PI);
		}

		#endregion

		#region Orbit

		[Header("Orbit")]

		[SerializeField] protected float m_orbitSpeed = 1;

		[SerializeField] protected float m_orbitHorizontalLength = 1f;
		[SerializeField] protected float m_orbitVerticalLength = 1f;

		protected float m_orbitTime = 0;
		protected Vector3 m_tmpPosition;

		protected virtual void UpdatePointPosition(Transform point, float time)
		{
			m_tmpPosition.x = Mathf.Cos(time) * m_orbitHorizontalLength;
			m_tmpPosition.y = Mathf.Sin(time) * m_orbitVerticalLength;
			point.localPosition = m_tmpPosition;
		}

		protected virtual void SetRandomOrbitTimeOffset()
		{
			m_orbitTime = Random.Range(0, 100);
		}

		#endregion

		#region Rotation

		[Header("Rotation")]
		[SerializeField] private float m_rotationSpeed = 10f;

		#endregion

		#region Name
			
		#endregion

	}
}
