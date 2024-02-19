using System;
using UnityEngine;
using PierreMizzi.Useful.PoolingObjects;
using DG.Tweening;

namespace StarWielder.Gameplay.Player
{
	[RequireComponent(typeof(ShipHealthModifier))]
	public class HealthPollen : MonoBehaviour
	{

		[SerializeField] private PoolingChannel m_poolingChannel;
		[SerializeField] private SpriteRenderer m_renderer;

		private ShipHealthModifier m_healthModifier;

		private void CallbackConsume()
		{
			m_poolingChannel.onReleaseToPool.Invoke(gameObject);
		}

		public void SetTemperature(float temperature)
		{
			m_renderer.material.SetFloat("_Temperature", temperature);
		}

		#region MonoBehaviour

		private void Awake()
		{
			m_healthModifier = GetComponent<ShipHealthModifier>();

			m_renderer.material = new Material(m_renderer.material);
		}

		private void Start()
		{
			m_isDetached = false;

			if (m_healthModifier != null)
				m_healthModifier.onModify += CallbackConsume;
		}

		private void Update()
		{
			if (m_isDetached)
				transform.rotation *= Quaternion.Euler(Vector3.forward * m_rotationSpeed * Time.deltaTime);
		}

		private void OnDestroy()
		{
			if (m_healthModifier != null)
				m_healthModifier.onModify -= CallbackConsume;
		}

		#endregion

		#region Bloom

		[Header("Bloom")]
		[SerializeField] private float m_rotationSpeed = 10f;
		[SerializeField] private float m_detachDistance = 3f;
		[SerializeField] private float m_detachDuration = 1.5f;
		private bool m_isDetached = false;

		public void Bloom()
		{
			transform.parent = null;
			transform.DOMove(transform.position + transform.up * m_detachDistance, m_detachDuration).OnComplete(() =>
			{
				m_isDetached = true;
			});
		}

		#endregion

	}

}
