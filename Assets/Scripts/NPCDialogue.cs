using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class NPCDialogue : MonoBehaviour
{

    public bool canInteract = false;
    public GameObject BLU;
    public GameObject d_template;
    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        BLU = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("In interact zone");
        if (other.tag == ("Player"))
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == ("Player"))
        {
            canInteract = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                //gameObject.SetActive(false);
                Debug.Log("NPC TALKING");
                //BLUReference.canMove;

                BLU.GetComponent<PlayerController>().canMove = false;
                //NewDialogue("HI");
                //NewDialogue("KILL YOURSELF");
                canvas.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
}
