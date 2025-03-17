using PierreMizzi.Useful;
using UnityEngine;

namespace StarWielder.Gameplay
{
	public delegate void ShakeTweenDelegate(ShakeTweenSettings settings);

	[CreateAssetMenu(fileName = "CameraChannel", menuName = "StarWielder/Channels/CameraChannel", order = 0)]
	public class CameraChannel : ScriptableObject
	{

		public ShakeTweenDelegate onShakeCameraPosition  = (ShakeTweenSettings settings) => {};

	}
}