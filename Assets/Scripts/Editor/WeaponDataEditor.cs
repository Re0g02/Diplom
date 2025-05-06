using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(WeaponDataScriptableObject))]
public class WeaponDataEditor : Editor
{
    WeaponDataScriptableObject weaponData;
    string[] weaponSubtypes;
    int selectedWeaponSubtype;

    void OnEnable()
    {
        weaponData = (WeaponDataScriptableObject)target;

        System.Type baseType = typeof(Weapon);
        List<System.Type> subTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => baseType.IsAssignableFrom(p) && p != baseType)
            .ToList();

        List<string> subTypesString = subTypes.Select(t => t.Name).ToList();
        subTypesString.Insert(0, "None");
        weaponSubtypes = subTypesString.ToArray();

        selectedWeaponSubtype = Math.Max(0, Array.IndexOf(weaponSubtypes, weaponData.Behaviour));
    }

    public override void OnInspectorGUI()
    {
        selectedWeaponSubtype = EditorGUILayout.Popup("Behaviour", Math.Max(0, selectedWeaponSubtype), weaponSubtypes);

        if (selectedWeaponSubtype > 0)
        {
            weaponData.Behaviour = weaponSubtypes[selectedWeaponSubtype].ToString();
            EditorUtility.SetDirty(weaponData);
            DrawDefaultInspector();
        }
    }
}
