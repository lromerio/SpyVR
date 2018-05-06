using UnityEngine;
using System.Collections;

[System.Serializable]
public class Action<T> {
	// public settings
	public Object target;
	public string method;

	// inspector cache
	public string[] candidates = {};
	public int index;

	// invocation
	public System.Action<T> action;

	public void Awake() {
		action = System.Action<T>.CreateDelegate(typeof(System.Action<T>), target, target.GetType().GetMethod(method)) as System.Action<T>;
		//string s = typeof(UnityAction<int>);
	}

}

public class ActionAttribute : PropertyAttribute {
	public System.Type returnType;
	public System.Type[] paramTypes;
	public ActionAttribute(System.Type returnType = null, params System.Type[] paramTypes) {
		this.returnType = returnType != null ? returnType : typeof(void);
		this.paramTypes = paramTypes != null ? paramTypes : new System.Type[0];
	}

	public System.Delegate method;
}