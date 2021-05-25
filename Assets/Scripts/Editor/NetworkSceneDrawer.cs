using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(NetworkScene))]
public class NetworkSceneDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Setup a position for only the foldout
        position = new Rect(position.x, position.y, position.width, LayoutUtilities.standardControlHeight);
        // Edit the foldout
        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
        // Move the position down by the height
        position.y += position.height;

        // If property is expanded, then continue with other properties
        if(property.isExpanded)
        {
            EditorGUI.indentLevel++;

            // Edit the scene name
            EditorGUI.PropertyField(position, property.FindPropertyRelative("name"));
            position.y += position.height;

            // Edit the "hasPlayer" boolean
            SerializedProperty hasPlayer = property.FindPropertyRelative("hasPlayer");
            EditorGUI.PropertyField(position, hasPlayer);
            position.y += position.height;

            // If this scene has a player in it, then edit the spawn point tag property
            if(hasPlayer.boolValue)
            {
                EditorGUI.PropertyField(position, property.FindPropertyRelative("spawnPointTag"));
            }

            EditorGUI.indentLevel--;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty hasPlayer = property.FindPropertyRelative("hasPlayer");
        float height = LayoutUtilities.standardControlHeight;

        // If property is expanded, add space for two more controls
        if(property.isExpanded)
        {
            height += (2f * LayoutUtilities.standardControlHeight);

            // If scene has player, add space for a third control
            if(hasPlayer.boolValue)
            {
                height += LayoutUtilities.standardControlHeight;
            }
        }

        return height;
    }
}
