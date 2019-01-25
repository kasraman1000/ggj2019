using UnityEngine;

public class Person : MonoBehaviour {

    [SerializeField]
    private float Speed = 1.0f;

    private void Start() {

    }

    private void Update() {
        var moveDir = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow)) {
            moveDir += Vector2.up;
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            moveDir += Vector2.down;
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            moveDir += Vector2.left;
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            moveDir += Vector2.right;
        }

        transform.position += (Vector3)moveDir * Speed * Time.deltaTime;
    }
}
