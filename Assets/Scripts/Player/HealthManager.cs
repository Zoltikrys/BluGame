using System.Collections;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    [field: SerializeField] public int b_Health {get; set;} = 0;
    [field: SerializeField] public int StartingHP {get; set;} = 10;
    [field: SerializeField] public int MaximumHP {get; set;} = 10;
    [field: SerializeField] public int Lives{get; set;} = 3;

    [SerializeField]
    private float cooldownTime = 1.0f;
    private bool damageLock = false;

    public float m_DamageCooldown = 0;

    public float flashTime;
    public MeshRenderer meshRenderer;
    public Material material;
    public Animator animator;

    [field: SerializeField] public GameObject RenderTarget {get; set;}
    private BaseStatRenderer HPRenderer;

    [SerializeField] private AudioClip[] damageSoundClips;

    // Start is called before the first frame update
    void Start()
    {

        meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        //Needs to be sorted, doesnt tint the model when hit because of a new model and materials being used
        // material = meshRenderer.material;

        if(transform.gameObject.name == "Player") RenderTarget = GameObject.FindGameObjectWithTag("HP");
        if(RenderTarget) RenderTarget.TryGetComponent<BaseStatRenderer>(out HPRenderer);
        if(HPRenderer) HPRenderer.UpdateValues(b_Health, MaximumHP);
        
        animator = GetComponentInChildren<Animator>();
        if(animator == null) {
            animator = GetComponent<Animator>();
        }
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
        if(damageLock) return;

        if(m_DamageCooldown > 0)
        {
            return;
        }

        SoundFXManager.instance.PlayRandomSoundFXClip(damageSoundClips, transform, 1f);

        m_DamageCooldown = cooldownTime;
        b_Health -= 1;
        //Debug.Log(b_Health);

        //StartCoroutine(EFlash());

        if (b_Health <= 0)
        {
            Death();
        }
    }

    public void Damage(int damageValue)
    {
        if (damageLock) return;

        if (m_DamageCooldown > 0) {
            return;
        }

        //SoundFXManager.instance.PlayRandomSoundFXClip(damageSoundClips, transform, 1f);

        m_DamageCooldown = cooldownTime;
        b_Health -= damageValue;
        Debug.Log(b_Health);

        //StartCoroutine(EFlash());

        if (b_Health <= 0) {
            Death();
        }
    }

    public void Heal()
    {
        b_Health += 1;
        //Debug.Log(b_Health);

        if(b_Health > MaximumHP) b_Health = MaximumHP;

    }

    public void Death()
    {
        Debug.Log($"{name} died");
        damageLock = true;
        PlayDeathAnimation();
    }

    private void PlayDeathAnimation()
    {
        animator.SetBool("Dead", true);
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (CompareTag("Player")) {
            StartCoroutine("HandleRespawn", stateInfo);
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private IEnumerator HandleRespawn(AnimatorStateInfo stateInfo){
        if(CompareTag("Player")) transform.gameObject.GetComponent<PlayerController>().LockMovement();

        //while (!anim.GetCurrentAnimatorStateInfo(0).IsName("Death")) yield return null;

        yield return new WaitForSeconds(stateInfo.length);
    
        if(CompareTag("Player")){
            SceneManager sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManager>();
            if(Lives - 1 > 0){
                sceneManager.LifeLostRespawn();
            }
            else sceneManager.GameOver();
        }
        else{
            transform.gameObject.SetActive(false);
        }
    }

    // Broken with new animated character
    //IEnumerator EFlash()
    //{
    //    material.SetColor("_Tint", Color.red);
    //    yield return new WaitForSeconds(flashTime);
    //    material.SetColor("_Tint", Color.white);
    //}

    public void Respawn(){
        b_Health = MaximumHP;
    }
}
