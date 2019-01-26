using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {

    [Header("Configs")]    
    public float delay = 5f;

    [Header("Scene References")]
    [SerializeField] Transform DroneSpawnPoint1;
    [SerializeField] Transform DroneSpawnPoint2;
    [SerializeField] Transform DroneDropoffLine;
    
    
    [Header("Prefabs ect.")]
    [SerializeField] StuffManager stuffManager;
    [SerializeField] Drone dronePrefab;
    
    
    float timer = 0;
    
    void OnValidate() {
        if (DroneSpawnPoint1 == null &&
            DroneSpawnPoint2== null &&
            DroneDropoffLine == null) {
            Debug.LogError("Missing Scene Reference", this);
        }

        if (stuffManager == null) {
            Debug.LogError("Missing stuffmanager", this);            
        }
        
        if (dronePrefab == null) {
            Debug.LogError("Missing drone prefab", this);            
        }
    }
    
    
    // Update is called once per frame
    void Update() {
        timer += Time.deltaTime;

        if (timer > delay) {
            timer -= delay;

            var stuff = stuffManager.makeRandomStuff();
            var dronePos = Vector2.Lerp(DroneSpawnPoint1.position, DroneSpawnPoint2.position, Random.Range(0f, 1f));
            var drone = Instantiate(dronePrefab, dronePos, Quaternion.identity);
            drone.deliverStuff(stuff, DroneDropoffLine.position.x);
            drone.setRemoveX(-200); // TODO set proper dynamic drone kill zone

        }
    }
}
