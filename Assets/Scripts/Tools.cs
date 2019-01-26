using System;
using UnityEngine;
using UnityEditor;

public class Tools : MonoBehaviour {
    [MenuItem("Custom/Generate Stuff Templates")]
    static void generateStuffTemplates() {
        var selectedGUIDS = Selection.assetGUIDs;

        foreach (var guid in selectedGUIDS) {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);

            Sprite loadedAsset = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);

            if (loadedAsset == null) {
                continue;
            }

            var template = ScriptableObject.CreateInstance<StuffTemplate>();

            template.sprite = loadedAsset;
            template.name = loadedAsset.name;

            var path = $"Assets/Stuff/{template.name}.asset";
            if (AssetDatabase.LoadAssetAtPath<StuffTemplate>(path) != null) {
                Debug.Log("Already exists");
                continue;
            }
            
            AssetDatabase.CreateAsset(template, path);            
        }
    }
}
