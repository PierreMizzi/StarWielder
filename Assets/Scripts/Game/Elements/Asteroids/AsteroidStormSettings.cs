using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "AsteroidSpawningConfig", menuName = "StarWielder/AsteroidSpawningConfig", order = 0)]
public class AsteroidStormSettings : ScriptableObject
{
	[Header("Storm")]
	public Vector3 startingPosition;
	public float length = 20f;
	public float width = 20f;

	public float boundLimits = 15;

	[Header("Spots")]
	public float cellSize = 1.5f;
	public float minOffsetDistance = 0f;
	public float maxOffsetDistance = 0.5f;

	[HideInInspector] public int lengthAmountCell;
	[HideInInspector] public int widthAmountCell;

	[Header("Asteroids")]
	public float asteroidSpawnChance = 0.33f;
	public float asteroidMinVelocity = 1f;
	public float asteroidMaxVelocity = 1.25f;
	public float asteroidRndVelocityAngle = 10;

	[Header("Minerals")]

	[Header("Visual settings")]
	public Gradient mineralColors;
	public float mineralSpawnChance = 0.33f;
	public float mineralMinScale = 0.5f;
	public float mineralMaxScale = 1.5f;

	public Color GetMineralRandomColor()
	{
		float random = Random.Range(0f, 1f);
		return mineralColors.Evaluate(random);
	}

	public float GetRandomScale()
	{
		return Random.Range(mineralMinScale, mineralMaxScale);
	}

	[Header("Health Flower")]
	public int healthFlowerAmount = 2;

	public float healthFlowerLivableRange = 4;

	[Header("Twin Stars")]
	public float twinStarsSpawnChance = 0.5f;

	public int m_twinStarsMinCount = 2;
	public int m_twinStarsMaxCount = 5;

	private void Awake()
	{
		lengthAmountCell = (int)(length / cellSize);
		widthAmountCell = (int)(width / cellSize);
	}

}