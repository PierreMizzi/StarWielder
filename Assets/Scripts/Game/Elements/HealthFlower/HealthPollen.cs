using System;
using UnityEngine;
using PierreMizzi.Useful.PoolingObjects;

namespace StarWielder.Gameplay.Player
{
	[RequireComponent(typeof(ShipHealthModifier))]
	public class HealthPollen : MonoBehaviour
	{

		[SerializeField] private PoolingChannel m_poolingChannel;

		[SerializeField] private float m_rotationSpeed = 10f;
		private ShipHealthModifier m_healthModifier;

		private void CallbackConsume()
		{
			m_poolingChannel.onReleaseToPool.Invoke(gameObject);
		}

		#region MonoBehaviour

		private void Awake()
		{
			m_healthModifier = GetComponent<ShipHealthModifier>();
		}

		private void Start()
		{
			if (m_healthModifier != null)
				m_healthModifier.onModify += CallbackConsume;
		}

		private void Update()
		{
			// transform.rotation *= Quaternion.Euler(Vector3.forward * m_rotationSpeed * Time.deltaTime);
		}

		private void OnDestroy()
		{
			if (m_healthModifier != null)
				m_healthModifier.onModify -= CallbackConsume;
		}

		#endregion

	}

}
