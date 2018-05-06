using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(Action<MonoBehaviour>))]
public class ActionDrawer : PropertyDrawer {
	const float rows = 3;

	public override void OnGUI (Rect pos, SerializedProperty properties, GUIContent label) {
		//Debug.Log(((attribute as ActionAttribute).paramTypes.Length).ToString());

		SerializedProperty targetProperty = properties.FindPropertyRelative("target");
		SerializedProperty methodNameProperty = properties.FindPropertyRelative("method"); 
		SerializedProperty candidateNamesProperty = properties.FindPropertyRelative("candidates");
		SerializedProperty indexProperty = properties.FindPropertyRelative("index");

		// pass through label
		EditorGUIUtility.LookLikeControls();

		// TODO Possibl fix for depricated thing
		//EditorGUIUtility.labelWidth = new Rect (pos.x, pos.y, pos.width / 2, pos.height / rows);

		EditorGUI.LabelField(
			new Rect (pos.x, pos.y, pos.width/2, pos.height/rows),
			label
		);

		// target + method section
		EditorGUI.indentLevel++;
		EditorGUI.BeginChangeCheck(); // if target changes we need to repopulate the candidate method lists

		// select target
		EditorGUI.PropertyField(
			new Rect (pos.x, pos.y += pos.height/rows, pos.width, pos.height/rows),
			targetProperty
		);
		if(targetProperty.objectReferenceValue == null) {
			return; // null objects have no methods - don't continue
		} 

		// polulate method candidate names
		string[] methodCandidateNames;
		if(EditorGUI.EndChangeCheck()) {
			// lets do some reflection work -> search, filter, collect candidate methods..
			methodCandidateNames = RepopulateCandidateList(targetProperty, candidateNamesProperty, indexProperty);
		}
		else {
			methodCandidateNames = new string [candidateNamesProperty.arraySize]; 

			int i = 0;
			foreach(SerializedProperty element in candidateNamesProperty) {
				methodCandidateNames[i++] = element.stringValue;
			}
		}

		// place holder when no candidates are available
		if(methodCandidateNames.Length == 0) {
			EditorGUI.LabelField (
				new Rect (pos.x, pos.y += pos.height/rows, pos.width, pos.height/rows),
				"Method",
				"none"
			);    
			return; // no names no game
		}

		// select method from candidates
		indexProperty.intValue = EditorGUI.Popup (
			new Rect (pos.x, pos.y += pos.height/rows, pos.width, pos.height/rows),
			"Method (" + targetProperty.objectReferenceValue.GetType().ToString() + ")",
			indexProperty.intValue,
			methodCandidateNames
		);

		methodNameProperty.stringValue = methodCandidateNames[indexProperty.intValue];
		EditorGUI.indentLevel--;
	}    

	public string[] RepopulateCandidateList(
		SerializedProperty targetProperty, 
		SerializedProperty candidateNamesProperty,
		SerializedProperty indexProperty
	) {
		System.Type type = targetProperty.objectReferenceValue.GetType();
		System.Type[] paramTypes = this.paramTypes;
		IList<MemberInfo> candidateList = new List<MemberInfo>();
		string[] candidateNames;
		int i = 0; 

		Debug.Log ("Candidate Criteria:");
		Debug.Log ("\treturn type:" + returnType.ToString());
		Debug.Log ("\tparam count:" + paramTypes.Length);    
		foreach(System.Type paramType in paramTypes)
			Debug.Log("\t\t" + paramType.ToString());

		type.FindMembers(
			MemberTypes.Method,
			BindingFlags.Instance | BindingFlags.Public,
			(member, criteria) => {
				Debug.Log("matching " + member.Name);
				MethodInfo method;
				if((method = type.GetMethod(member.Name, paramTypes)) != null && method.ReturnType == returnType) {
					candidateList.Add(method);
					return true;
				}
				return false;
			},
			null
		);

		// clear/resize/initialize storage containers
		candidateNamesProperty.ClearArray();
		candidateNamesProperty.arraySize = candidateList.Count;
		candidateNames = new string[candidateList.Count];

		// assign storage containers
		i = 0;
		foreach(SerializedProperty element in candidateNamesProperty) {
			element.stringValue = candidateNames[i] = candidateList[i++].Name;
		}

		// reset popup index
		indexProperty.intValue = 0;

		return candidateNames;
	}

	public System.Type returnType {
		get { return attribute != null ? (attribute as ActionAttribute).returnType : typeof(void); }
	}

	public System.Type[] paramTypes {
		get { 
			return (attribute != null && (attribute as ActionAttribute).paramTypes != null) ? (attribute as ActionAttribute).paramTypes : new System.Type[0];
		}
	}

	public System.Delegate method {
		get { return attribute != null ? (attribute as ActionAttribute).method : null; }
		set { (attribute as ActionAttribute).method = value; }
	}

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
		return base.GetPropertyHeight (property, label) * rows;
	}
}