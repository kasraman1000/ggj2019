#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StuffTester))]
public class StuffTesterEditor : Editor {
    StuffTester obj;

    void OnEnable() {
        obj = target as StuffTester;
    }

    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        GUILayout.Space(20);

        if (GUILayout.Button("Show all Stuff")) {
            clear();
            addStuffs(obj.stuffManager.stuffList, Vector2.zero, Vector2.right);
        }

        GUILayout.Space(20);

        GUILayout.Label("Show single stuff");
        foreach (var stuffTemplate in obj.stuffManager.stuffList) {
            if (GUILayout.Button(stuffTemplate.name)) {
                clear();
                addStuff(stuffTemplate, Vector2.zero);
            }
        }
    }

    void addStuffs(StuffTemplate[] stuffTemplates, Vector2 startingPosition, Vector2 spacing) {
        var pos = startingPosition;
        foreach (var template in stuffTemplates) {
            var newStuff = addStuff(template, pos);
            newStuff.transform.Translate(newStuff.GetComponent<SpriteRenderer>().bounds.extents * Vector2.right);
            pos += newStuff.GetComponent<SpriteRenderer>().bounds.size * Vector2.right;
            pos += spacing;
        }

        
    }

    Stuff addStuff(StuffTemplate stuffTemplate, Vector2 position) {
        var newStuff = obj.stuffManager.makeStuff(stuffTemplate);
        newStuff.transform.SetParent(obj.transform);
        newStuff.transform.position = position;
        return newStuff;
    }

    void clear() {
        foreach (var child in obj.GetComponentsInChildren<Transform>()) {
            if (child == obj.transform) {
                continue;
            }
            DestroyImmediate(child.gameObject);
        }
    }
}

public class StuffTester : MonoBehaviour {

    public StuffManager stuffManager;

    



}

#endif
