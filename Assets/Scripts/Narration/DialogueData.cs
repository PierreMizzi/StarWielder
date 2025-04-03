using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSunSocketData", menuName = "StarWielder/DialogueSunSocketData", order = 0)]
public class DialogueData : ScriptableObject
{
	[TextArea(5, 10)] public string text;
}