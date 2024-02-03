using System;
using UnityEngine;
namespace StarWielder.Gameplay.Player
{
	public class StarAbsorbable : MonoBehaviour
	{
		[SerializeField] private float m_energy = 3f;
		public float energy => m_energy;

		public Action onAbsorb = () => { };
	}
}