using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(Stuff))]
public class StuffEditor : Editor{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        GUILayout.Space(20);

        if (GUILayout.Button("Go To Template")) {
            var template = ((Stuff) target).template;

            Selection.SetActiveObjectWithContext(template, template);
        }
    }
}
#endif

public class Stuff : MonoBehaviour
{
#if UNITY_EDITOR
    public StuffTemplate template;
    
#endif


}
