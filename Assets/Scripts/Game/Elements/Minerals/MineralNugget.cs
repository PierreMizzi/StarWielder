using System;
using DG.Tweening;
using PierreMizzi.Useful.PoolingObjects;
using StarWielder.Gameplay;
using StarWielder.Gameplay.Player;
using UnityEngine;

[RequireComponent(typeof(SunDestroyable))]
public class MineralNugget : MonoBehaviour
{
	
	#region Behaviour

	[SerializeField] private GameChannel m_gameChannel;
	[SerializeField] private PoolingChannel m_poolingChannel;

	private SunDestroyable m_sunDestroyable;

	private CircleCollider2D m_collider;

	public void Initialize()
	{
		SetStateNormal();
	}

	public void Project(Vector3 endPosition, Vector3 endRotation, Vector3 endScale)
	{
		transform.DOMove(endPosition, 2.0f).SetEase(Ease.OutCirc);
		transform.DORotate(endRotation, 4f).SetEase(Ease.OutCirc);
		transform.DOScale(endScale, 0.5f).SetEase(Ease.OutBack);
	}

	public void Collect(Transform shipCollectPoint)
	{
		m_collider.enabled = false;

		Vector3 fromPosition = transform.position;
		Vector3 fromScale = transform.localScale;

		DOVirtual
		.Float(
			0f,
			1f,
			0.75f,
			(float value) =>
			{
				transform.position = Vector3.Lerp(fromPosition, shipCollectPoint.position, value);
				transform.localScale = Vector3.Lerp(fromScale, Vector3.zero, value);
			}
		)
		.SetEase(Ease.OutCubic)
		.OnComplete(CollectComplete);
	}

    private void CollectComplete()
    {
		m_gameChannel?.onIncrementMineralNugget.Invoke(1);
		ReleaseToPool();
	}

	private void ReleaseToPool()
	{
		if (m_poolingChannel == null)
		{
			return;
		}
		m_poolingChannel.onReleaseToPool.Invoke(gameObject);
	}

	#endregion

	#region Animation

	private Animator m_animator;

	public void SetStateNormal()
	{
		transform.localScale = Vector3.one;
		m_collider.enabled = true;
		m_animator?.SetBool("IsBurned", false);
	}

	public void SetStateBurn()
	{
		transform.localScale *= 0.8f;
		m_collider.enabled = false;
		m_animator?.SetBool("IsBurned", true);
	}



	/// <summary>
	/// Called when "Burn" animation clip is finished
	/// </summary>
	public void CallbackBurnFinished()
	{
		ReleaseToPool();
	}
		
	#endregion

	#region MonoBehaviour

	private void Awake()
	{
		m_sunDestroyable = GetComponent<SunDestroyable>();
		m_collider = GetComponent<CircleCollider2D>();
		m_animator = GetComponent<Animator>();

		if (m_sunDestroyable != null)
		{
			m_sunDestroyable.onSunDestroyed += SetStateBurn;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.TryGetComponent(out MineralCollector collector))
		{
			Collect(collector.CollectionPoint);
		}
	}

	private void OnDestroy()
	{
		if (m_sunDestroyable != null)
		{
			m_sunDestroyable.onSunDestroyed -= SetStateBurn;
		}
	}



    #endregion

}