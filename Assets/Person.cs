using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour {

    float speed = 1.0f;
    
    void Start()
    {
        
    }

    void Update() {
        var moveDir = Vector2.zero;
        
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            moveDir = moveDir + Vector2.up;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            moveDir = moveDir + Vector2.down;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            moveDir = moveDir + Vector2.left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            moveDir = moveDir + Vector2.right;
        }

        transform.position += (Vector3) moveDir * speed;
    }
}
