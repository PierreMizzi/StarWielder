using StarWielder.Gameplay;
using UnityEngine;

public class StageSettings : ScriptableObject
{
	[Multiline(5)]
	[SerializeField] private string note;

	[SerializeField] protected StageStateType m_type;
	public StageStateType Type => m_type;
}