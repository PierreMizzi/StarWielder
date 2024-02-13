using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour
{

	private Rigidbody2D m_rigidbody;

	[SerializeField] private SpriteRenderer m_spriteRenderer;

	private void Awake()
	{
		m_rigidbody = GetComponent<Rigidbody2D>();
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