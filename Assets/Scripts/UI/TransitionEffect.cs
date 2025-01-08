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
        }
        
    }

    public IEnumerator Instant(float targetValue, float duration, Action callback){
        Color oldColor = image.material.color;
        oldColor.a = targetValue;
        image.material.color = oldColor;
        yield return null;

        callback?.Invoke();
    }

    public IEnumerator FadeToBlackTransition(float targetValue, float duration, Action callback){
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
        yield return null;
        callback?.Invoke();
    }
}