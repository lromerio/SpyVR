using UnityEngine;
using System.Collections;

/// <summary>
/// Example script. Controls a point light and a material to sets the light bulb's colour and intensity.
/// </summary>
[RequireComponent(typeof(Light))]
public class LightController : MonoBehaviour {

	// Required components
	Light mLight;
	MeshRenderer mMeshRenderer;
	Animation mAnimation;

	void OnEnable()
	{
		
		mLight = GetComponent<Light> ();
		if (mLight == null)
			Debug.LogError ("Liight is missing from " + name);

		mMeshRenderer = GetComponent<MeshRenderer> ();
		if (mMeshRenderer == null)
			Debug.LogError ("MeshRenderer is missing from " + name);

		mAnimation = GetComponent<Animation> ();
		if (mAnimation == null)
			Debug.LogError ("Animation is missing from " + name);
		else
			mAnimation["Bob"].time = Random.Range(0.0f, mAnimation["Bob"].length);

		// Initialize the light with it's first colour and intensity
		SetLightColour (Colours [CurrentColour]);
		SetModelColour (Materials [CurrentColour]);		
	}

	/// <summary>
	/// List of colours the light cycles through
	/// </summary>
	public Color[] Colours;

	/// <summary>
	/// List of materials the light cycles through.
	/// </summary>
	public Material[] Materials;

	/// <summary>
	/// Cache to remember position in the colour list
	/// </summary>
	public int CurrentColour = 0;

	/// <summary>
	/// Sets the point light intensity.
	/// </summary>
	/// <param name="_value">Value. 0 - 1</param>
	void SetIntensity(float _value)
	{
		mLight.intensity = _value * 8.0f;
	}

	/// <summary>
	/// Sets the point light colour.
	/// </summary>
	/// <param name="_colour">Colour.</param>
	void SetLightColour (Color _colour)
	{
		mLight.color = _colour;
	}

	/// <summary>
	/// Sets the model material colour.
	/// </summary>
	/// <param name="_mat">Mat.</param>
	void SetModelColour (Material _mat)
	{
		mMeshRenderer.material = _mat;
	}

	/// <summary>
	/// Function called by the button to change the lights
	/// </summary>
	public void ColourChanged()
	{
		CurrentColour = CurrentColour >= ( Colours.Length - 1 ) ?  0 : CurrentColour + 1;

		SetLightColour (Colours [CurrentColour]);
		SetModelColour (Materials [CurrentColour]);
			
	}

	/// <summary>
	/// Changes the light intensity, called by the lever
	/// </summary>
	/// <param name="_lever">Lever.</param>

	public void IntensityChanged(VRLever _lever, float _currentValue, float _lastValue)
	{
		SetIntensity(_currentValue);
	}

	/// <summary>
	/// Changes the light intensity, called by the lever
	/// </summary>
	/// <param name="_lever">Lever.</param>
	public void IntensityChanged(VRLever _lever)
	{
		if (_lever == null) {
			Debug.LogError ("_lever is null");
			return;
		}
			
		SetIntensity(_lever.Value);
	}

}
