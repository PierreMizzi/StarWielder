using UnityEngine;

namespace PierreMizzi.Useful.PoolingObjects
{

	public class PooledParticleSystem : MonoBehaviour
	{

		[SerializeField] private PoolingChannel m_poolingChannel;

		private void OnParticleSystemStopped()
		{
			m_poolingChannel.onReleaseToPool.Invoke(gameObject);
		}

	}
}