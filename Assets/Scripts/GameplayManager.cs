using UnityEngine;

public class GameplayManager : MonoBehaviour {

    [Header("Configs")]
    [SerializeField] float DroneDelay = 5f;
    float DroneTimer = 0f;

    [SerializeField] float CarDelay = 5f;
    float CarTimer = 0f;

    [Header("Scene References")]
    [SerializeField] Transform DroneSpawnPoint1 = null;
    [SerializeField] Transform DroneSpawnPoint2 = null;
    [SerializeField] Transform DroneDropoffLine = null;


    [Header("Prefabs ect.")]
    [SerializeField] StuffManager stuffManager = null;
    [SerializeField] Drone dronePrefab = null;

    [SerializeField] CarSpawner CarSpawner = null;
    [SerializeField] House House = null;

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

    // Update is called once per frame
    void Update() {
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
        Debug.LogWarning("Car collided! OH NOES!!");
    }
}
