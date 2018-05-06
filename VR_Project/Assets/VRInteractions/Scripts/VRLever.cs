using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// Lever change event. Used for registering callbacks in the editor.
/// </summary>
[System.Serializable]
public class LeverChangeEvent : UnityEvent<VRLever, float, float> {}

/// <summary>
/// VR lever. Acts like a UI slider but is moved physically like a lever.
/// </summary>
public class VRLever : VRInteractable {

	/// <summary>
	/// Callbacks.
	/// </summary>
	public LeverChangeEvent LeverListeners;

	[Tooltip("Minimum angle in degrees, get's updated by and updates the joint if there is one")]
	public float Min = 0;

	[Tooltip("Maximum angle in degrees, get's updated by and updates the joint if there is one")]
	public float Max = 0;

	/// <summary>
	/// The current hinge.
	/// </summary>
	private HingeJoint CurrentHinge;

	//[SerializeField]
	protected float mValue = 0;
	protected float valueCache;

	public float Value
	{
		get { 
			return mValue;
		}
		set {
			if (mValue != value) {
				mValue = value;

				CheckHingeValue ();

				if (LeverListeners != null) { // execute callbacks
					try {	
						LeverListeners.Invoke (this, mValue, valueCache);
					} catch {
						Debug.LogError ("A delegate failed to execute for ValueChangedhandler in VRLever");
					}
				}
					
				valueCache = mValue; // Update value
			}
		}
	}
		
	/// <summary>
	/// Called after an update of value.
	/// This checks that the value and the hinge are the same. An external component can set value to set where the hinge should be.
	/// </summary>
	void CheckHingeValue()
	{
		if (isActing == false && Mathf.Round(ValueToAngle ()) != Mathf.Round(transform.rotation.eulerAngles.x)) {
			float valueToAngle = Mathf.Round (ValueToAngle ());
			float currentAngle = Mathf.Round(transform.rotation.eulerAngles.x);
			Debug.Log ("Setting value to angle value = " + valueToAngle + " cur angle " + currentAngle);
			SetAngleToValue ();
		}
	}

	private float mMinCache = 0;
	private float mMaxCache = 0;


	/// <summary>
	/// Called on reset in the editor, puts the cached values back to 0 so the component will accept the current hige joint values
	/// </summary>
	void Reset()
	{
		mMinCache = 0;
		mMaxCache = 0;

		Validate ();
	}

	void OnValidate ()
	{
		Validate ();
	}

	/// <summary>
	/// Called from editor, updates the hinge values
	/// </summary>
	void Validate (){
		CurrentHinge = GetComponent<HingeJoint> ();

		if (CurrentHinge != null)
			EditorUpdateHinges ();

	}

	/// <summary>
	/// Updates the hinge min and max values (how far the lever goes up and down). You can set this value on the hinge or
	/// this component.
	/// </summary>
	void EditorUpdateHinges()
	{

		if (CurrentHinge.limits.min != mMinCache) { // If the joint has been changed, update this component

			mMinCache = CurrentHinge.limits.min;
			Min = CurrentHinge.limits.min;

		} else if (mMinCache != Min) { // If the component has been changed, update the hinge
			mMinCache = Min;

			var limitCache = CurrentHinge.limits;
			limitCache.min = Min;
			CurrentHinge.limits = limitCache; 

		}

		if (CurrentHinge.limits.max != mMaxCache) { // If the joint has been changed, update this component

			mMaxCache = CurrentHinge.limits.max; 
			Max = mMaxCache;

		} else if (mMaxCache != Max) { // If the component has been changed, update the hinge
			mMaxCache = Max;

			var limitCache = CurrentHinge.limits;
			limitCache.max = Max;
			CurrentHinge.limits = limitCache; 

		}
	}
		
	void OnEnable()
	{
		// Take the current angle of the lever and set our slider value.
		SetAngleToValue ();
	}

	/// <summary>
	/// Sets the angle of the lever to the slider value (
	/// </summary>
	void SetAngleToValue()
	{
		Vector3 rotation = transform.rotation.eulerAngles;
		rotation.x = ValueToAngle ();
		Quaternion rot = transform.rotation;
		rot.eulerAngles = rotation;

		transform.rotation = rot;
	}

	/// <summary>
	/// Returns the angle of the lever as a value of the slider
	/// </summary>
	/// <returns>The to value.</returns>
	float AngleToValue()
	{
		float value = transform.rotation.eulerAngles.x > 180 ? transform.rotation.eulerAngles.x - 360 : transform.rotation.eulerAngles.x;
		//Debug.Log ("value = " + value);
		value = Mathf.Clamp(value, Min, Max);
		value += Min * Mathf.Sign (Min);
		return value / (Max + Min * Mathf.Sign (Min));
	}

	/// <summary>
	/// Converts the slider value to the lever angle based on the min and max hinge values
	/// </summary>
	/// <returns>The to angle.</returns>
	float ValueToAngle()
	{
		float angle = (((Max + Min * Mathf.Sign (Min)) * Value) - Min * Mathf.Sign (Min) + 360);
		angle = (angle >= 360) ? angle - 360: angle;
		return angle;
	}

	// TODO: This will be replaced with a haptic response object in the future
	/// <summary>
	/// The vibration strength.
	/// </summary>
	public float VibrationStrength = 0.2f;

	/// <summary>
	/// The trigger enabled.
	/// </summary>
	public bool triggerEnabled= false;

	new void Update()
	{
		base.Update ();

		if (Interactable == false)
			return;

		if (AngleToValue () != valueCache) { // Update the value if the lever has moved
			Value = AngleToValue ();
		}

		if (isActing == true) {
			controller.HapticVibration (VibrationStrength, Time.deltaTime); // If there is a controller attached provide haptic feedback.
		}
	}

	/// <summary>
	/// Controller that is currently aacting on the lever. Null if there is no actor.
	/// </summary>
	VRGripper controller;

	void OnCollisionEnter(Collision _collision)
	{
		if (_collision.rigidbody == null)
			return;

		controller = _collision.rigidbody.GetComponent<VRGripper> ();

		// If we have been moved by a vr object...
		if (controller == null)
			return;

		// Found gripper beginning actions
		BeginAction ();	 
	}

	void OnCollisionExit(Collision _collision)
	{
		//Debug.Log ("Attempting Exiting with " + _collision.gameObject.name);
		if (_collision.rigidbody == null)
			return;

		//Debug.Log ("Have rigidbody");
		VRGripper gripper = _collision.rigidbody.GetComponent<VRGripper> ();

		// If it is the same gripper...
		if (controller != gripper)
			return;

		// lost gripper ending actions
		EndAction ();

	}

	bool isActing = false;

	void BeginAction()
	{
		isActing = true;
	}

	void EndAction()
	{
		isActing = false;
	}

	public void OnGripBegin()
	{
		BeginAction ();
	}

	public void OnGripEnd()
	{
		EndAction ();
	}
}
