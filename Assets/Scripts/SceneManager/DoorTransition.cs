using UnityEngine;

public class DoorTransition : MonoBehaviour
{
    [field: SerializeField] public LEVELS SceneToLoad;
    [field: SerializeField] public uint IndexOfSpawnPointInNextScene;
    [field: SerializeField] public uint SceneID;
    [field: SerializeField] public CAMERA_TRANSITION_TYPE ExitTransition {get; set;}
    [field: SerializeField] public CAMERA_TRANSITION_TYPE EnterTransition {get; set;}

    private SceneManager SceneManager;

    void Start(){
        var sceneMan = GameObject.FindWithTag("SceneManager");
        if(!sceneMan) {
            Debug.Log("No game object with tag SceneManager found.");
            return;
        }
        sceneMan.TryGetComponent<SceneManager>(out SceneManager);
        if(!SceneManager) Debug.LogWarning($"{name} door trigger point could not find SceneManager Component of scenemanager. Triggering this point will not switch scenes.");
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player")){
            if(SceneManager) SceneManager.RequestLoadScene(SceneToLoad, SceneID, IndexOfSpawnPointInNextScene, ExitTransition, EnterTransition);
            else Debug.Log($"{name} triggered. No scene manager present.");
            
        }
    }
}
