using UnityEngine;
using System.Collections;

/// <summary>
/// Component makes objects with a material that can be transparent invisible on spawn then "fade in" over a specified period
/// </summary>
public class FadeIn : MonoBehaviour {

	/// <summary>
	/// Duration for object to become visible
	/// </summary>
	public float FadeInTime = 1;
	private Color cachedColour;
	MeshRenderer r;

	void Awake()
	{
		r = GetComponent<MeshRenderer> ();
		cachedColour = r.material.color;

		StartFade ();
	}

	void StartFade()
	{
		

		//Color TargetColour = cachedColour;
		Color CurrentColour = cachedColour;
		CurrentColour.a = 0;

		r.material.color = CurrentColour;

		StartCoroutine (FadeInLinear ());
	}

	/// <summary>
	/// Fades in material colour with lerp.
	/// </summary>
	IEnumerator FadeInLinear()
	{
		//Color TargetColour = cachedColour;
		Color CurrentColour = cachedColour;
		CurrentColour.a = 0;

		r.material.color = CurrentColour;
		float t = 0;
		while (CurrentColour.a < cachedColour.a) {
			t += (Time.fixedDeltaTime / FadeInTime);

			CurrentColour.a = Mathf.Lerp(0, cachedColour.a, t);
			r.material.color = CurrentColour;
			yield return new WaitForFixedUpdate();
		}
	}

	public bool test = false;
	void Update()
	{
		if (test == true) {
			test = false;
			StartFade ();
		}
	}
}
