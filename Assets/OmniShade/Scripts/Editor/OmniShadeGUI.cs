//------------------------------------
//             OmniShade
//     Copyright© 2022 OmniShade     
//------------------------------------

using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using System.Linq;

/**
 * This class manages the GUI for the shader, automatically enabling/disabling keywords when values change.
 */
public static class OmniShade {
	public const string NAME = "OmniShade";
	public const string DOCS_URL = "https://www.omnishade.io/documentation/features";
	public const string FULL_URL = "https://assetstore.unity.com/packages/vfx/shaders/omnishade-mobile-optimized-shader-215111";
}

public class OmniShadeGUI : ShaderGUI {
	// Shader keywords to detect to automatically enable/disable
	List<(string keyword, string name, PropertyType type, Vector4 defaultValue)> props = 
		new List<(string keyword, string name, PropertyType type, Vector4 defaultValue)>(){
		("BASE_CONTRAST", "_Contrast", PropertyType.Float, Vector4.one),
		("BASE_SATURATION", "_Saturation", PropertyType.Float, Vector4.one),
		("MATCAP", "_MatCapTex", PropertyType.Texture, Vector4.one),	
		("MATCAP_CONTRAST", "_MatCapContrast", PropertyType.Float, Vector4.one),
		("NORMAL_MAP", "_NormalTex", PropertyType.Texture, Vector4.one),
		("AMBIENT", "_AmbientBrightness", PropertyType.Float, Vector4.zero),
		("ZOFFSET", "_ZOffset", PropertyType.Float, Vector4.zero),
	};

	// Parameters that are ON by default
	List<(string keyword, string name)> defaultOnParams = new List<(string keyword, string name)>() {
		("MATCAP_PERSPECTIVE", "_MatCapPerspective" ),
		("SHADOWS_ENABLED", "_ShadowsEnabled" ),
		("FOG", "_Fog" ),
	};

	const string HEADER_GROUP = "HeaderGroup";

	enum PropertyType {
		Float, Vector, Texture
	};

	struct PropertyHeader {
		public string headerName;
		public bool isOpen;
		public PropertyHeader(string _header, bool _isOpen) {
			this.headerName = _header;
			this.isOpen = _isOpen;
		}
	}; 

	int forceExpand = 1;
	int prevPreset = -1;
	List<Material> prevSelectedMats = new List<Material>();
	Dictionary<string, PropertyHeader> propertyHeaders = new Dictionary<string, PropertyHeader>();

	static Dictionary<string, GUIContent> toolTipsCache = new Dictionary<string, GUIContent>();

	public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties) {
		this.RenderGUI(materialEditor, properties);

		// Multi-selection
		var mat = materialEditor.target as Material;
		var mats = new List<Material>();
		if (mat != null)
			mats.Add(mat);
		foreach (var selected in Selection.objects) {
			if (selected.GetType() == typeof(Material)) {
				var selectedMat = selected as Material;
				if (selectedMat != mat && selectedMat != null &&
					selectedMat.shader.name.Contains(OmniShade.NAME))
					mats.Add(selectedMat);
			}
		}

		// Loop selected materials
		foreach (var material in mats) {
			this.AutoEnableShaderKeywords(material);
			this.SetPresetIfChanged(material);
			if (mats.Count > 1)
				this.prevPreset = -1;
		}

		// Reset preset if selection changed
		if (mats.Count == 1 && this.prevSelectedMats.Count == 1 &&
			mats[0] != null && this.prevSelectedMats[0] != null && mats[0].name != this.prevSelectedMats[0].name)
			this.prevPreset = -1;
		this.prevSelectedMats = mats;
	}

	public void AutoEnableShaderKeywords(Material mat) {
		foreach (var prop in this.props) {
			if (!mat.HasProperty(prop.name))
				continue;

			// Check if property value is being used (not set to default)
			bool isInUse = false;
			switch (prop.type) {
				case PropertyType.Float:
					isInUse = mat.GetFloat(prop.name) != prop.defaultValue.x;
					break;
				case PropertyType.Vector:
					isInUse = mat.GetVector(prop.name) != prop.defaultValue;
					break;
				case PropertyType.Texture:
					isInUse = mat.GetTexture(prop.name) != null;
					break;
				default:
					break;
			}

			// Enable or disable shader keyword
			if (isInUse) {
				if (!mat.IsKeywordEnabled(prop.keyword))
					mat.EnableKeyword(prop.keyword);
			} 
			else if (mat.IsKeywordEnabled(prop.keyword))
				mat.DisableKeyword(prop.keyword);
		}

		// Set keywords for parameters that are ON by default
		foreach (var defaultOnParam in this.defaultOnParams) {
			if (mat.HasProperty(defaultOnParam.name) && mat.GetFloat(defaultOnParam.name) == 1)
				mat.EnableKeyword(defaultOnParam.keyword);
		}

		// MatCap Static Rotation default angle points to camera
		if (mat.IsKeywordEnabled("MATCAP_STATIC") && mat.HasProperty("_MatCapRot") && 
			mat.GetVector("_MatCapRot") == Vector4.zero) {
			var cam = GameObject.FindObjectOfType<Camera>();
			if (cam != null) {
				var matCapRot = Vector4.zero;
				matCapRot = -cam.transform.rotation.eulerAngles * Mathf.PI / 180;
				mat.SetVector("_MatCapRot", matCapRot);
			}
		}
	}

	void SetPresetIfChanged(Material mat) {
		int preset = (int)mat.GetFloat("_Preset");
		if (this.prevPreset != -1 && this.prevPreset != preset) {  // New preset selected - set values
			mat.SetFloat("_Cull", 2);					// Back
			mat.SetFloat("_ZTest", 4);					// LessEqual
			mat.SetFloat("_BlendOp", 0);				// Add

			switch (preset) {
				case 0:		// Opaque
					mat.SetFloat("_ZWrite", 1);
					mat.SetFloat("_SourceBlend", 5);	// SrcAlpha
					mat.SetFloat("_DestBlend", 10);		// OneMinusSrcAlpha
					if (mat.renderQueue >= 2450)
						mat.renderQueue = 2000;
					mat.SetFloat("_Cutout", 0);			// Cutout
					mat.DisableKeyword("CUTOUT");
					break;

				case 1:		// Transparent
					mat.SetFloat("_ZWrite", 0);
					mat.SetFloat("_SourceBlend", 5);	// SrcAlpha
					mat.SetFloat("_DestBlend", 10);		// OneMinusSrcAlpha
					if (mat.renderQueue < 3000)
						mat.renderQueue = 3000;
					mat.SetFloat("_Cutout", 0);			// Cutout
					mat.DisableKeyword("CUTOUT");
					break;

				case 2:		// Transparent Additive
					mat.SetFloat("_ZWrite", 0);
					mat.SetFloat("_SourceBlend", 1);	// One
					mat.SetFloat("_DestBlend", 1);		// One
					if (mat.renderQueue < 3000)
						mat.renderQueue = 3000;
					mat.SetFloat("_Cutout", 0);			// Cutout
					mat.DisableKeyword("CUTOUT");
					break;

				case 3:		// Transparent Additive Alpha
					mat.SetFloat("_ZWrite", 0);
					mat.SetFloat("_SourceBlend", 5);	// SrcAlpha
					mat.SetFloat("_DestBlend", 1);		// One
					if (mat.renderQueue < 3000)
						mat.renderQueue = 3000;
					mat.SetFloat("_Cutout", 0);			// Cutout
					mat.DisableKeyword("CUTOUT");
					break;

				case 4:		// Opaque Cutout
					mat.SetFloat("_Cull", 0);			// Disabled
					mat.SetFloat("_ZWrite", 1);
					mat.SetFloat("_SourceBlend", 5);	// SrcAlpha
					mat.SetFloat("_DestBlend", 10);		// OneMinusSrcAlpha
					mat.SetFloat("_Cutout", 1);			// Cutout
					mat.EnableKeyword("CUTOUT");
					if (mat.renderQueue < 2450 || mat.renderQueue >= 3000)
						mat.renderQueue = 2450;
					break;

				default:
					Debug.LogError(OmniShade.NAME + ": Unrecognized Preset (" + preset +")");
					break;
			}
		}
		
		this.prevPreset = preset;
	}

	void RenderGUI(MaterialEditor materialEditor, MaterialProperty[] properties) {
		materialEditor.SetDefaultGUIWidths();

		// Documentation button
		var content = new GUIContent(EditorGUIUtility.IconContent("_Help")) {
			text = OmniShade.NAME + " Docs",
			tooltip = OmniShade.DOCS_URL
		};
      	if (GUILayout.Button(content))
		  Help.BrowseURL(OmniShade.DOCS_URL);
		
		// Expand/Close all buttons
		GUILayout.BeginHorizontal();
		var expandAll = new GUIContent(EditorGUIUtility.IconContent("Toolbar Plus")) { text = "Expand All" };
		if (GUILayout.Button(expandAll))
			this.forceExpand = 0;
		if (GUILayout.Button("Expand Active"))
			this.forceExpand = 1;
		var closeAll = new GUIContent(EditorGUIUtility.IconContent("Toolbar Minus")) { text = "Collapse" };
		if (GUILayout.Button(closeAll))
			this.forceExpand = 2;
		GUILayout.EndHorizontal();

		// Fetch header info
		this.FetchPropertyHeaders(materialEditor);

		// Render GUI
		bool isFoldoutOpen = true;
		string currentHeaderName = string.Empty;
		foreach (var prop in properties) {
			// If start of header, begin a new foldout group
			if (this.propertyHeaders.ContainsKey(prop.name)) {
				// Close previous foldout group
				if (!string.IsNullOrEmpty(currentHeaderName))
					EditorGUILayout.EndFoldoutHeaderGroup();

				// Begin foldout header
				var header = this.propertyHeaders[prop.name];
				currentHeaderName = header.headerName;
				var defaultColor = GUI.backgroundColor;				
				isFoldoutOpen = header.isOpen = this.BeginFoldoutHeader(header.isOpen, header.headerName);
				this.propertyHeaders[prop.name] = header;				
			}

			// Render shader property
			if (isFoldoutOpen)
				this.RenderShaderProperty(materialEditor, prop);
		}
		// Append Unity rendering options to end of groups
		if (isFoldoutOpen) {
			materialEditor.RenderQueueField();
			EditorGUILayout.Separator();
			EditorGUILayout.LabelField("Rendering", EditorStyles.boldLabel);
			materialEditor.EnableInstancingField();
			materialEditor.DoubleSidedGIField();
		}
		EditorGUILayout.EndFoldoutHeaderGroup();

		// Ad
		GUILayout.Space(10);
		if (GUILayout.Button("Upgrade to " + OmniShade.NAME))
			Help.BrowseURL(OmniShade.FULL_URL);
	}

	void RenderShaderProperty(MaterialEditor materialEditor, MaterialProperty prop) {
		string label = prop.displayName;
		var content = this.GetTooltip(label);
		materialEditor.ShaderProperty(prop, content);
	}
	
	bool BeginFoldoutHeader(bool isOpen, string label) {
		var content = this.GetTooltip(label);
		var defaultColor = GUI.backgroundColor;
		GUI.backgroundColor = new Color(1.35f, 1.35f, 1.35f);
		isOpen = EditorGUILayout.BeginFoldoutHeaderGroup(isOpen, content);
		GUI.backgroundColor = defaultColor;
		return isOpen;
	}
	

	void FetchPropertyHeaders(MaterialEditor materialEditor) {
		var defaultOnHeaders = new string[] {
			"Culling And Blending"
		};

		var mat = materialEditor.target as Material;
		var shader = mat.shader;
		int numProps = shader.GetPropertyCount();
		
		// Check which headers have active items
		var headerActiveDic = new Dictionary<string, bool>();
		if (this.forceExpand == 1) {
			string currentHeaderName = string.Empty;
			for (int i = 0; i < numProps; i++) {
				// Check if header group
				var propAttrs = shader.GetPropertyAttributes(i);
				for (int j = 0; j < propAttrs.Length; j++) {
					string propAttr = propAttrs[j];
					if (propAttr.StartsWith(HEADER_GROUP))
						currentHeaderName = this.GetHeaderGroupName(propAttr);
				}

				// Skip if no headers found yet
				if (string.IsNullOrEmpty(currentHeaderName))
					continue;
				
				// Check if property active
				bool isInUse = false;
				if (defaultOnHeaders.Contains(currentHeaderName))
					isInUse = true;
				else {
					string propDesc = shader.GetPropertyDescription(i);
					string propName = shader.GetPropertyName(i);
					var propType = shader.GetPropertyType(i);
					if (this.IsPropertyActive(mat, propName, propDesc, propType))
						isInUse = true;
				}

				// Cache
				if (headerActiveDic.ContainsKey(currentHeaderName))
					headerActiveDic[currentHeaderName] |= isInUse;
				else
					headerActiveDic.Add(currentHeaderName, isInUse);
			}
		}

		// Fetch property headers
		var newProps = new List<string>();
		for (int i = 0; i < numProps; i++) {
			var propAttrs = shader.GetPropertyAttributes(i);
			for (int j = 0; j < propAttrs.Length; j++) {
				// Skip if not a header attribute
				string propAttr = propAttrs[j];
				if (!propAttr.StartsWith(HEADER_GROUP)) 
					continue;

				string propName = shader.GetPropertyName(i);
				string headerName = this.GetHeaderGroupName(propAttr);
				newProps.Add(propName);

				// Update cache if something changed
				if (!this.propertyHeaders.ContainsKey(propName) || 
					this.propertyHeaders[propName].headerName != headerName || this.forceExpand != -1) {
					bool isOpen = this.forceExpand == 0;  // 0: Expand all or 2: Collapse
					if (!this.propertyHeaders.ContainsKey(propName)) {  // New entry
						if (this.forceExpand == -1 || this.forceExpand == 1)  // -1: Keep existing or 1: Expand Active
							isOpen = headerActiveDic.ContainsKey(headerName) ? headerActiveDic[headerName] : true;
						var header = new PropertyHeader(headerName, isOpen);
						this.propertyHeaders.Add(propName, header);
					}
					else {  // Update existing entry
						if (this.forceExpand == -1)  // -1: Keep existing
							isOpen = this.propertyHeaders[propName].isOpen;
						else if (this.forceExpand == 1)  // 1: Expand Active
							isOpen = headerActiveDic.ContainsKey(headerName) ? headerActiveDic[headerName] : true;
						var header = new PropertyHeader(headerName, isOpen);
						this.propertyHeaders[propName] = header;
					}
				}
			}
		}
		this.forceExpand = -1;

		// Remove any headers that were deleted
		var propNames = new List<string>();
		foreach (var propName in this.propertyHeaders.Keys)
			propNames.Add(propName);
		var deletedProps = propNames.Except(newProps);
		foreach (var deletedProp in deletedProps)
			this.propertyHeaders.Remove(deletedProp);
	}

	bool IsPropertyActive(Material mat, string propName, string propDesc, ShaderPropertyType propType) {
		if (!mat.HasProperty(propName))
			return false;
		if ((propType == ShaderPropertyType.Float && propDesc.StartsWith("Enable") && mat.GetFloat(propName) == 1) ||
			(propType == ShaderPropertyType.Texture && mat.GetTexture(propName) != null) ||
			(propName == "_AmbientBrightness" && mat.GetFloat("_AmbientBrightness") != 0))
			return true;
		return false;
	}
	
	string GetHeaderGroupName(string header) {
		int headerGroupLen = HEADER_GROUP.Length + 1;
		return header.Substring(headerGroupLen, header.LastIndexOf(")") - headerGroupLen);
	}

	GUIContent GetTooltip(string label) {
		// Check cache first
		if (OmniShadeGUI.toolTipsCache.ContainsKey(label))
			return OmniShadeGUI.toolTipsCache[label];

		string tooltip = string.Empty;
		switch (label) {
			case "Ignore Main Texture Alpha": tooltip = "Ignore the alpha channel on the texture, forcing it to be opaque."; break;
			case "Perspective Correction": tooltip = "Reduces texture sliding when rotating the camera. For stationary cameras, disable this to improve performance."; break;
			case "Use Static Rotation": tooltip = "Lock the MatCap rotation to prevent it from rotating with the camera. Does not work with Normal Map."; break;
			case "Ambient Brightness": tooltip = "Intensity of the Environment Lighting from the Lighting Settings."; break;
			case "Culling And Blend Preset": tooltip = "Presets depending on the object type."; break;
			case "Culling": tooltip = "Which face of the geometry is rendered."; break;
			case "Z Write": tooltip = "If enabled, this object occludes those behind it."; break;
			case "Z Test": tooltip = "Set to Always if this object should always render, even if behind others."; break;
			case "Depth Offset": tooltip = "Moves the object closer/farther from camera to improve visibility."; break;
			case "Cutout Transparency": tooltip = "Discards pixels with alpha less than 0.5. Performance may be slow on mobile."; break;
			default: tooltip = ""; break;
		}

		// Create tool tip and cache
		var content = new GUIContent() {
			text = label,
			tooltip = tooltip
		};
		OmniShadeGUI.toolTipsCache.Add(label, content);
		return content;
	}
}
