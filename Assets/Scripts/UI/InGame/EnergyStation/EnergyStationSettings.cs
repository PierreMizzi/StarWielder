using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "EnergyStationSettings", menuName = "StarWielder/EnergyStationSettings", order = 0)]
public class EnergyStationSettings : ScriptableObject
{
	public AnimationCurve SpentCoin;
	public AnimationCurve ReceivedEnergy;
}