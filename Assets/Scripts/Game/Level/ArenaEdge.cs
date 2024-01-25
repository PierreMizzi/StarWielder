using UnityEngine;

namespace StarWielder.Gameplay
{

	public class ArenaEdge : MonoBehaviour
	{

		[SerializeField] private BoxCollider2D m_boxCollider;

		[SerializeField] private Transform m_sprite;

		public void SetSize(float width, float height)
		{
			m_boxCollider.size = new Vector2(width, height);
			m_sprite.localScale = new Vector3(width, height, 1);
		}

	}
}