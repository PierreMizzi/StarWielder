using DG.Tweening;
using PierreMizzi.Useful.PoolingObjects;
using StarWielder.Gameplay;
using StarWielder.Gameplay.Player;
using UnityEngine;

public class MineralNugget : MonoBehaviour
{
	
	#region Behaviour

	[SerializeField] private GameChannel m_gameChannel;
	[SerializeField] private PoolingChannel m_poolingChannel;

	public void Project(Vector3 endPosition, Vector3 endRotation, Vector3 endScale)
	{
		transform.DOMove(endPosition, 2.0f).SetEase(Ease.OutCirc);
		transform.DORotate(endRotation, 4f).SetEase(Ease.OutCirc);
		transform.DOScale(endScale, 0.5f).SetEase(Ease.OutBack);
	}

	#endregion

	#region MonoBehaviour

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "MineralCollector")
		{
			m_gameChannel?.onCollectMineralNugget.Invoke(1);
			OnDestroy();
		}
		else if (other.gameObject.TryGetComponent(out Star star))
		{
			OnDestroy();
		}
	}

	private void OnDestroy()
	{
		if (m_poolingChannel == null)
		{
			return;
		}

		m_poolingChannel.onReleaseToPool.Invoke(gameObject);
	}

	#endregion

}