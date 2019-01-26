using System;
using UnityEngine;

public class Car : MonoBehaviour {
    [SerializeField]
    private float Speed = 1f;

    [SerializeField]
    private float EndY = -40f;

    public Action<Collision2D> OnCollision;

    // Update is called once per frame
    void Update() {
        var position = transform.position;

        position.y -= Speed * Time.deltaTime;
        if (position.y < EndY) {
            Destroy(this.gameObject);
        }

        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Assert(OnCollision != null, gameObject);
        OnCollision.Invoke(collision);
    }
}
