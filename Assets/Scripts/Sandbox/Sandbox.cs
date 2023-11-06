using PierreMizzi.SoundManager;
using PierreMizzi.Useful;
using UnityEngine;
using UnityEngine.Audio;

public class Sandbox : MonoBehaviour
{

	#region Trigger 2D

	// [SerializeField] private LayerMask m_layerMask;

	// private void OnTriggerEnter2D(Collider2D other)
	// {
	// 	if (UtilsClass.CheckLayer(m_layerMask.value, other.gameObject.layer))
	// 	{
	// 		Debug.Log(other.name);
	// 	}
	// }

	#endregion

	#region Pitch Variation

	[SerializeField] private SoundManagerToolSettings m_soundManagerSettings;

	[SerializeField] private SoundSource m_soundSource;

	[SerializeField] private float m_basePitchValue = 1f;

	[SerializeField] private float m_pitchShift = 0.1f;

	private const string k_pitchParameterName = "EnemyStarPitch";

	private void Awake()
	{
		SoundManager.Init("SoundManager");
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
			PlayAtPitch(1f);
		else if (Input.GetKeyDown(KeyCode.Alpha2))
			PlayAtPitch(2f);
		else if (Input.GetKeyDown(KeyCode.Alpha3))
			PlayAtPitch(3f);
		else if (Input.GetKeyDown(KeyCode.Alpha4))
			PlayAtPitch(4f);
		else if (Input.GetKeyDown(KeyCode.Alpha5))
			PlayAtPitch(5f);
	}

	private void PlayAtPitch(float pitchShiftIndex)
	{
		float pitch = m_basePitchValue + pitchShiftIndex * m_pitchShift;
		Debug.Log(pitch);
		m_soundSource.audioMixerGroup.audioMixer.SetFloat(k_pitchParameterName, pitch);
		m_soundSource.Play();
	}

	#endregion

}