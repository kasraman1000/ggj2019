#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class StuffTesterEditor : Editor {
    StuffTester obj;

    void OnEnable() {
        obj = target as StuffTester;
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        foreach (var VARIABLE in ) {
            
        }
    }
}

public class StuffTester : MonoBehaviour {

    [SerializeField] StuffManager stuffManager;

    



}

#endif
