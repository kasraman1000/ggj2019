using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(SpriteRenderer))]
public class Highlight : MonoBehaviour {

    [SerializeField]
    private SpriteRenderer ReticlePrefab = default;

    [SerializeField]
    private Vector2 Padding = Vector2.one;

    private SpriteRenderer ReticleInstance;

    private void Start() {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        ReticleInstance = Instantiate(ReticlePrefab);
        
        Vector3 targetExtents = renderer.bounds.extents;

        Vector3 position = transform.position;
        position.y += targetExtents.y; // - Padding.y / 2;

        ReticleInstance.transform.position = position;
        ReticleInstance.transform.rotation = Quaternion.identity;

        ReticleInstance.transform.parent = transform;

        Vector3 objectExtends = ReticleInstance.bounds.extents;
        float sizeX = targetExtents.x / objectExtends.x + Padding.x;
        float sizeY = targetExtents.y / objectExtends.y + Padding.y;
        ReticleInstance.size = new Vector2(sizeX, sizeY);
        ReticleInstance.enabled = false;
    }

    public void SetHighlight(bool enabled) {
        ReticleInstance.enabled = enabled;
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(Highlight))]
    public class HighlightEditor : Editor {

        Highlight Highlight;

        void OnEnable() {
            Highlight = target as Highlight;
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            if (!EditorApplication.isPlaying) {
                return;
            }

            GUILayout.Space(20);

            GUILayout.Label("Highlight");

            if (GUILayout.Button("On")) {
                Highlight.SetHighlight(enabled: true);
            }

            if (GUILayout.Button("Off")) {
                Highlight.SetHighlight(enabled: false);
            }
        }
    }

#endif
}
