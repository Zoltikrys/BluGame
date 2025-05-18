using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFade : MonoBehaviour
{
    [SerializeField] private CanvasGroup BlackOutSquare;

    [SerializeField] private bool fadeDone = false;
    [SerializeField] public bool startFade = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startFade)
        {
            if (BlackOutSquare.alpha < 1)
            {
                BlackOutSquare.alpha += (Time.deltaTime / 2);
                if(BlackOutSquare.alpha >= 1)
                {
                    fadeDone = true;
                    startFade = false;
                }
            }
        }
    }

    public void Fade()
    {
        BlackOutSquare.alpha = 1;
    }
}
