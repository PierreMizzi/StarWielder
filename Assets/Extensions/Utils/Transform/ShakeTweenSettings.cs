using DG.Tweening;

namespace PierreMizzi.Useful
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "ShakeTweenSettings", menuName = "Utils/Shake Tween Settings", order = 0)]
	public class ShakeTweenSettings : ScriptableObject
	{
		[SerializeField] private float m_duration = 1f;
		[SerializeField] private Vector3 m_strength = Vector3.one;
		[SerializeField] private int m_vibrato = 1;
		[SerializeField] private float m_randomness = 1f;
		[SerializeField] private bool m_snapping = true;
		[SerializeField] private bool m_fadeOut = true;
		[SerializeField] private ShakeRandomnessMode m_randomnessMode = ShakeRandomnessMode.Full;

		public float duration => m_duration;
		public Vector3 strength => m_strength;
		public int vibrato => m_vibrato;
		public float randomness => m_randomness;
		public bool snapping => m_snapping;
		public bool fadeOut => m_fadeOut;
		public ShakeRandomnessMode randomnessMode => m_randomnessMode;

		public Tween PlayPositionShake(Transform transform)
		{
			return transform.DOShakePosition(m_duration, m_strength, m_vibrato, m_randomness, m_snapping, m_fadeOut, m_randomnessMode);
		}

		public Tween PlayRotationShake(Transform transform)
		{
			return transform.DOShakeRotation(m_duration, m_strength, m_vibrato, m_randomness, m_fadeOut, m_randomnessMode);
		}

	}
}