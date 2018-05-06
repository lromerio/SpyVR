using UnityEngine;
using System.Collections;

public class PistonLeverLink : MonoBehaviour {

	public VRLever Lever;

	private Piston CurrentPiston;



	void OnEnable()
	{
		//Lever.ValueChangedHandler += new VRLever.ValueChanged(LeverValueChange);

		CurrentPiston = GetComponent<Piston> ();


	}


	public void LeverValueChange(VRLever _lever,  float _newValue, float _oldValue)
	{
		CurrentPiston.Value = _newValue;	
	}

	public void LeverValueChange(VRLever _lever)
	{
		CurrentPiston.Value = _lever.Value;	
	}

	void OnDisable()
	{
		//Lever.ValueChangedHandler -= LeverValueChange;
	}



}
