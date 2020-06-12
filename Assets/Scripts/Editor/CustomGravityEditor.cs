/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(customGravity))]
public class CustomGravityEditor : Editor
{
	SerializedProperty gravVec;
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		customGravity customG = (customGravity)target;
		if(customG.gravityType  == GravityType.Custom)
			EditorGUILayout.PropertyField(gravVec);
		serializedObject.ApplyModifiedProperties();
	}
	void OnEnable()
	{
		gravVec = serializedObject.FindProperty("gravityDir");
	}
}

*/