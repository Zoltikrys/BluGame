using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    [field: SerializeField] public int b_Health {get; set;} = 0;

    private bool damageBool = true;
    private bool newState = false;

    [SerializeField]
    private float cooldownTime = 1.0f;

    float m_DamageCooldown = 0;

    public float flashTime;
    Color originalColor;
    public MeshRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = gameObject.GetComponentInChildren<MeshRenderer>();
        originalColor = renderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_DamageCooldown > 0)
        {
            m_DamageCooldown -= Time.deltaTime;
        }
    }
    
    public void SetHealth(int amount){
        b_Health = amount;
    }
    public void Damage()
    {
        if(m_DamageCooldown > 0)
        {
            return;
        }

        m_DamageCooldown = cooldownTime;
        b_Health -= 1;
        Debug.Log(b_Health);
        damageBool = false;

        //StartCoroutine(EFlash());

        if (b_Health <= 0)
        {
            PlayerDeath();
        }

    }

    public void Heal()
    {
        b_Health += 1;
        Debug.Log(b_Health);

    }

    public void PlayerDeath()
    {
        Debug.Log("you died :(");
        PlayDeathAnimation();
        SceneManager sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>();
        sceneManager.Respawn();
    }

    private void PlayDeathAnimation()
    {
        Debug.LogWarning("No player death animation.");
    }

    void FlashStart()
    {
        renderer.material.color = Color.red;
        Invoke("FlashStop", flashTime);
    }

    void FlashStop()
    {
        renderer.material.color = originalColor;
    }

    IEnumerator EFlash()
    {
        renderer.material.color = Color.red;
        yield return new WaitForSeconds(flashTime);
        renderer.material.color = originalColor;
    }
    

}
