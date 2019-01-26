using UnityEditor;
using UnityEngine;

public class Person : MonoBehaviour {

    [Header("Configs")]
    [SerializeField]
    private float Speed = 1.0f;

    [SerializeField]
    private float PickupDistance = 1.0f;


    [Header("Gameobject references")] 
    
    [SerializeField]
    Transform stuffHoldingAnchor;
    
    Vector2 facingDir;

    Stuff holding;

    void OnValidate() {
        if (stuffHoldingAnchor == null) {
            Debug.LogError("no stuffholdingachor assigned", this);
        }
    }


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

        if (moveDir != Vector2.zero) {
            facingDir = moveDir;            
        }

        Debug.DrawLine(transform.position, transform.position + (Vector3) facingDir * PickupDistance);

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (holding != null) {
                // put sutff down
                putDownStuff();
                
            } else {
                // pick stuff up
                
                // step 1 find stuff :D

                int layerMask = LayerMask.GetMask("Stuff");
                
                var hit = Physics2D.Raycast(transform.position, facingDir, PickupDistance,
                    layerMask); // This is inefficient layermask stuff lul

                if (hit.collider != null) {
                    var stuff = hit.collider.gameObject.GetComponent<Stuff>();
                    
                    Debug.Assert(stuff != null);

                    pickUpStuff(stuff);
                }
            }
        }
    }

    private void pickUpStuff(Stuff stuff) {
        holding = stuff;

        stuff.transform.SetParent(stuffHoldingAnchor);
        stuff.transform.localPosition = Vector2.zero;
    }

    private void putDownStuff() {
        holding.transform.SetParent(null);
        holding.transform.position = transform.position + (Vector3) facingDir * PickupDistance;

        holding = null;
    }
}
