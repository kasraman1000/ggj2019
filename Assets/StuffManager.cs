
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StuffManager : ScriptableObject {
    [SerializeField]
    StuffTemplate[] stuffList;
    
    [SerializeField]
    Stuff stuffPrefab;

    public Stuff makeRandomStuff() {
        var stuffToMake = stuffList[Random.Range(0, stuffList.Length-1)];
        
        var result = GameObject.Instantiate(stuffPrefab);
        
        // stuffToMake.sprite.pivot = new Vector2(stuffToMake.sprite.rect.width / 2, 0);
        
        result.GetComponent<SpriteRenderer>().sprite = stuffToMake.sprite;
        result.transform.localScale = stuffToMake.size;
        result.gameObject.AddComponent<PolygonCollider2D>();
        
        return result;
    }
}
