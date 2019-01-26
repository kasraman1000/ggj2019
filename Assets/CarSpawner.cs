using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CarSpawner : MonoBehaviour {
    public Car CarPrefab;

    public void OnValidate() {
        Debug.Assert(CarPrefab != null, this.gameObject);
    }

    public Car SpawnCar() {
        Car car = Instantiate(CarPrefab, transform.position, transform.rotation);
        return car;
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(CarSpawner))]
    public class CarSpawnerTester : Editor {

        CarSpawner CarSpawner;

        void OnEnable() {
            CarSpawner = target as CarSpawner;
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            if (!EditorApplication.isPlaying) {
                return;
            }

            GUILayout.Space(20);

            if (GUILayout.Button("Spawn")) {
                CarSpawner.SpawnCar();
            }
        }
    }

#endif
}
