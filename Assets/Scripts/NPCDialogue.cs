using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Linq;


public class NPCDialogue : MonoBehaviour {

    public bool canInteract = false;
    public GameObject BLU;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    //public Text dialogueText;
    public string[] dialogue;
    private int index;
    private bool finishedTyping;

    public float wordSpeed;

    [SerializeField] private GameObject player;

    //public GameObject canvas;

    // Start is called before the first frame update
    void Start() {
        BLU = GameObject.Find("Player");
        dialogueText.text = "";
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("In interact zone");
        if (other.tag == ("Player")) {
            canInteract = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == ("Player")) {
            canInteract = false;
            zeroText(); //probably superfluous
        }
    }


    // Update is called once per frame
    void Update() {
    }

    public void NPCInteract() {
        if (dialoguePanel.activeInHierarchy) {
            NextLine();
        }
        else {
            BLU.GetComponent<PlayerController>().canMove = false;
            dialoguePanel.SetActive(true);
            StartCoroutine(Typing());
        }
    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive (false);
        BLU.GetComponent<PlayerController>().canMove = true;
    }

    IEnumerator Typing()
    {
        finishedTyping = false;
        foreach(char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        finishedTyping = true;
    }

    public void NextLine()
    {
        if (finishedTyping)
        {
            if (index < dialogue.Length - 1)
            {
                index++;
                dialogueText.text = "";
                StartCoroutine(Typing());
            }
            else
            {
                zeroText();
            }
        }
    }
}
