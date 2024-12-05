using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BrewedInk.CRT;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    [Tooltip("Scene to load first")]
    [field: SerializeField] public SceneAsset FirstLoad;

    [Tooltip("Current Scene loaded")]
    [field: SerializeField] public SceneAsset CurrentScene;

    [Tooltip("The current active camera")]
    [field: SerializeField] public Camera CurrentCamera;
    [field: SerializeField] public uint RequestedSpawnPoint = 0;

    public int RoomID { get; set; }

    [field: SerializeField] public GameObject Player;

    //room ID
    private Dictionary<int, RoomInfo> StateTracker = new Dictionary<int, RoomInfo>();

    void Start()
    {
        DontDestroyOnLoad(this);

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

        if(FirstLoad)LoadScene(FirstLoad);
        else {
            Debug.LogError("No Scene given to load first. Please add a scene to load initially.");
            Application.Quit();
        }
  

    }

    private void LoadScene(SceneAsset scene){


        if(CurrentScene && IsSceneLoaded(CurrentScene)) UnityEngine.SceneManagement.SceneManager.LoadScene(CurrentScene.name);

        string sceneName = scene.name;
        CurrentScene = scene;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void RequestLoadScene(SceneAsset scene, int id, uint requestedSpawnpoint){
        LockPlayer();
        RequestedSpawnPoint = requestedSpawnpoint;
        RoomID = id;

        if(CurrentCamera) CurrentCamera.GetComponent<CameraController>().StartCameraTransitionEffect(CAMERA_EFFECTS.LEAVE_ROOM, () => LoadScene(scene));
        else LoadScene(scene);
    }

    private void LockPlayer()
    {
        //TODO when we have new character controller
        if(Player) Player.GetComponent<CharacterController>();
    }

    private void UnlockPlayer()
    {
        //TODO when we have new character controller
        if(Player) Player.GetComponent<CharacterController>();
    }


    private bool IsSceneLoaded(SceneAsset sceneToCheck){
        bool foundScene = false;
        for(int i = 0; i <  UnityEngine.SceneManagement.SceneManager.sceneCount; i++){
            Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            if(scene.name == sceneToCheck.name){
                foundScene = true;
                break;
            }
        }

        return foundScene;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        Debug.Log($"Loaded scene: {scene.name}");
        SetRoomState(scene, RoomID);
        Player = GameObject.FindGameObjectWithTag("Player");
        SetSpawn(scene);
        SetCamera(scene);
        UnlockPlayer();
    }

    private void SetRoomState(Scene scene, int roomID)
    {
        if(StateTracker.ContainsKey(roomID)){
            
        }
        var trackedComponenets = scene.GetRootGameObjects()
                                    .SelectMany(s => scene.GetRootGameObjects())
                                    .Where(g => g.activeInHierarchy)
                                    .SelectMany(g => g.GetComponents<TrackedObject>())
                                    .ToList();
        foreach(var trackedComponent in trackedComponenets){
            Debug.Log($"TRACKED OBJECT {trackedComponent.name} found tracked");
        }
    }

    void OnDestroy(){
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void SetPlayer(Vector3 position, Quaternion rotation)
    {
        if(Player){ 
            // TODO: Update this when we change the character controller
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
        GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetSceneByName(CurrentScene.name).GetRootGameObjects();
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
            if(SpawnPoints.transform.GetChild((int)RequestedSpawnPoint).gameObject) newSpawnPoint = SpawnPoints.transform.GetChild((int)RequestedSpawnPoint).transform.gameObject;
            else Debug.LogWarning($"Could not find spawn point {RequestedSpawnPoint}. Setting Player to {new Vector3()}");
        } 
        else{
            Debug.LogWarning("Could not find any spawn points in scene. Character will default to (0,0,0)");
        }

        Debug.Log($"Setting Player spawn to: {newSpawnPoint.transform.position}, {newSpawnPoint.transform.rotation}");

        SetPlayer(newSpawnPoint.transform.position, newSpawnPoint.transform.rotation);

        
    }

    private void SetCamera(Scene scene){
        GameObject[] rootObjects = UnityEngine.SceneManagement.SceneManager.GetSceneByName(CurrentScene.name).GetRootGameObjects();
        foreach(GameObject obj in rootObjects){
            if(obj.CompareTag("MainCamera")){
                Debug.Log("Found Camera");
                CurrentCamera = obj.GetComponent<Camera>();
                CameraController camController = CurrentCamera.GetComponent<CameraController>();
                camController.WeightedFocalPoints.Add(new WeightedFocalPoint(Player, 1.0f));
                camController.StartCameraTransitionEffect(CAMERA_EFFECTS.ENTER_ROOM, () => {});
                break;
            }
        }
    }

}