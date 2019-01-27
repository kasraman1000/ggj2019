using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed;
    [SerializeField]
    float scaleSpeed;
    [SerializeField]
    float acceleration;

    [ContextMenu("Start Explosion")]
    public void startExplosion() {
        StartCoroutine(explosionRoutine());
    }
    

    IEnumerator explosionRoutine() {
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        while (true) {

            transform.localScale = transform.localScale + transform.localScale * scaleSpeed * Time.unscaledDeltaTime;
            transform.Rotate(0, 0, rotationSpeed * Time.unscaledDeltaTime);

            
            rotationSpeed += acceleration * Time.unscaledDeltaTime;
//            scaleSpeed *= acceleration;
            
            yield return null;
        }
    }
}
