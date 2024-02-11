using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidSpawningConfig", menuName = "StarWielder/AsteroidSpawningConfig", order = 0)]
public class AsteroidSpawningConfig : ScriptableObject
{
	[Header("Storm")]
	public float length = 10f;
	public float width = 10f;

	[Header("Voronoi")]
	public float cellSize = 0.5f;
	public float minOffsetDistance = 0;
	public float maxOffsetDistance = 0;

	public int lengthAmountCell;
	public int widthAmountCell;

	[Header("Asteroids")]
	public float spawnPercentage = 0.5f;
	public float minStrength = 0.25f;
	public float maxStrength = 0.25f;
	public float randomAngleRange = 10;

	private void Awake()
	{
		lengthAmountCell = (int)(length / cellSize);
		widthAmountCell = (int)(width / cellSize);
	}

}