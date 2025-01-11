using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killZone : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private float offset;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player") {
            HealthManager healthMan = other.GetComponent<HealthManager>();
            healthMan.Death();
        }
    }

    void Update()
    {
        offset += Time.deltaTime * -0.1f;

        if (offset < 1f) {
            offset -= 1f;
        }
        if (offset < -1f) {
            offset += 1f;
        }

        mat.mainTextureOffset = new Vector2(0, offset);
    }
}
