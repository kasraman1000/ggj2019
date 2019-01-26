using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(StuffManager))]
public class StuffManagerEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        GUILayout.Space(20);

        if (GUILayout.Button("Regenerate List")) {
            regenerateList();
        }
    }

    void regenerateList() {
        var assets = AssetDatabase.FindAssets("t:StuffTemplate", new[]{"Assets/Stuff"});

        var templates = new List<StuffTemplate>();
        
        foreach (var guid in assets) {
            var assetPath = AssetDatabase.GUIDToAssetPath(guid);

            StuffTemplate loadedAsset = AssetDatabase.LoadAssetAtPath<StuffTemplate>(assetPath);

            if (loadedAsset == null) {
                continue;
            }

            templates.Add(loadedAsset);
            
            
        }

        ((StuffManager) target).stuffList = templates.ToArray();
    }
}
    
#endif


[CreateAssetMenu]
public class StuffManager : ScriptableObject {
    public StuffTemplate[] stuffList = null;

    [SerializeField]
    Stuff stuffPrefab = null;

    public Stuff makeRandomStuff() {
        var stuffToMake = stuffList[Random.Range(0, stuffList.Length-1)];

        return makeStuff(stuffToMake);
    }

    public Stuff makeStuff(StuffTemplate stuffToMake) {
        var result = GameObject.Instantiate(stuffPrefab);
        result.GetComponent<SpriteRenderer>().sprite = stuffToMake.sprite;
        result.transform.localScale = result.transform.localScale * stuffToMake.sizeMultiplier;
        result.gameObject.AddComponent<PolygonCollider2D>();
        result.name = stuffToMake.name;

#if UNITY_EDITOR
        result.template = stuffToMake;
#endif

        return result;
    }
}
