using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    [Tooltip("Scene to load first")]
    [field: SerializeField] public LEVELS FirstLoad = LEVELS.NO_SCENE;

    [Tooltip("Current Scene loaded")]
    [field: SerializeField] public LEVELS CurrentScene = LEVELS.NO_SCENE;

    [Tooltip("The current active camera")]
    [field: SerializeField] public Camera CurrentCamera;
    [field: SerializeField] public uint RequestedSpawnPoint = 0;
    [field: SerializeField] public StateManager StateManager {get; set;}

    public uint RoomID { get; set; }

    [field: SerializeField] public GameObject Player;

    private bool respawning = false;

    //room ID


    void Start()
    {
        DontDestroyOnLoad(this);

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

        if(FirstLoad != LEVELS.NO_SCENE) {
            RequestLoadScene(FirstLoad, 0, 0);
        }
        else {
            Debug.LogError("No Scene given to load first. Please add a scene to load initially.");
            Application.Quit();
        }
    }

    private void LoadScene(LEVELS sceneID, string sceneName){
        if(CurrentScene != LEVELS.NO_SCENE && IsSceneLoaded(sceneName)) UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        CurrentScene = sceneID;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void RequestLoadScene(LEVELS scene, uint id, uint requestedSpawnpoint){
        string sceneName;
        RoomDirectory.StoredRooms.TryGetValue(scene, out sceneName);
        if(sceneName == ""){
            throw new System.Exception($"Tried accessing invalid scene ({scene}). Exiting...");
        }
        LockPlayer();
        if(Player) Player.GetComponent<RgbGoggles>().TurnGogglesOff();
        if(Player) StateManager.StorePlayerInfo(Player);
        else {
            Battery bat = new Battery();
            bat.CurrentBatteryCharge = 0;
            bat.MaxCharge = 0.0f;
            bat.MinCharge = 0.0f;
            StateManager.StorePlayerInfo(10, false, false, bat);
        }


        if(CurrentScene != LEVELS.NO_SCENE) StateManager.SetRoomState(sceneName);
        RequestedSpawnPoint = requestedSpawnpoint;
        RoomID = id;

        //if(CurrentCamera) //CurrentCamera.GetComponent<CameraController>().StartCameraTransitionEffect(CAMERA_EFFECTS.LEAVE_ROOM, () => LoadScene(scene));
        LoadScene(scene, sceneName);
    }

    public void Respawn(){
        respawning = true;
        Player.GetComponent<RgbGoggles>().TurnGogglesOff();
        StateManager.SetPlayerState(Player, StateManager.CurrentCheckPoint.PlayerInfo);
        StateManager.SetStateTracker(StateManager.CurrentCheckPoint.StateTracker);
        RequestLoadScene(StateManager.CurrentCheckPoint.scene, StateManager.CurrentCheckPoint.RoomID, StateManager.CurrentCheckPoint.SpawnPoint);
    }

    private void LockPlayer()
    {
        if(Player) Player.GetComponent<PlayerController>().LockMovement();
    }

    private void UnlockPlayer()
    {
        if(Player) Player.GetComponent<PlayerController>().UnlockMovement();
    }


    private bool IsSceneLoaded(string sceneName){
        bool foundScene = false;
        for(int i = 0; i <  UnityEngine.SceneManagement.SceneManager.sceneCount; i++){
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            if(scene.name == sceneName){
                foundScene = true;
                break;
            }
        }

        return foundScene;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        Debug.Log($"Loaded scene: {scene.name}");
        Player = GameObject.FindGameObjectWithTag("Player");

        StateManager.SetRoomState(scene.name);
        StateManager.SetPlayerState(Player, StateManager.PlayerInfo);

        SetSpawn(scene);
        SetCamera(scene);

        UnlockPlayer();
        respawning = false;
    }

    void OnDestroy(){
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void SetPlayer(Vector3 position, Quaternion rotation)
    {
        if(Player){ 
            Debug.Log($"Setting position to: {position}");
            CharacterController controller = Player.GetComponent<CharacterController>();
            controller.enabled = false;
            Player.transform.position = position;     
            Player.transform.rotation =  rotation;    
            controller.enabled = true; 
        }
    }

    private void SetSpawn(Scene scene)
    {
        GameObject newSpawnPoint = new GameObject();
        newSpawnPoint.transform.position = new Vector3(0,4,0);
        GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetSceneByName(scene.name).GetRootGameObjects();
        GameObject SpawnPoints = null;

        foreach(GameObject obj in rootObjects){
            if(obj.CompareTag("SpawnPoints")){
                SpawnPoints = obj;
                break;
            }
        }

        if(SpawnPoints){
            if(RequestedSpawnPoint >= SpawnPoints.transform.childCount){
                Debug.LogWarning("Requested spawn point larger than number of spawn points. Defaulting to index 0");
                RequestedSpawnPoint = 0;
            }
            if(SpawnPoints.transform.GetChild((int)RequestedSpawnPoint).gameObject) {
                newSpawnPoint = SpawnPoints.transform.GetChild((int)RequestedSpawnPoint).transform.gameObject;
                CheckPointable a;
                if(newSpawnPoint.TryGetComponent<CheckPointable>(out a)) {
                    if( (StateManager.CurrentCheckPoint.scene == LEVELS.NO_SCENE || a.isCheckpoint) && !respawning){
                        StateManager.SetCheckpoint(CurrentScene, RoomID, RequestedSpawnPoint);
                    }   
                }
            }
            else Debug.LogWarning($"Could not find spawn point {RequestedSpawnPoint}. Setting Player to {new Vector3()}");
        } 
        else{
            Debug.LogWarning("Could not find any spawn points in scene. Character will default to (0,0,0)");
        }

        Debug.Log($"Setting Player spawn to: {newSpawnPoint.transform.position}, {newSpawnPoint.transform.rotation}");

        SetPlayer(newSpawnPoint.transform.position, newSpawnPoint.transform.rotation);
    }

    private void SetCamera(Scene scene){
        GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetSceneByName(scene.name).GetRootGameObjects();
        foreach(GameObject obj in rootObjects){
            if(obj.CompareTag("MainCamera")){
                Debug.Log("Found Camera");
                CurrentCamera = obj.GetComponent<Camera>();
                // CameraController camController = CurrentCamera.GetComponent<CameraController>();
                // camController.WeightedFocalPoints.Add(new WeightedFocalPoint(Player, 1.0f));
                // camController.StartCameraTransitionEffect(CAMERA_EFFECTS.ENTER_ROOM, () => {});
                break;
            }
        }
    }
}