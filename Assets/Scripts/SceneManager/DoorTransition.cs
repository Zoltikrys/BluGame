using UnityEditor;
using UnityEngine;

public class DoorTransition : MonoBehaviour
{
    [field: SerializeField] public SceneAsset SceneToLoad;
    [field: SerializeField] public uint IndexOfSpawnPointInNextScene;
    [field: SerializeField] public uint SceneID;

    private SceneManager SceneManager;

    void Start(){
        SceneManager = GameObject.FindWithTag("SceneManager").GetComponent<SceneManager>();
        if(!SceneManager) Debug.LogWarning($"{name} door trigger point could not find SceneManager. Triggering this point will not switch scenes.");
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player")){
            if(SceneManager) SceneManager.RequestLoadScene(SceneToLoad, SceneID, IndexOfSpawnPointInNextScene);
        }
    }
}
