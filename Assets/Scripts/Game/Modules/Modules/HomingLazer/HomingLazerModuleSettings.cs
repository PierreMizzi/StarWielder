using StarWielder.Gameplay.Modules;
using UnityEngine;

[CreateAssetMenu(fileName = "HomingLazerModuleSettings", menuName = "StarWielder/Modules/HomingLazerModuleSettings", order = 0)]
public class HomingLazerModuleSettings : BaseModuleSettings
{
	[Header("Homing Lazer")]
	[SerializeField] private float m_energyToHealthRatio;
	[SerializeField] private float m_lazerWidth = 0.2f;
	[SerializeField] private float m_lazerSpeed = 300;
	[SerializeField] private float m_lazerTrailDelay = 0.1f;
	[SerializeField] private float m_lazerTrailLifetime = 1f;
	[SerializeField] private Color m_trailBaseColor = Color.yellow;

	public float EnergyToHealthRatio => m_energyToHealthRatio;
	public float LazerWidth => m_lazerWidth;
	public float LazerSpeed => m_lazerSpeed;
	public float LazerTrailDelay => m_lazerTrailDelay;
	public float LazerTrailLifetime => m_lazerTrailLifetime;
	public Color TrailBaseColor => m_trailBaseColor;

}