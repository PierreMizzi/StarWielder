using DG.Tweening;
using StarWielder.Gameplay.Player;
using UnityEngine;
using PierreMizzi.Useful.PoolingObjects;


namespace StarWielder.Gameplay.Enemies
{

	[RequireComponent(typeof(StarAbsorbable))]
	[RequireComponent(typeof(ShipHealthModifier))]
	public class OverheaterMine : MonoBehaviour
	{
		[SerializeField] private PoolingChannel m_poolingChannel;

		private OverheaterMineSpawner m_spawner;
		private StarAbsorbable m_starAbsorbable;
		private ShipHealthModifier m_healthModifier;
		[SerializeField] private float m_tweenDuration = 1f;
		private Tween m_spawningTween;


		public void Initialize(OverheaterMineSpawner spawner, Vector3 rndPosition)
		{
			m_spawner = spawner;
			m_spawningTween = transform.DOMove(rndPosition, m_tweenDuration).SetEase(Ease.OutCubic);
		}

		public void Kill()
		{
			m_spawner.hasMine = false;

			m_poolingChannel.onReleaseToPool.Invoke(gameObject);

			m_spawningTween.Kill();
		}

		#region MonoBehaviour

		private void Awake()
		{
			m_starAbsorbable = GetComponent<StarAbsorbable>();
			m_healthModifier = GetComponent<ShipHealthModifier>();
		}

		private void Start()
		{
			if (m_starAbsorbable != null)
				m_starAbsorbable.onAbsorb += Kill;

			if (m_healthModifier != null)
				m_healthModifier.onModify += Kill;
		}

		private void OnDestroy()
		{
			if (m_starAbsorbable != null)
				m_starAbsorbable.onAbsorb -= Kill;

			if (m_healthModifier != null)
				m_healthModifier.onModify -= Kill;
		}

		#endregion

	}
}