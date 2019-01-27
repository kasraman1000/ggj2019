using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour {

    float defaultZ;
    float defaultZoomLevel;
    Camera cam;
    
    GameObject followTarget;

    void Awake() {
        cam = GetComponent<Camera>();
        defaultZoomLevel = cam.orthographicSize;
        defaultZ = transform.position.z;
    }

    void Update() {
        if (followTarget != null) {
            var targetPos = followTarget.transform.position;
            targetPos.z = defaultZ;
            transform.position = targetPos;
        }
    }

    public void followObject(GameObject target) {
        followTarget = target;
    }

    public void lerpTo(Vector2 target, float duration) {
        followTarget = null;
        StartCoroutine(lerpRoutine(target, duration));
    }
    
    IEnumerator lerpRoutine(Vector2 target, float duration) {
        float timeElapsed = 0;
        var startPos = transform.position;
        
        while (timeElapsed < duration) {
            timeElapsed += Time.unscaledDeltaTime;

            Vector3 pos = Vector2.Lerp(
                startPos,
                target,
                Mathf.SmoothStep(0, 1, timeElapsed / duration)    
            );

            pos.z = defaultZ;
            transform.position = pos; 
            yield return null;
        }
    }
    
    public void zoomToSize(float size, float duration) {
        StartCoroutine(zoomRoutine(size, duration));
    }

    public void resetZoom(float duration) {
        StartCoroutine(zoomRoutine(defaultZoomLevel, duration));        
    }

    
    IEnumerator zoomRoutine(float targetSize, float duration) {
        float timeElapsed = 0;
        float startSize = cam.orthographicSize;
        
        while (timeElapsed < duration) {
            timeElapsed += Time.unscaledDeltaTime;

            cam.orthographicSize = Mathf.SmoothStep(
                startSize,
                targetSize,
                timeElapsed / duration
            );
            yield return null;
        }
    }

    [ContextMenu("Zoom Out Test")]
    public void zoomTest() {
        zoomToSize(20, 0.5f);
    }
}
