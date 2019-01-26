using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class House : MonoBehaviour {
    [SerializeField]
    private Wall North;

    [SerializeField]
    private Wall South;

    [SerializeField]
    private Wall West;

    [SerializeField]
    private Wall NorthEast;

    [SerializeField]
    private Wall SouthEast;

    [SerializeField]
    private Floor Floor;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void ExpandNorth(float units) {
        North.Move(Vector2.up * units);
        NorthEast.Grow(Vector2.up * units);
        West.Grow(Vector2.up * units);
        Floor.Grow(Vector2.up * units);
    }

    void ExpandWest(float units) {
        North.Grow(Vector2.left * units);
        West.Move(Vector2.left * units);
        South.Grow(Vector2.left * units);
        Floor.Grow(Vector2.left * units);
    }

    void ExpandSouth(float units) {
        South.Move(Vector2.down * units);
        SouthEast.Grow(Vector2.down * units);
        West.Grow(Vector2.down * units);
        Floor.Grow(Vector2.down * units);
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
