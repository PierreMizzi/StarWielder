using PierreMizzi.Rendering;
using UnityEngine;

public class SunSocketRing : MonoBehaviour
{
	[SerializeField] private Transform m_pivot;
	public Transform Pivot => m_pivot;

	[Header("Lights")]
	[SerializeField] private MaterialPropertyBlockModifier m_propertBlockLights;
	[SerializeField] private Color m_colorLightsOn;
	[SerializeField] private Color m_colorLightsOff;

	[Header("Solar Panel")]
	[SerializeField] private MaterialPropertyBlockModifier m_propertyBlockSolarPanel;
	[SerializeField] private Color m_colorSolarPanelOn;
	[SerializeField] private Color m_colorSolarPanelOff;

	public void On()
	{
		m_propertBlockLights.SetProperty(MaterialPropertyName.k_emissionColor, m_colorLightsOn);
		m_propertyBlockSolarPanel.SetProperty(MaterialPropertyName.k_baseColor, m_colorSolarPanelOn);
	}

    public void Off()
    {
		m_propertBlockLights.SetProperty(MaterialPropertyName.k_emissionColor, m_colorLightsOff);
		m_propertyBlockSolarPanel.SetProperty(MaterialPropertyName.k_baseColor, m_colorSolarPanelOff);
	}

}