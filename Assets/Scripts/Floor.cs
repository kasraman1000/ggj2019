using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Floor : MonoBehaviour {
    private SpriteRenderer Sprite;

    // Start is called before the first frame update
    void Start() {
        Sprite = GetComponent<SpriteRenderer>();
    }

    public void Grow(Vector2 delta) {
        Vector2 growth = new Vector2(
            Mathf.Abs(delta.x),
            Mathf.Abs(delta.y)
        );

        Sprite.size += growth;

        transform.position += (Vector3)delta * 0.5f;
    }

    public void Move(Vector2 delta) {
        transform.position += (Vector3)delta;
    }
}
