using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenedObject : MonoBehaviour{
    [field:SerializeField] public List<TweenDestination> tweenPositions = new List<TweenDestination>();
    public TweenDestination prevPosition;
    public TweenType type;
    public float DelayBeforeStarting = 1.0f;
    public int tweenIndex = 0;
    public bool paused = false;

    public bool ForwardsAndBackwards = false;


    void Start(){
        prevPosition = new TweenDestination();
        prevPosition.WaitAtDestinationTime = DelayBeforeStarting;
        prevPosition.PositionToHit = transform.position;
        prevPosition.RotationToHit = transform.rotation;

        StartCoroutine(StartTweeningList());
    }

    IEnumerator TweenToTarget(TweenDestination target){
        float elapsedTime = 0f;

        Vector3 startingPosition = transform.position;
        Quaternion startingRotation = Quaternion.Normalize(transform.rotation);

        while (elapsedTime < target.TimeToArrive)
        {
            if (!paused)
            {
                elapsedTime += Time.deltaTime;

                transform.position = Vector3.Lerp(startingPosition, target.PositionToHit, elapsedTime / target.TimeToArrive);
                transform.rotation = Quaternion.Lerp(startingRotation, Quaternion.Normalize(target.RotationToHit), elapsedTime / target.TimeToArrive);
            }
            yield return null;
        }

        transform.position = target.PositionToHit;
        transform.rotation = target.RotationToHit;

        yield return new WaitForSeconds(target.WaitAtDestinationTime);
}

    IEnumerator StartTweeningList(){
        yield return new WaitForSeconds(DelayBeforeStarting);

        bool forward = true;
        while(true){
            if(tweenPositions.Count <= 1) yield break; // no elements or no tween

            if(paused) yield return null;

            TweenDestination target =  tweenPositions[tweenIndex];

            yield return StartCoroutine(TweenToTarget(target));

            prevPosition = target;
            
            if(type == TweenType.FORWARDS_ONLY){
                tweenIndex += 1;
                if(tweenIndex >= tweenPositions.Count) tweenIndex = 0;
            }

            if(type == TweenType.BACKWARDS_ONLY){
                 tweenIndex -= 1;
                if(tweenIndex < 0) tweenIndex = tweenPositions.Count - 1;
            }
            if(type == TweenType.FORWARDS_AND_BACKWARDS){
                if(forward){
                    tweenIndex += 1;
                    if(tweenIndex >= tweenPositions.Count){
                        forward = false;
                        tweenIndex -= 1;
                    }
                }
                else{
                    tweenIndex -= 1;
                    if(tweenIndex < 0) {
                        tweenIndex += 1;
                        forward = true;
                    }
                }
            }

        }
    }
}

[Serializable]
public class TweenDestination{
    [field:SerializeField] public float TimeToArrive {get; set;}
    [field:SerializeField] public float WaitAtDestinationTime {get; set;}
    [field:SerializeField] public Vector3 PositionToHit {get; set;}
    [field:SerializeField] public Quaternion RotationToHit {get; set;}
}

