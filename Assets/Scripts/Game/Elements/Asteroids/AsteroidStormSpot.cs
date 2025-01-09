
using UnityEngine;

namespace StarWielder.Gameplay.Elements
{

	public class AsteroidStormSpot
	{
		public AsteroidStormSpot(Vector3 position)
		{
			this.position = position;
			isTaken = false;
		}

		public bool isTaken = false;

		public Vector3 position;
	}

}