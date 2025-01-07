using StarWielder.Gameplay;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourcesStageSettings", menuName = "StarWielder/StageSettings/Default", order = 0)]
public class StageSettings : ScriptableObject
{
	[Multiline(5)]
	[SerializeField] private string note;

	[SerializeField] protected StageStateType m_type;
	public StageStateType Type => m_type;
}