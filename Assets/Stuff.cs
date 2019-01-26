using UnityEngine;

public class Stuff : MonoBehaviour {
    private float GizmoRadius = 0.1f;

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(transform.position, GizmoRadius);

    }
}
