using UnityEngine;

namespace StarWielder.Gameplay
{
	[CreateAssetMenu(fileName = "ResourcesStageSettings", menuName = "StarWielder/StageSettings/Resources", order = 0)]
	public class ResourcesStageSettings : StageSettings
	{
		public ResourcesStageSettings()
		{
			m_type = StageStateType.Resources;
		}
	}
}