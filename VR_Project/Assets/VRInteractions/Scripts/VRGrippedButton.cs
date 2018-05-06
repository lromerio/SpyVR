using UnityEngine;
using System.Collections;

/// <summary>
/// VR gripped button. Requires VR button. This button responds to being "clicked" rather than a physical press.
/// </summary>
[RequireComponent(typeof(VRButton))]
public class VRGrippedButton : MonoBehaviour {

	/// <summary>
	/// Animation that makes the button press down
	/// </summary>
	public Animation ButtonAnim;

	/// <summary>
	/// Button component
	/// </summary>
	VRButton Button;


	void OnEnable()
	{
		Button = GetComponent<VRButton> ();
		if (Button == null)
			Debug.LogError ("VRButton is null"); 


		Collider collider = GetComponent<Collider> ();
		collider.isTrigger = true; // This button should only work as a trigger
	}

	void OnTriggerEnter(Collider _collider)
	{
		if (Button.Interactable == true)
			ActivateButton (_collider.attachedRigidbody);
	}

	/// <summary>
	/// Triggers the button if the controllers action key is down
	/// </summary>
	/// <param name="_controllerBody">Controller body.</param>
	public void ActivateButton(Rigidbody _controllerBody)
	{
		if (_controllerBody == null)
			return;
		
		SteamVR_TrackedObject controller = _controllerBody.gameObject.GetComponent<SteamVR_TrackedObject> ();

		if (controller == null)
			return;

		var device = SteamVR_Controller.Input ((int)controller.index);

		// TODO: Will update this to allow the key to be specified
		if (device.GetHairTrigger () == true) {
			if (ButtonAnim != null)
				ButtonAnim.Play ();
		}
	}

}
