using System.Collections.Generic;
using UnityEngine;
using PierreMizzi.Useful.PoolingObjects;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{

	[SerializeField] private PoolingChannel m_poolingChannel;
	[SerializeField] private SpriteRenderer m_spriteRenderer;


	private Rigidbody2D m_rigidbody;


	private void Awake()
	{
		m_rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		// TODO : 🟥 Make it better, maybe ?
		if (transform.position.x > 15)
			m_poolingChannel.onReleaseToPool.Invoke(gameObject);
	}

	public void Initialize(Vector3 direction, Color color)
	{
		m_rigidbody.velocity = direction;
		m_spriteRenderer.color = color;
	}

	#region Health Flower

	[Header("Health Flower")]
	[SerializeField] private List<Transform> m_healthFlowerAnchors = new List<Transform>();
	public List<Transform> healthFlowerAnchors { get { return m_healthFlowerAnchors; } }

	#endregion

}