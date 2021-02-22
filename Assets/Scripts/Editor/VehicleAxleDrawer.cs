using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(VehicleAxle))]
public class VehicleAxleDrawer : PropertyDrawer
{
    private const float MINI_LABEL_WIDTH = 20f;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position = EditorGUI.PrefixLabel(position, label);

        Layout.Builder builder = new Layout.Builder();
        builder.PushChild(new LayoutChild(LayoutSize.Exact(MINI_LABEL_WIDTH)));
        builder.PushChild(new LayoutChild(LayoutSize.RatioOfRemainder(0.5f), LayoutMargin.Right(10f)));
        builder.PushChild(new LayoutChild(LayoutSize.Exact(MINI_LABEL_WIDTH)));
        builder.PushChild(new LayoutChild(LayoutSize.RatioOfRemainder(0.5f)));
        Layout layout = builder.Compile(position);

        EditorGUI.LabelField(layout.Next(), new GUIContent("L:"));
        EditorGUI.PropertyField(layout.Next(), property.FindPropertyRelative("left"), GUIContent.none);
        EditorGUI.LabelField(layout.Next(), new GUIContent("R:"));
        EditorGUI.PropertyField(layout.Next(), property.FindPropertyRelative("right"), GUIContent.none);
    }
}
