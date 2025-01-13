using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject grinder1;
    public GameObject grinder2;
    public GameObject blockingwall;

    public void Death(){
        Destroy(grinder1);
        Destroy(grinder2);
        Destroy(blockingwall);
    }
}
