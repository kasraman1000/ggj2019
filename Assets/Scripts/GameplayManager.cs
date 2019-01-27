using System.Collections;
using UnityEngine;

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

    bool paused;


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

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.A)) {
            expandHouse();
        }
        
        UpdateDrones();
        UpdateCars();
    }

    void UpdateDrones() {
        DroneTimer += Time.deltaTime;

        if (DroneTimer < DroneDelay) {
            return;
        }

        DroneTimer -= DroneDelay;

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
        var hitStuff = collision.collider.GetComponent<Stuff>();
        StartCoroutine(gameOverRoutine(hitStuff));
    }

    [ContextMenu("expandHouse")]
    void expandHouse() {
        StartCoroutine(expandHouseRoutine());
    }

    IEnumerator expandHouseRoutine() {
        const float growSize = 5;
        
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

    IEnumerator gameOverRoutine(Stuff hitStuff) {
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
        var camSize = bounds.extents.y;
        
        cameraManager.lerpTo(camTarget, 2f);
        cameraManager.zoomToSize(camSize, 2f);
        
        yield return new WaitForSecondsRealtime(3f);
        
        Debug.Log("BOOM!");
    }
    

    void pauseWorld(bool pause) {
        Time.timeScale = pause ? 0 : 1;
    }

}
