using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// VR adds a grippable handle to the lever. Makes the lever attachable to the VR controller.
/// </summary>
public class VRHandle : MonoBehaviour {

	public string 						Button;
	CharacterJoint						HandJoint;

	private Rigidbody 					OldConnection;
	private bool 						bAttached = false;

	/// <summary>
	/// Controller that is connected to the VRHandle
	/// </summary>
	private SteamVR_TrackedObject 		AttachedController;

	/// <summary>
	/// Object that is the spawn point of the handle joint
	/// </summary>
	public Transform 					HandlePosition;

	/// <summary>
	/// The joint that is spawned to connect the controller to the lever
	/// The prefab can be found in the prefabs folder
	/// </summary>
	public Transform 					HandleJointPrefab;

	/// <summary>
	/// Current joint object (null if there is no connection)
	/// </summary>
	private Transform 					JointObject;

	/// <summary>
	/// Controllers that are currently colliding with the VRHandle object
	/// </summary>
	private List<SteamVR_TrackedObject> ActiveControllers = new List<SteamVR_TrackedObject>();

	void OnCollisionEnter(Collision _collision)
	{
		Debug.Log ("Collision entered" + _collision.collider.gameObject.name);
		AttachTo (_collision.collider.attachedRigidbody);
	}

	void OnTriggerEnter(Collider _collider)
	{

		Rigidbody controllerBody = _collider.attachedRigidbody;
		if (controllerBody == null)
			return;
		
		SteamVR_TrackedObject controller = controllerBody.gameObject.GetComponent<SteamVR_TrackedObject> ();

		if (ActiveControllers.Contains (controller) == false) {
			ActiveControllers.Add (controller);
		}

		AttachTo (_collider.attachedRigidbody);
	}

	void OnCollisionExit(Collision _collision)
	{
		HandleExit (_collision.collider.attachedRigidbody);
	}
	void OnTriggerExit(Collider _collider)
	{
		HandleExit (_collider.attachedRigidbody);
	}

	void HandleExit(Rigidbody _controllerBody)
	{
		
		if (_controllerBody == null)
			return;

		SteamVR_TrackedObject controller = _controllerBody.gameObject.GetComponent<SteamVR_TrackedObject> ();

		if (ActiveControllers.Contains (controller) == true) {
			ActiveControllers.Remove (controller);
		}
	}

	public void AttachTo(Rigidbody _controllerBody)
	{
		if (_controllerBody == null)
			return;
		SteamVR_TrackedObject controller = _controllerBody.gameObject.GetComponent<SteamVR_TrackedObject> ();


		if (controller == null)
			return;
		var device = SteamVR_Controller.Input ((int)controller.index);

		if (device.GetHairTrigger () == true && bAttached == false) {
			AttachTo (controller);
		} else {
		}
	}

	/// <summary>
	/// Sets the breakforce of the joint that connects the controller and the lever. Some circumstances create extreme forces and it is better to allow the connection to break
	/// An infinite force (unbreakable) joint causes the lever to disappear and other weird behavior.
	/// </summary>
	public float breakForces = 10;

	public void AttachTo(SteamVR_TrackedObject _controller)
	{
		// Trigger haptic feedback
		var device = SteamVR_Controller.Input ((int)_controller.index);
		device.TriggerHapticPulse (500, Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);

		AddNewJoint (_controller);
		
	}

	public void AddNewJoint(SteamVR_TrackedObject _controller)
	{

		JointObject = (Transform) Instantiate (HandleJointPrefab, HandlePosition.position, Quaternion.identity);
		JointObject.parent = transform;
		ConfigurableJoint cj = JointObject.GetComponent<ConfigurableJoint> ();
		cj.connectedBody = _controller.gameObject.GetComponent<Rigidbody>();

		FixedJoint fj = JointObject.GetComponent<FixedJoint> ();
		fj.connectedBody = transform.GetComponent<Rigidbody> ();

		AttachedController = _controller;

		bAttached = true;

	}

//	public void AddOldJoint(SteamVR_TrackedObject _controller)
//	{
//		// Set up the joint
//		HandJoint = gameObject.AddComponent<CharacterJoint>();
//		HandJoint.connectedBody = _controller.gameObject.GetComponent<Rigidbody>();
//		HandJoint.breakForce = breakForces;
//		HandJoint.breakTorque = breakForces;
//
//		_controller.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
//		AttachedController = _controller;
//
//		bAttached = true;
//	}
		
	void Update()
	{
		// Check if the controller has disconnected
		if (bAttached == true) {
			var device = SteamVR_Controller.Input ((int)AttachedController.index);

			if (device.GetHairTrigger () == false)
				Disconnect ();
		} else if (bAttached == false) { // If the controller is inside the collider and they press the trigger we want to know!
			foreach (SteamVR_TrackedObject controller in ActiveControllers) {
				var device = SteamVR_Controller.Input ((int)controller.index);	
				if (device.GetHairTrigger () == true) {
					AttachTo (controller);
				}
			}
		}
	}

	/// <summary>
	/// Disconnects the controller from the lever
	/// </summary>
	public void Disconnect ()
	{
		Destroy (JointObject.gameObject);

		AttachedController = null;
		bAttached = false;
	}
		
}
