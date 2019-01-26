using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField]
    private float Speed = 1.0f;
    [SerializeField]
    private float HoverFrequency = 1.0f;
    [SerializeField]
    private float HoverAmplitude = 1.0f;

    [Header("Gameobject references")]
    public Transform StuffHoldingAnchor;
    
    Stuff Holding;

    float baseY;
    float dropX;
    float removeX;

    float startTime;
    
    void OnValidate() {
        if (StuffHoldingAnchor == null) {
            Debug.LogError("no stuffholdingachor assigned", this);
        }
    }
    
    void Start() {
        baseY = transform.position.y;
        startTime = Time.time;
    }

    void Update() {
        var pos = transform.position;
        pos.x -= Speed * Time.deltaTime;
        pos.y = baseY + (HoverAmplitude * Mathf.Sin(HoverFrequency * Time.time - startTime));
        transform.position = pos;

        if (Holding != null && transform.position.x <= dropX) {
            dropStuff();
        }

        if (transform.position.x < removeX) {
            Destroy(this.gameObject);
        }
    }

    public void deliverStuff(Stuff stuff, float dropX) {
        Holding = stuff;

        Holding.GetComponent<Collider2D>().enabled = false;

        Holding.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        stuff.transform.SetParent(StuffHoldingAnchor);
        stuff.transform.localPosition = Vector2.zero + (stuff.GetComponent<SpriteRenderer>().size * Vector2.down);

        this.dropX = dropX;
    }

    public void setRemoveX(float removeX) {
        this.removeX = removeX;
    }

    void dropStuff() {
        Holding.transform.SetParent(null, true);

        Holding.GetComponent<Collider2D>().enabled = true;

        Holding = null;
    }
    
}
