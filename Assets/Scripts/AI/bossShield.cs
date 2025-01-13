using UnityEngine;

public class bossShield : MonoBehaviour
{
    [SerializeField] public int powerSourceFlags = 0;
    [SerializeField] public int powerSourceTarget;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject mainMenu;
    private Animator anim;
    private Vector3 target;
    private GameObject player;
    [SerializeField] private GameObject bossEye;
    [SerializeField] private float bossHealth = 3f;
    [SerializeField] private GameObject bossMain;
    [SerializeField] private EnemySpawner[] enemySpawner;

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
    }

    public void TakeDamage()
    {
        if(anim.GetBool("EyeOpen") == true) {
            
            bossHealth--;

            if (bossHealth == 0) {
                DeathSequence();
            }
        }
    }

    public void DeathSequence()
    {
        foreach(var spawner in enemySpawner) {
            spawner.GetComponent<EnemySpawner>();
            spawner.enabled = false;
        }

        FlyingEnemyHunter[] others = (GameObject.FindObjectsOfType<FlyingEnemyHunter>());
        foreach (FlyingEnemyHunter other in others) { Destroy(other.gameObject); }

        gameObject.GetComponent<ParticleSystem>().Play();
        Animator bossMainAnim = bossMain.GetComponent<Animator>();
        bossMainAnim.SetBool("isDead", true);

        Boss boss = bossMain.GetComponent<Boss>();
        boss.Death();

    }

}
