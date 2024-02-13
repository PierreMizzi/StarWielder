using System;
using UnityEngine;

namespace StarWielder.Gameplay.Player
{
	public class ShipHealthModifier : MonoBehaviour
	{
		[SerializeField] private float m_healthModification = 1;
		public float healthModification => m_healthModification;

		public Action onModify = () => { };
	}
}