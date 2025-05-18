using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject grinder;
    public GameObject grinder2;
    public GameObject blockingwall;
    public ParticleSystem smokeParticle1;
    public ParticleSystem smokeParticle2;
    public doorBehaviour endDoor;

    public void Death(){
        Destroy(grinder);
        //Destroy(grinder2);
        Destroy(blockingwall);
        smokeParticle1.Stop();
        smokeParticle2.Stop();
        endDoor.IncreaseDoorStatus();
    }
}
