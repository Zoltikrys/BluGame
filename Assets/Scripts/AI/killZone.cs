using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killZone : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private float offset;

    [SerializeField] private float scrollSpeed = 0.5f;
    Renderer rend;

    private void OnTriggerEnter(Collider other)
    {
        rend = GetComponent<Renderer>();

        if(other.gameObject.name == "Player") {
            HealthManager healthMan = other.GetComponent<HealthManager>();
            healthMan.Damage(20);
        }
    }

    void Update()
    {
        offset += (scrollSpeed * Time.deltaTime);

        if (offset < 1f) {
            offset -= 1f;
        }
        if (offset < -1f) {
            offset += 1f;
        }

        mat.mainTextureOffset = new Vector2(offset, 0);
    }
}
