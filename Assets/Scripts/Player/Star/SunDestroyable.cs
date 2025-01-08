using System;
using UnityEngine;

namespace StarWielder.Gameplay.Player
{
	public class SunDestroyable : MonoBehaviour
	{
		public Action onSunDestroyed = () => { };
	}
}