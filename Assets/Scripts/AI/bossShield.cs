using UnityEngine;

public class bossShield : MonoBehaviour
{
    [SerializeField] public int powerSourceFlags = 0;
    [SerializeField] public int powerSourceTarget;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Animator anim;
    [SerializeField] private Vector3 target;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bossEye;
    [SerializeField] private float bossHealth = 3f;
    [SerializeField] private GameObject bossMain;

    public void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
    }

    public void OpenSequence()
    {
        Debug.Log("omg its openinggg");
        anim.SetBool("EyeOpen", true);
    }

    private void Update()
    {
        target = player.transform.position;
        if(bossHealth > 0) bossEye.transform.LookAt(target);
        else bossEye.transform.LookAt(transform.forward);

        if (Input.GetKeyDown(KeyCode.G)) {
            DeathSequence();
        }
    }

    public void TakeDamage()
    {
        if(anim.GetBool("EyeOpen") == true) {
            
            bossHealth--;

            if (bossHealth <= 0) {
                DeathSequence();
            }
        }
    }

    public void DeathSequence()
    {
        Animator bossMainAnim = bossMain.GetComponent<Animator>();
        bossMainAnim.SetBool("Dead", true);

        Boss boss = bossMain.GetComponent<Boss>();
        boss.Death();

    }

}
