
using PierreMizzi.Rendering;
using UnityEngine;

namespace StarWielder.Gameplay.Player
{
	public class SunMaterialConfigTool : MonoBehaviour
	{
		[SerializeField] private StarSettings m_sunSettings;
		[SerializeField] private float m_baseSpeed = 0.5f;
		[SerializeField] private float m_incrementedSpeed = 0.2f;


		[ContextMenu("Call CopyExamplesInSettings")]
		public void CopyExamplesInSettings()
		{
			if (m_sunSettings == null)
			{
				return;
			}

			m_sunSettings.otherSunMaterialConfigs.Clear();
			int index = 0;
			
			foreach (Transform child in transform)
			{
				if (child.TryGetComponent(out MaterialPropertyBlockModifier modifier))
				{
					SunMaterialConfig config = new()
					{
						innerColor = modifier.GetProperty(SunMaterialConfig.color_innerColor).colorValue,
						outerColor = modifier.GetProperty(SunMaterialConfig.color_outerColor).colorValue,
						scrollSpeed = m_baseSpeed + m_incrementedSpeed * index,
						trailColor = GetTrailColor(child),
					};
					m_sunSettings.otherSunMaterialConfigs.Add(config);

					index++;
				}
			}
		}

		public Color GetTrailColor(Transform child)
		{
			if (child.childCount == 0 || child.GetChild(0) == null)
			{
				return Color.white;
			}

			if (child.GetChild(0).TryGetComponent(out MaterialPropertyBlockModifier modifier))
			{
				return modifier.GetProperty(SunMaterialConfig.color_color).colorValue;
			}
			else
			{
				return Color.white;
			}
		}
	}
}