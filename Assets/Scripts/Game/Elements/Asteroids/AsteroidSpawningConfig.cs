using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidSpawningConfig", menuName = "StarWielder/AsteroidSpawningConfig", order = 0)]
public class AsteroidSpawningConfig : ScriptableObject
{
	[Header("Storm")]
	public Vector3 startingPosition;
	public float length = 10f;
	public float width = 10f;

	[Header("Voronoi")]
	public float cellSize = 0.5f;
	public float minOffsetDistance = 0;
	public float maxOffsetDistance = 0;

	[HideInInspector] public int lengthAmountCell;
	[HideInInspector] public int widthAmountCell;

	[Header("Asteroids")]
	public float spawnPercentage = 0.5f;
	public float minVelocityScalar = 1f;
	public float maxVelocityScalar = 1.25f;
	public float randomVelocityAngle = 10;

	[Header("Minerals")]


	[Header("Visual settings")]
	public Gradient tintGradient;
	public float minScale = 0.5f;
	public float maxScale = 1.5f;

	public Color GetRandomColor()
	{
		float random = Random.Range(0f, 1f);
		return tintGradient.Evaluate(random);
	}

	public float GetRandomScale()
	{
		return Random.Range(minScale, maxScale);
	}

	[Header("Health Flower")]
	public int amountHealthFlower;

	private void Awake()
	{
		lengthAmountCell = (int)(length / cellSize);
		widthAmountCell = (int)(width / cellSize);
	}

}