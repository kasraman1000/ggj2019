using TMPro;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class House : MonoBehaviour {
    [SerializeField]
    private Wall North = null;

    [SerializeField]
    private Wall South = null;

    [SerializeField]
    private Wall West = null;

    [SerializeField]
    private Wall NorthEast = null;

    [SerializeField]
    private Wall SouthEast = null;

    [SerializeField]
    private Floor Floor = null;

    public void ExpandNorth(float units) {
        North.Move(Vector2.up * units);
        NorthEast.Grow(Vector2.up * units);
        West.Grow(Vector2.up * units);
        Floor.Grow(Vector2.up * units);
    }

    public void ExpandWest(float units) {
        North.Grow(Vector2.left * units);
        West.Move(Vector2.left * units);
        South.Grow(Vector2.left * units);
        Floor.Grow(Vector2.left * units);
    }

    public void ExpandSouth(float units) {
        South.Move(Vector2.down * units);
        SouthEast.Grow(Vector2.down * units);
        West.Grow(Vector2.down * units);
        Floor.Grow(Vector2.down * units);
    }

    public Bounds houseBounds {
        get {
            var bounds = new Bounds();

            bounds.Encapsulate(North.bounds);
            bounds.Encapsulate(South.bounds);
            bounds.Encapsulate(West.bounds);
            bounds.Encapsulate(NorthEast.bounds);
            bounds.Encapsulate(SouthEast.bounds);

            return bounds;
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(House))]
    public class HouseTester : Editor {

        House House;
        float ExpandUnits = 1.0f;

        void OnEnable() {
            House = target as House;
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            if (!EditorApplication.isPlaying) {
                return;
            }

            GUILayout.Space(20);

            GUILayout.Label("Expand");

            ExpandUnits = EditorGUILayout.FloatField("Units", ExpandUnits);

            if (GUILayout.Button("North")) {
                House.ExpandNorth(ExpandUnits);
            }

            if (GUILayout.Button("West")) {
                House.ExpandWest(ExpandUnits);
            }

            if (GUILayout.Button("South")) {
                House.ExpandSouth(ExpandUnits);
            }
        }
    }

#endif
}
