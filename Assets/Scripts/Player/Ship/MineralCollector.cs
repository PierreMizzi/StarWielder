using UnityEngine;

public class MineralCollector : MonoBehaviour
{
	[SerializeField] private Transform m_collectionPoint;
	public Transform CollectionPoint => m_collectionPoint;
}