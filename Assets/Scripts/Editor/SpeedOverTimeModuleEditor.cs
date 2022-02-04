using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpeedOverTimeModule))]
public class SpeedOverTimeModuleEditor : Editor
{
    #region Editor Overrides
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty current = serializedObject.FindProperty("effectMagnitude");
        SerializedProperty applyForce = serializedObject.FindProperty(nameof(applyForce));
        
        // Iterate over all properties
        do
        {
            // Use special conditions for the force field
            if (current.name == "force")
            {
                // If we should apply a force then display the field
                if (applyForce.boolValue)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(current, true);
                    EditorGUI.indentLevel--;
                }
            }
            else EditorGUILayout.PropertyField(current, true);

        // Loop while a property exists after this one
        } while (current.Next(false));

        serializedObject.ApplyModifiedProperties();
    }
    #endregion
}
