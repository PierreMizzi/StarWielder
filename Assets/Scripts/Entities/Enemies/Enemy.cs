using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private float m_energy = 1f;

	public float energy => m_energy;

}