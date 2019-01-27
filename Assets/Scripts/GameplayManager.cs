using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour {

    [Header("Configs")] [SerializeField] float DroneDelay = 5f;
    float DroneTimer = 0f;

    [SerializeField] float CarDelay = 5f;
    float CarTimer = 0f;

    [Header("Scene References")] [SerializeField]
    Transform DroneSpawnPoint1 = null;

    [SerializeField] Transform DroneSpawnPoint2 = null;
    [SerializeField] Transform DroneDropoffLine = null;


    [Header("Prefabs ect.")] [SerializeField]
    StuffManager stuffManager = null;

    [SerializeField] Drone dronePrefab = null;

    [SerializeField] CarSpawner CarSpawner = null;
    [SerializeField] House House = null;
    [SerializeField] CameraManager cameraManager = null;
    [SerializeField] Person person = null;
    [SerializeField] Explosion explosion = null;
    [SerializeField] Stuff[] startingThings;
    

    bool paused;

    int dronesRemaining = 0;


    void OnValidate() {
        AssertNotNull(DroneSpawnPoint1, nameof(DroneSpawnPoint1));
        AssertNotNull(DroneSpawnPoint2, nameof(DroneSpawnPoint2));
        AssertNotNull(DroneDropoffLine, nameof(DroneDropoffLine));
        AssertNotNull(stuffManager, nameof(stuffManager));
        AssertNotNull(dronePrefab, nameof(dronePrefab));
        AssertNotNull(CarSpawner, nameof(CarSpawner));
        AssertNotNull(House, nameof(House));
    }

    private void AssertNotNull(object value, string name) {
        Debug.Assert(value != null, string.Format("Missing '{0}'", name), this.gameObject);
    }

    void Awake() {
        cameraManager.followObject(person.gameObject);
    }

    void Start() {
        StartCoroutine(gameplayRoutine());
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            expandHouse();
        }
        
        UpdateDrones();
        UpdateCars();
    }

    void UpdateDrones() {
        if (dronesRemaining <= 0) {
            return;
        }
        
        DroneTimer += Time.deltaTime;

        if (DroneTimer < DroneDelay) {
            return;
        }

        DroneTimer -= DroneDelay;

        makeDrone();
        dronesRemaining--;
    }

    void makeDrone() {
        var stuff = stuffManager.makeRandomStuff();
        var dronePos = Vector2.Lerp(DroneSpawnPoint1.position, DroneSpawnPoint2.position, Random.Range(0f, 1f));
        var drone = Instantiate(dronePrefab, dronePos, Quaternion.identity);
        drone.deliverStuff(stuff, DroneDropoffLine.position.x);
        drone.setRemoveX(-200); // TODO set proper dynamic drone kill zone
    }

    void UpdateCars() {
        CarTimer += Time.deltaTime;

        if (CarTimer < CarDelay) {
            return;
        }

        CarTimer -= CarDelay;

        Car car = CarSpawner.SpawnCar();
        car.OnCollision = OnCarCollision;
    }

    private void OnCarCollision(Collision2D collision) {
        StartCoroutine(gameOverRoutine(collision));
    }



    IEnumerator gameplayRoutine() {
        // Starting part
        /*while (true) {
            yield return null;

            bool allMovedIn = true;
            var houseBounds = House.houseBounds;
            foreach (var stuff in startingThings) {
                if (!houseBounds.Contains(new Vector3(stuff.transform.position.x, stuff.transform.position.y, houseBounds.center.z) )) {
                    allMovedIn = false;
                }
            }

            if (allMovedIn && !person.isHolding) {
                Debug.Log("All stuff moved in!");
                break;
                
            }
        }*/


        while (true) {
            var level = 1;

            dronesRemaining = 10 + (level - 1) * 5;
            DroneDelay = 30 / dronesRemaining;
            
            // Level stuff
            while (dronesRemaining > 0) {
                yield return null;
            }
        
            yield return new WaitForSecondsRealtime(10f);

            yield return expandHouseRoutine();

        }        
    }
    
    
    
    
    
    [ContextMenu("expandHouse")]
    void expandHouse() {
        StartCoroutine(expandHouseRoutine());
    }

    IEnumerator expandHouseRoutine() {
        const float growSize = 10;
        
        pauseWorld(true);

        var houseBounds = House.houseBounds;
        houseBounds.Expand(growSize);
        var camHeight = houseBounds.extents.y;
        
        cameraManager.lerpTo(houseBounds.center, 2f);
        cameraManager.zoomToSize(camHeight, 2f);
        
        yield return new WaitForSecondsRealtime(3f);

        switch (Random.Range(0, 3)) {
            case 0: House.ExpandNorth(growSize); break;
            case 1: House.ExpandWest(growSize); break;
            case 2: House.ExpandSouth(growSize); break;
        }
        
        yield return new WaitForSecondsRealtime(1.5f);
        
        cameraManager.resetZoom(2f);
        cameraManager.lerpTo(person.transform.position, 2f);
        
        yield return new WaitForSecondsRealtime(3f);
        
        cameraManager.followObject(person.gameObject);
        pauseWorld(false);
    }

    IEnumerator gameOverRoutine(Collision2D collision) {
       var hitStuff = collision.collider.GetComponent<Stuff>();
        
        pauseWorld(true);
        
        
//        var houseBounds = House.houseBounds;
//        houseBounds.Expand(2f);
//        var camHeight = houseBounds.extents.y;
//        
//        cameraManager.lerpTo(houseBounds.center, 2f);
//        cameraManager.zoomToSize(camHeight, 2f);
//        
//        yield return new WaitForSecondsRealtime(3f);


        var bounds = hitStuff.GetComponent<SpriteRenderer>().bounds;
        var camTarget = bounds.center;
        bounds.Expand(bounds.extents.y * 0.2f);
        var camSize = bounds.extents.y;
        
        cameraManager.lerpTo(camTarget, 2f);
        cameraManager.zoomToSize(camSize, 2f);
        
        yield return new WaitForSecondsRealtime(3f);

        var currentZ = -1f;
        for (int i = 0; i < 10; i++) {
            var boom = Instantiate(explosion);

            if (i == 0) {

                var contact = collision.GetContact(0);
                var pos = contact.point;
                var newPos = new Vector3(pos.x, pos.y, currentZ);
                boom.transform.position = newPos;
                
                yield return new WaitForSecondsRealtime(0.5f);
                    
                
            } else {
                var pos = new Vector2(
                    Mathf.Lerp(bounds.min.x, bounds.max.x, Random.Range(0f, 1f)),
                    Mathf.Lerp(bounds.min.y, bounds.max.y, Random.Range(0f, 1f))
                );
                
                var newPos = new Vector3(pos.x, pos.y, currentZ);
                boom.transform.position = newPos;
                
                yield return new WaitForSecondsRealtime(0.1f);
            }

            boom.startExplosion();
            
            currentZ -= float.Epsilon;
        }
        
        yield return new WaitForSecondsRealtime(2f);

        SceneManager.LoadScene("Intro Scene");
    }
    

    void pauseWorld(bool pause) {
        Time.timeScale = pause ? 0 : 1;
    }

}
