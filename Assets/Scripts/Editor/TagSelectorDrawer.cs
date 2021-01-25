using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TagSelectorAttribute))]
public class TagSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.String)
        {
            Rect newPos = EditorGUI.PrefixLabel(position, label);
            property.stringValue = EditorGUI.TagField(newPos, property.stringValue);
        }
        else
        {
            base.OnGUI(position, property, label);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.String)
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
        else
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}
