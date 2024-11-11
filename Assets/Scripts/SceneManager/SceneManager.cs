using System.Collections;
using BrewedInk.CRT;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    [Tooltip("Scene to load first")]
    [field: SerializeField]
    public SceneAsset FirstLoad;

    [Tooltip("Current Scene loaded")]
    [field: SerializeField]
    public SceneAsset CurrentScene;

    [Tooltip("The current active camera")]
    [field: SerializeField]
    public Camera CurrentCamera;

    [field: SerializeField]
    public uint RequestedSpawnPoint = 2;

    [field: SerializeField]
    public GameObject Player;

    void Start()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

        if(FirstLoad)LoadScene(FirstLoad, RequestedSpawnPoint);
        else {
            Debug.LogError("No Scene given to load first. Please add a scene to load initially.");
            Application.Quit();
        }
  

    }

    private void LoadScene(SceneAsset scene, uint requestedSpawnpoint){
        RequestedSpawnPoint = requestedSpawnpoint;

        if(CurrentScene && IsSceneLoaded(CurrentScene)) UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(CurrentScene.name);

        string sceneName = scene.name;
        CurrentScene = scene;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void RequestLoadScene(SceneAsset scene, uint requestedSpawnpoint){
        LockPlayer();

        if(CurrentCamera) CurrentCamera.GetComponent<CameraController>().StartCameraTransitionEffect(CAMERA_EFFECTS.LEAVE_ROOM, () => LoadScene(scene, requestedSpawnpoint));
        else LoadScene(scene, requestedSpawnpoint);
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
        if(GameObject.FindWithTag("Player")) UnlockPlayer();
        else Player = Instantiate(Player, new Vector3(), Quaternion.identity);
        SetSpawn(scene);
        SetCamera(scene);
        UnlockPlayer();

    }


    private void SetPlayer(Vector3 position)
    {
        if(Player){ 
            // TODO: Update this when we change the character controller
            Debug.Log($"Setting position to: {position}");
            CharacterController controller = Player.GetComponent<CharacterController>();
            controller.enabled = false;
            Player.transform.position = position;           
            controller.enabled = true; 
        }
    }

    private void SetSpawn(Scene scene)
    {
        Vector3 newSpawnPoint = new Vector3();
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
            if(SpawnPoints.transform.GetChild((int)RequestedSpawnPoint).gameObject) newSpawnPoint = SpawnPoints.transform.GetChild((int)RequestedSpawnPoint).position;
            else Debug.LogWarning($"Could not find spawn point {RequestedSpawnPoint}. Setting Player to {new Vector3()}");
        } 
        else{
            Debug.LogWarning("Could not find any spawn points in scene. Character will default to (0,0,0)");
        }

        Debug.Log($"Setting Player spawn to: {newSpawnPoint}");

        SetPlayer(newSpawnPoint);

        
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
