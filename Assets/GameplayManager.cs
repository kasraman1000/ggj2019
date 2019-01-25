using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {

    
    public float delay = 5f;

    [SerializeField] StuffManager stuffManager;
    
    float timer = 0;
    
    // Update is called once per frame
    void Update() {
        timer += Time.deltaTime;

        if (timer > delay) {
            timer -= delay;

            var stuff = stuffManager.makeRandomStuff();
            
            

        }

        
        

    }
}
