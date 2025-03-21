using System;
using System.Collections.Generic;
using UnityEngine;

namespace PierreMizzi.Rendering
{
	[RequireComponent(typeof(Renderer))]
	public class MaterialPropertyBlockModifier : MonoBehaviour
	{

		#region Common Material Properties


			
		#endregion

		private static MaterialPropertyBlock s_materialPropertyBlock { get; set; }

		[Serializable]
		public class Property
		{
			public enum Type
			{
				Int,
				Float,
				Vector,
				Color,
				Texture
			}

			public string name;
			public Type type;
			public float floatValue;
			public int intValue;
			[ColorUsage(true, true)] public Color32 colorValue;
			public Texture textureValue;
			public Vector4 vectorValue;
		}

		[SerializeField]
		private int m_materialIndex = 0;

		[SerializeField]
		private List<Property> m_propreties = new List<Property>();

		private new Renderer renderer { get; set; }

		public void Awake()
		{
			RendererReferenceSafety();

			if (s_materialPropertyBlock == null)
				s_materialPropertyBlock = new MaterialPropertyBlock();

			this.ApplyProperties();
		}

		private void RendererReferenceSafety()
		{
			if (renderer == null)
			{
				this.renderer = GetComponent<Renderer>();
			}
		}

		private void OnValidate()
		{
			if (s_materialPropertyBlock == null)
				s_materialPropertyBlock = new MaterialPropertyBlock();

			if (this.renderer == null)
				this.renderer = base.GetComponent<Renderer>();

			if (m_materialIndex >= this.renderer.sharedMaterials.Length)
				return;

			this.ApplyProperties();
		}

		private void ApplyProperties()
		{
			RendererReferenceSafety();

			this.renderer.GetPropertyBlock(s_materialPropertyBlock, m_materialIndex);

			foreach (Property property in m_propreties)
			{
				if (!this.renderer.sharedMaterials[m_materialIndex].HasProperty(property.name))
					continue;

				switch (property.type)
				{
					case Property.Type.Int:
						s_materialPropertyBlock.SetInt(property.name, property.intValue);
						break;

					case Property.Type.Float:
						s_materialPropertyBlock.SetFloat(property.name, property.floatValue);
						break;

					case Property.Type.Vector:
						s_materialPropertyBlock.SetVector(property.name, property.vectorValue);
						break;

					case Property.Type.Color:
						s_materialPropertyBlock.SetColor(property.name, property.colorValue);
						break;

					case Property.Type.Texture:
						if (property.textureValue != null)
							s_materialPropertyBlock.SetTexture(
								property.name,
								property.textureValue
							);
						else
							s_materialPropertyBlock.SetTexture(
								property.name,
								Texture2D.whiteTexture
							);
						break;
				}
			}

			this.renderer.SetPropertyBlock(s_materialPropertyBlock, m_materialIndex);
		}

		public void SetProperty(string propertyName, float value)
		{
			RendererReferenceSafety();

			this.renderer.GetPropertyBlock(s_materialPropertyBlock, m_materialIndex);
			Property property = m_propreties.Find(item => item.name == propertyName);
			if (property != null)
			{
				property.floatValue = value;
				s_materialPropertyBlock.SetFloat(property.name, property.floatValue);
				this.renderer.SetPropertyBlock(s_materialPropertyBlock, m_materialIndex);
			}
		}

		public void SetProperty(string propertyName, Color value)
		{
			RendererReferenceSafety();

			this.renderer.GetPropertyBlock(s_materialPropertyBlock, m_materialIndex);
			Property property = m_propreties.Find(item => item.name == propertyName);
			if (property != null)
			{
				property.colorValue = value;
				s_materialPropertyBlock.SetColor(property.name, property.colorValue);
				this.renderer.SetPropertyBlock(s_materialPropertyBlock, m_materialIndex);
			}
		}

		public void SetProperty(string propertyName, Vector4 value)
		{
			RendererReferenceSafety();

			this.renderer.GetPropertyBlock(s_materialPropertyBlock, m_materialIndex);
			Property property = m_propreties.Find(item => item.name == propertyName);
			if (property != null)
			{
				property.vectorValue = value;
				s_materialPropertyBlock.SetVector(property.name, property.vectorValue);
				this.renderer.SetPropertyBlock(s_materialPropertyBlock, m_materialIndex);
			}
		}

		public void SetProperty(string propertyName, Texture value)
		{
			RendererReferenceSafety();

			this.renderer.GetPropertyBlock(s_materialPropertyBlock, m_materialIndex);
			Property property = m_propreties.Find(item => item.name == propertyName);
			if (property != null)
			{
				property.textureValue = value;
				s_materialPropertyBlock.SetTexture(property.name, property.textureValue);
				this.renderer.SetPropertyBlock(s_materialPropertyBlock, m_materialIndex);
			}
		}

		public Property GetProperty(string propertyName)
		{
			if (String.IsNullOrEmpty(propertyName))
			{
				return null;
			}
			return m_propreties.Find((Property property) => property.name == propertyName);
		}
    }

	public static class MaterialPropertyName
	{
		public static string k_color = "_Color";

		public static string k_tint = "_Tint";

		/// <summary>
		/// Universal Render Pipeline/Lit |
		/// </summary>
		public static string k_baseColor = "_BaseColor";

		/// <summary>
		/// Universal Render Pipeline/Lit |
		/// </summary>
		public static string k_emissionColor = "_EmissionColor"; 
	}

}
