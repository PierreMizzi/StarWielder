using UnityEngine;
using PierreMizzi.Useful.PoolingObjects;
using System;
using StarWielder.Gameplay.Player;
using System.Linq;


// 🟩 : Put mineral in pooling system
// 🟩 : Generate random minerals on random asteroids
// 🟧 : Interact with the sun to smelt
// 🟥 : Generate droplets of mineral

namespace StarWielder.Gameplay.Elements
{
	[ExecuteInEditMode]
	public class Mineral : MonoBehaviour
	{

		#region Main

		private Star m_sun;

		private void Destroy()
		{
		}

		#endregion

		#region MonoBehaviour

		private void OnEnable()
		{
			m_sun = GameObject.FindGameObjectWithTag("Star").GetComponent<Star>();
			m_currentDurability = m_totalDurability;
			ComputeSmeltingMaxDistance();
		}

		private void OnValidate()
        {
            ComputeSmeltingMaxDistance();
        }

		private void Update()
		{
			if (m_currentDurability < 0)
			{
				m_currentDurability -= SmeltingSpeedFromDistance() * Time.deltaTime;
				m_normalizedDurability = m_currentDurability / m_totalDurability;

				if (m_currentDurability <= 0)
				{
					Destroy();
				}
			}
		}

        #endregion

        #region Smelting

        [Header("Smelting")]
		[SerializeField] private float m_totalDurability;
		private float m_currentDurability;
		[SerializeField] private AnimationCurve m_smeltingSpeedCurve;

		private float m_normalizedDurability;
		private float m_currentDistance;

		public float SmeltingSpeedFromDistance()
		{
			m_currentDistance = Vector3.Distance(transform.position, m_sun.transform.position);
			return m_smeltingSpeedCurve.Evaluate(m_currentDistance);
		}

		#endregion

		#region Mineral Nuggets

		[SerializeField] private PoolingChannel m_poolingChannel;

		

		#endregion

		#region Debug

		Color defaultGizmosColor;
		float m_smeltingMaxDistance;

		private void ComputeSmeltingMaxDistance()
		{
			if (m_smeltingSpeedCurve.keys.Length > 0)
			{
				m_smeltingMaxDistance = m_smeltingSpeedCurve.keys[m_smeltingSpeedCurve.keys.Length - 1].time;
				Debug.Log("m_maxSmeltingDistance");
			}
		}

		protected void OnDrawGizmos()
		{
			defaultGizmosColor = Gizmos.color;

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, m_smeltingMaxDistance);

			Gizmos.color = defaultGizmosColor;
		}

        #endregion

    }
}