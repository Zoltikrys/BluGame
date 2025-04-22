using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class InteractText : MonoBehaviour {

    [SerializeField] private Transform cam;

    public PlayerInput PlayerInput;

    private void Awake() {
        cam = GameObject.Find("MainCamera").transform;
    }

    // Update is called once per frame
    private void Update() {
        gameObject.transform.LookAt(cam);
    }

    public void ShowText() {
        gameObject.SetActive(true);
    }

    public void HideText() {
        gameObject.SetActive(false);
    }
}
