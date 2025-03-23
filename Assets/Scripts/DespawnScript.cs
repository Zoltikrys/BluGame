using UnityEngine;
using UnityEngine.VFX;

public class DespawnScript : MonoBehaviour
{

    public float stompEffectTime = 0;
    public float stompEffectThreshold = 0.5f;
    public VisualEffect stompEffect;
    // Start is called before the first frame update
    void Start()
    {
        stompEffect.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (stompEffectTime >= stompEffectThreshold)
        {
            stompEffect.Stop();
            Destroy(gameObject, 2f);
        }
        else
        {
            stompEffectTime += Time.deltaTime;
        }
    }
}
