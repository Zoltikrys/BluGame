using UnityEngine;
using UnityEngine.VFX;

public class DespawnScript : MonoBehaviour
{

    public float vfxTime = 0;
    public float vfxThreshold = 0.5f;
    public VisualEffect vfxEffect;
    // Start is called before the first frame update
    void Start()
    {
        vfxEffect.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (vfxTime >= vfxThreshold)
        {
            vfxEffect.Stop();
            Destroy(gameObject, 2f);
        }
        else
        {
            vfxTime += Time.deltaTime;
        }
    }
}
