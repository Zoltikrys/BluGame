using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    private int b_Health = 3;

    private bool damageBool = true;
    private bool newState = false;

    [SerializeField]
    private float cooldownTime = 1.0f;

    float m_DamageCooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_DamageCooldown > 0)
        {
            m_DamageCooldown -= Time.deltaTime;
        }
    }

    public void DamagePlayer()
    {
        if(m_DamageCooldown > 0)
        {
            return;
        }

        m_DamageCooldown = cooldownTime;
        b_Health -= 1;
        Debug.Log(b_Health);
        damageBool = false;

        if (b_Health <= 0)
        {
            PlayerDeath();
        }

    }

    public void HealPlayer()
    {
        b_Health += 1;
        Debug.Log(b_Health);

    }

    public void PlayerDeath()
    {
        Debug.Log("you died :(");
        Destroy(gameObject);
    }

}
