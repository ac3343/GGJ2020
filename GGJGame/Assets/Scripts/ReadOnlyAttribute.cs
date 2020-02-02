using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ReadOnlyAttribute : PropertyAttribute
{

}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //Disabling the GUI so that I can modify it
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        //re-enabling the GUI so that the property will be shown in the UI
        GUI.enabled = true;
    }
}
#endif
