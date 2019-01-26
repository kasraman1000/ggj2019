
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StuffManager : ScriptableObject {
    [SerializeField]
    StuffTemplate[] _stuffList;
    
    [SerializeField]
    Stuff stuffPrefab;

    public StuffTemplate[] stuffList => _stuffList;

    public Stuff makeRandomStuff() {
        var stuffToMake = _stuffList[Random.Range(0, _stuffList.Length-1)];

        return makeStuff(stuffToMake);
    }

    public Stuff makeStuff(StuffTemplate stuffToMake) {
        var result = GameObject.Instantiate(stuffPrefab);
        result.GetComponent<SpriteRenderer>().sprite = stuffToMake.sprite;
        result.transform.localScale = result.transform.localScale * stuffToMake.sizeMultiplier;
        result.gameObject.AddComponent<PolygonCollider2D>();
        result.name = stuffToMake.name;

#if UNITY_EDITOR
        result.template = stuffToMake;
#endif

        return result;
    }
}
