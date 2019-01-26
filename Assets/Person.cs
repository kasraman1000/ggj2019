using UnityEngine;

public class Person : MonoBehaviour {

    [Header("Configs")]
    [SerializeField]
    private float Speed = 1.0f;

    [SerializeField]
    private float PickupDistance = 1.0f;

    [SerializeField]
    private float ThrowVelocity = 1.0f;

    [SerializeField]
    private float PlayerRadius = 0.1f;

    [Header("Gameobject references")]

    [SerializeField]
    Transform StuffHoldingAnchor = null;

    Vector2 FacingDirection;

    Stuff Holding;

    void OnValidate() {
        if (StuffHoldingAnchor == null) {
            Debug.LogError("no stuffholdingachor assigned", this);
        }
    }


    private void Start() {

    }

    private void Move(Vector2 direction, float distance) {
        if (direction == Vector2.zero) {
            return;
        }

        Vector3 origin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance + PlayerRadius);

        if (hit.collider != null) {
            transform.position = hit.point - direction * PlayerRadius;
        } else {
            transform.position = origin + (Vector3)direction * distance;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(transform.position, PlayerRadius);
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)FacingDirection * PickupDistance);
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

        float distance = Speed * Time.deltaTime;
        Move(moveDir, distance);

        if (moveDir != Vector2.zero) {
            FacingDirection = moveDir;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (Holding != null) {


                if (moveDir == Vector2.zero) {
                    // put stuff down
                    putDownStuff();
                } else {
                    throwStuff();
                }

            } else {
                // pick stuff up

                // step 1 find stuff :D

                int layerMask = LayerMask.GetMask("Stuff");

                var hit = Physics2D.Raycast(transform.position, FacingDirection, PickupDistance,
                    layerMask); // This is inefficient layermask stuff lul

                if (hit.collider != null) {
                    var stuff = hit.collider.gameObject.GetComponent<Stuff>();

                    Debug.Assert(stuff != null);

                    pickUpStuff(stuff);
                }
            }
        }
    }

    private void throwStuff() {
        Holding.transform.SetParent(null);

        var body = Holding.GetComponent<Rigidbody2D>();

        Debug.Assert(body != null, Holding);

        var renderer = Holding.GetComponent<SpriteRenderer>();

        float offsetX = FacingDirection.x * (renderer.bounds.extents.x + PlayerRadius);
        float offsetY = FacingDirection.y * PlayerRadius;

        if (FacingDirection.y < 0) {
            offsetY -= renderer.bounds.extents.y * 2;
        } 
        /*
        else if (FacingDirection.y < 0) {
            offsetY += sprite.bounds.extents.y * 0,
        }
        */

        Vector3 offset = new Vector3(offsetX, offsetY);

        Holding.transform.position = transform.position + offset;
        body.velocity = ThrowVelocity * FacingDirection;

        var collider = Holding.GetComponent<Collider2D>();
        collider.enabled = true;

        Holding = null;
    }

    private void pickUpStuff(Stuff stuff) {
        Holding = stuff;

        var collider = Holding.GetComponent<Collider2D>();
        collider.enabled = false;

        var body = Holding.GetComponent<Rigidbody2D>();
        body.velocity = Vector2.zero;

        stuff.transform.SetParent(StuffHoldingAnchor);
        stuff.transform.localPosition = Vector2.zero;
    }



    private void putDownStuff() {
        Holding.transform.SetParent(null);
        Holding.transform.position = transform.position + (Vector3)FacingDirection * PickupDistance;

        var collider = Holding.GetComponent<Collider2D>();
        collider.enabled = true;

        Holding = null;
    }
}
