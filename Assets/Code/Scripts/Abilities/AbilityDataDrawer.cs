// Purpose of this file is to have the Slow Duration option in the Inspector to appear only for Abilities of AbilityType = SlowAll
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbilityData))]
public class AbilityDataDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        // Draw standard fields
        EditorGUILayout.PropertyField(serializedObject.FindProperty("abilityName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("icon"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("unlockRound"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cooldownDuration"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("effectDuration"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("type"));
        
        // Only show slow multiplier if type is SlowAll
        var typeProperty = serializedObject.FindProperty("type");
        if ((AbilityData.AbilityType)typeProperty.enumValueIndex == AbilityData.AbilityType.SlowAll)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("slowMultiplier"));
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}
#endif