using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    [field: SerializeField] public int b_Health {get; set;} = 0;
    [field: SerializeField] public int StartingHP {get; set;} = 0;
    [field: SerializeField] public int MaximumHP {get; set;} = 10;

    [SerializeField]
    private float cooldownTime = 1.0f;

    float m_DamageCooldown = 0;

    public float flashTime;
    public MeshRenderer meshRenderer;
    public Material material;
    public Animator anim;

    [field: SerializeField] public GameObject RenderTarget {get; set;}
    private BaseStatRenderer HPRenderer;

    // Start is called before the first frame update
    void Start()
    {

        meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        material = meshRenderer.material;

        if(transform.gameObject.name == "Player") RenderTarget = GameObject.FindGameObjectWithTag("HP");
        if(RenderTarget) RenderTarget.TryGetComponent<BaseStatRenderer>(out HPRenderer);
        if(HPRenderer) HPRenderer.UpdateValues(b_Health, MaximumHP);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_DamageCooldown > 0)
        {
            m_DamageCooldown -= Time.deltaTime;
        }
        if(HPRenderer) HPRenderer.UpdateValues(b_Health, MaximumHP);
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

        StartCoroutine(EFlash());

        if (b_Health <= 0)
        {
            Death();
        }

    }

    public void Heal()
    {
        b_Health += 1;
        Debug.Log(b_Health);

        if(b_Health > MaximumHP) b_Health = MaximumHP;

    }

    public void Death()
    {
        Debug.Log($"{name} died");
        PlayDeathAnimation();


    }

    private void PlayDeathAnimation()
    {
        anim.Play("Death");
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        StartCoroutine("HandleRespawn", stateInfo);
    }

    private IEnumerator HandleRespawn(AnimatorStateInfo stateInfo){
        if(CompareTag("Player")) transform.gameObject.GetComponent<PlayerController>().LockMovement();

        //while (!anim.GetCurrentAnimatorStateInfo(0).IsName("Death")) yield return null;

        yield return new WaitForSeconds(stateInfo.length);
    
        if(CompareTag("Player")){
            SceneManager sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>();
            sceneManager.Respawn();
        }
        else{
            transform.gameObject.SetActive(false);
        }
    }

    void FlashStart()
    {
        material.SetColor("_Tint", Color.red);
        Invoke("FlashStop", flashTime);
    }

    void FlashStop()
    {
        material.SetColor("_Tint", Color.white);
    }

    IEnumerator EFlash()
    {
        material.SetColor("_Tint", Color.red);
        yield return new WaitForSeconds(flashTime);
        material.SetColor("_Tint", Color.white);
    }

    public void Respawn(){
        b_Health = StartingHP;
    }
}
