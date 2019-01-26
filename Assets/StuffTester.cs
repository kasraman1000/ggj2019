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

        foreach (var stuffTemplate in obj.stuffManager.stuffList) {
            if (GUILayout.Button(stuffTemplate.name)) {
                obj.stuffManager.makeStuff(stuffTemplate);
            }
        }
    }
}

public class StuffTester : MonoBehaviour {

    public StuffManager stuffManager;

    



}

#endif
