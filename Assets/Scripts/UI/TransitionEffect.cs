using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransitionEffect : MonoBehaviour{
    [field: SerializeField] public Image image;
    public float CircleRadius {get; set;} = 0.5f;
    public float Duration {get; set;} = 1.0f;
    public void StartCameraTransitionEffect(CAMERA_EFFECTS effect, CAMERA_TRANSITION_TYPE type, Action callback){

        if(image == null) {
            Debug.Log("CameraTransitionEffect -- Image was null, returning");
            callback?.Invoke();
            return;
        }

        switch(effect){
            case CAMERA_EFFECTS.ENTER_ROOM: RoomTransition(type, 0.0f, Duration, callback);
                                            break;
            case CAMERA_EFFECTS.LEAVE_ROOM: RoomTransition(type, 1.0f, Duration, callback);
                                            break;
        }
    }

    public void RoomTransition(CAMERA_TRANSITION_TYPE type, float targetValue, float duration, Action callback){

        switch(type){
            case CAMERA_TRANSITION_TYPE.NONE: StartCoroutine(Instant(targetValue, duration, callback));
                                              break;
            case CAMERA_TRANSITION_TYPE.FADE_TO_BLACK: StartCoroutine(FadeToBlackTransition(targetValue, duration, callback));
                                              break;
            case CAMERA_TRANSITION_TYPE.FADE_TO_CIRCLE: StartCoroutine(FadeToCircleTransition(targetValue, duration, callback));
                                              break;
            case CAMERA_TRANSITION_TYPE.SHIFT_LEFT: StartCoroutine(DirectionalShift(CAMERA_TRANSITION_TYPE.SHIFT_LEFT, targetValue, duration, callback));
                                              break;
            case CAMERA_TRANSITION_TYPE.SHIFT_RIGHT: StartCoroutine(DirectionalShift(CAMERA_TRANSITION_TYPE.SHIFT_RIGHT, targetValue, duration, callback));
                                              break;
            case CAMERA_TRANSITION_TYPE.SHIFT_UP: StartCoroutine(DirectionalShift(CAMERA_TRANSITION_TYPE.SHIFT_UP, targetValue, duration, callback));
                                              break;
            case CAMERA_TRANSITION_TYPE.SHIFT_DOWN: StartCoroutine(DirectionalShift(CAMERA_TRANSITION_TYPE.SHIFT_DOWN, targetValue, duration, callback));
                                              break;

        }
        
    }

    public IEnumerator DirectionalShift(CAMERA_TRANSITION_TYPE direction, float targetValue, float duration, Action callback){
        RectTransform rectTransform = image.rectTransform; 
        Vector3 screenSize = new Vector3(rectTransform.rect.width, rectTransform.rect.height, 0);   
        Vector3 startPosition = Vector3.zero; //center the fade
        Vector3 targetPosition = Vector3.zero; 
        Color color = image.material.color;
        color.a = 1.0f;
        image.material.color = color;

        // Each of these determine start and end points of the transition, Vector3(0,0,0) is the center of the screen.
        // Target value is the "alpha" to set it to, reused from the fade to black but is essentailly: 
        //                                                                                  0 for shift fade off screen (enter room)
        //                                                                                  1 for shift fade on screen  (leave room)
      
        switch (direction)
        {
            case CAMERA_TRANSITION_TYPE.SHIFT_LEFT:
                if (targetValue == 0)
                {
                    startPosition = new Vector3(0, 0, 0); 
                    targetPosition = new Vector3(-rectTransform.rect.width, 0, 0); 
                }
                else
                {
                    startPosition = new Vector3(rectTransform.rect.width, 0, 0); 
                    targetPosition = new Vector3(0, 0, 0); 
                }
                break;

            case CAMERA_TRANSITION_TYPE.SHIFT_RIGHT:
                if (targetValue == 0)
                {
                    startPosition = new Vector3(0, 0, 0); 
                    targetPosition = new Vector3(rectTransform.rect.width, 0, 0); 
                }
                else
                {
                    startPosition = new Vector3(-rectTransform.rect.width, 0, 0); 
                    targetPosition = new Vector3(0, 0, 0); 
                }
                break;

            case CAMERA_TRANSITION_TYPE.SHIFT_UP:
                if (targetValue == 0)
                {
                    startPosition = new Vector3(0, 0, 0); 
                    targetPosition = new Vector3(0, rectTransform.rect.height, 0);
                }
                else
                {
                    startPosition = new Vector3(0, -rectTransform.rect.height, 0);
                    targetPosition = new Vector3(0, 0, 0); 
                }
                break;

            case CAMERA_TRANSITION_TYPE.SHIFT_DOWN:
                if (targetValue == 0)
                {
                    startPosition = new Vector3(0, 0, 0);
                    targetPosition = new Vector3(0, -rectTransform.rect.height, 0); 
                }
                else
                {
                    startPosition = new Vector3(0, rectTransform.rect.height, 0); 
                    targetPosition = new Vector3(0, 0, 0); 
                }
                break;
        }

        rectTransform.anchoredPosition = startPosition;

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            rectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
        callback?.Invoke();
    }

    public IEnumerator Instant(float targetValue, float duration, Action callback){
        image.rectTransform.anchoredPosition = Vector3.zero;  //recenter image
        Color oldColor = image.material.color;
        oldColor.a = targetValue;
        image.material.color = oldColor;
        yield return null;

        callback?.Invoke();
    }

    public IEnumerator FadeToBlackTransition(float targetValue, float duration, Action callback){
        image.rectTransform.anchoredPosition = Vector3.zero;
        Color oldColor = image.material.color;
        float startValue = oldColor.a;
        float time = 0f;

        Color newColor = oldColor;
        while(time < duration){
            newColor.a = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            image.material.color = newColor;
            yield return null;
        }

        callback?.Invoke();
    }

    public IEnumerator FadeToCircleTransition(float targetValue, float duration, Action callback){
        image.rectTransform.anchoredPosition = Vector3.zero;
        yield return null;
        callback?.Invoke();
    }
}