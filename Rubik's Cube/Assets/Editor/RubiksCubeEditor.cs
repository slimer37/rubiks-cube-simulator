using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RubiksCube))]
public class RubiksCubeEditor : Editor
{
    Rotation rubiksRotation;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RubiksCube selected = (RubiksCube)target;

        rubiksRotation = (Rotation)EditorGUILayout.EnumPopup("Rotation", rubiksRotation);

        if (!selected.turning)
        {
            if (GUILayout.Button("Turn Face"))
            {
                selected.Turn(rubiksRotation);
            }
        }
    }
}