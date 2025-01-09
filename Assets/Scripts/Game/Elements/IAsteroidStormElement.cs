using StarWielder.Gameplay.Elements;
using UnityEngine;

public interface IAsteroidStormElement
{

	public AsteroidSpawnerManager manager {get; set;}

	public Rigidbody2D rigidbody2D { get; set; }

	public bool CheckOutOfBounds();

	public void DestroyOutOfBounds();

}