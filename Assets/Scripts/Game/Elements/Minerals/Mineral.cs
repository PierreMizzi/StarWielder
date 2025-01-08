using UnityEngine;
using PierreMizzi.Useful.PoolingObjects;
using StarWielder.Gameplay.Player;

// 🟥 : Put all Mineral and MineralNugget settings inside a ScriptableObject

namespace StarWielder.Gameplay.Elements
{
	public class Mineral : MonoBehaviour
	{

		#region Main

		private Star m_sun;

		private void Destroy()
		{
		}

		#endregion

		#region MonoBehaviour

		private void Awake()
		{
			m_sun = GameObject.FindGameObjectWithTag("Star").GetComponent<Star>();
			m_currentDurability = m_totalDurability;
			ComputeSmeltingMaxDistance();

			InitializeNuggetCreation();
		}

		private void OnValidate()
        {
            ComputeSmeltingMaxDistance();
        }

		private void Update()
		{
			if (m_currentDurability > 0)
			{
				m_currentDurability -= SmeltingSpeedFromDistance() * Time.deltaTime;
				m_normalizedDurability = m_currentDurability / m_totalDurability;
				ManageNuggetCreation();

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

		[Header("Mineral Nuggets")]
		[SerializeField] private PoolingChannel m_poolingChannel;
		[SerializeField] private MineralNugget m_mineralNuggetPrefab;
		[SerializeField] private int m_nuggetTotalCount;

		[Header("Projection")]
		[SerializeField] private float m_projectMinDistance;
		[SerializeField] private float m_projectionMaxDistance;

		[Header("Scale")]
		[SerializeField] private float m_nuggetMinScale;
		[SerializeField] private float m_nuggetMaxScale;

		private float m_nuggetTotalCountFraction;
        private double m_nuggetCreationFraction;

        private void InitializeNuggetCreation()
		{
			m_nuggetTotalCountFraction = 1.0f / (m_nuggetTotalCount + 1);
			m_nuggetCreationFraction = 1.0 - m_nuggetTotalCountFraction;
		}

		private void ManageNuggetCreation()
		{
			if (m_normalizedDurability <= m_nuggetCreationFraction)
			{
				m_nuggetCreationFraction -= m_nuggetTotalCountFraction;
				CreateNugget();
			}
		}

		private void CreateNugget()
		{
			if (m_poolingChannel == null)
			{
				return;
			}

			GameObject pooledObject = m_poolingChannel.onGetFromPool.Invoke(m_mineralNuggetPrefab.gameObject);

			if (pooledObject != null && pooledObject.TryGetComponent(out MineralNugget nugget))
			{
				nugget.Initialize();

				nugget.transform.position = transform.position;
				nugget.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360f));
				nugget.transform.localScale = new Vector3(0.25f, 0.25f, 1f);

				Vector3 projectionPosition = GetRandomProjectionPosition(nugget.transform.position);
				Vector3 projectionRotation = GetRandomProjectionRotation(nugget.transform.rotation.eulerAngles);
				Vector3 projectionScale = GetRandomScale();

				nugget.Project(projectionPosition, projectionRotation, projectionScale);
			}
		}

		private Vector3 GetRandomProjectionPosition(Vector3 initialPosition)
		{
			float randomAngle = Random.Range(0.0f, Mathf.PI * 2f);
			Vector3 randomDirection = new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0f);
			float randomDistance = Random.Range(m_projectMinDistance, m_projectionMaxDistance);
			return initialPosition + randomDirection * randomDistance; 
		}

		private Vector3 GetRandomProjectionRotation(Vector3 initialRotation)
		{
			if (Random.value > 0.5f)
				initialRotation.z += Random.Range(0f, 359f);
			else
				initialRotation.z -= Random.Range(0f, 359f);

			return initialRotation;
		}

		private Vector3 GetRandomScale()
		{
			float randomScale = Random.Range(m_nuggetMinScale, m_nuggetMaxScale);
			return new Vector3(randomScale, randomScale, 1f);
		}


		#endregion

		#region Debug

		Color defaultGizmosColor;
		float m_smeltingMaxDistance;


        private void ComputeSmeltingMaxDistance()
		{
			if (m_smeltingSpeedCurve.keys.Length > 0)
			{
				m_smeltingMaxDistance = m_smeltingSpeedCurve.keys[m_smeltingSpeedCurve.keys.Length - 1].time;
			}
		}

		protected void OnDrawGizmos()
		{
			defaultGizmosColor = Gizmos.color;

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, m_smeltingMaxDistance);

			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(transform.position, m_projectMinDistance);
			Gizmos.DrawWireSphere(transform.position, m_projectionMaxDistance);

			Gizmos.color = defaultGizmosColor;
		}

        #endregion

    }
}