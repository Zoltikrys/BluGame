using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Linq;
using System;


public class NPCDialogue : MonoBehaviour {

    public bool canInteract = false;
    private bool isTalking = false;

    public GameObject BLU;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    //public Text dialogueText;
    public string[] dialogue;
    private int index;
    private bool finishedTyping;

    public float wordSpeed;

    private Quaternion originalRotation;


    private GameObject player;
    [SerializeField] private Transform playerTransform;

    //public GameObject canvas;

    // Start is called before the first frame update
    void Start() {
        BLU = GameObject.Find("Player");
        dialogueText.text = "";
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("In interact zone");
        if (other.tag == ("Player")) {
            player = other.gameObject;
            player.GetComponent<PlayerLocomotionInput>().npc = gameObject;
            canInteract = true;
            other.transform.GetChild(1).gameObject.GetComponent<InteractText>().ShowText();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == ("Player")) {
            canInteract = false;
            zeroText(); //probably superfluous
            other.transform.GetChild(1).gameObject.GetComponent<InteractText>().HideText();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (isTalking)
        {
            FacePlayer();
        }
    }


    public void NPCInteract()
    {
        if (dialoguePanel.activeInHierarchy)
        {
            NextLine();
        }
        else
        {
            BLU.GetComponent<PlayerController>().canMove = false;
            dialoguePanel.SetActive(true);
            StartCoroutine(Typing());

            originalRotation = transform.rotation; // Save the original rotation
            isTalking = true; // Enable rotation
        }
    }



    private void FacePlayer()
    {
        Vector3 direction = playerTransform.position - transform.position;
        direction.y = 0; // Prevents tilting

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }


    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
        BLU.GetComponent<PlayerController>().canMove = true;
        isTalking = false; // Stop facing the player

        StartCoroutine(RotateBack()); // Start returning to original rotation
    }

    private IEnumerator RotateBack()
    {
        float rotationSpeed = 5f;
        float timeElapsed = 0f;
        float duration = 20f; // Adjust as needed

        while (timeElapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime * rotationSpeed;
            yield return null;
        }

        transform.rotation = originalRotation; // Ensure it ends exactly at the original rotation
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
