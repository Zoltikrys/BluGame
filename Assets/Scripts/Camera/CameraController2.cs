using System;
using System.Collections;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;          
    public Vector3 minWorldLimits;  
    public Vector3 maxWorldLimits; 

    void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogWarning("Player not assigned to camera controller.");
            return;
        }

        Vector3 targetPosition = player.position + offset;

        Vector3 clampedPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minWorldLimits.x, maxWorldLimits.x),
            Mathf.Clamp(targetPosition.y, minWorldLimits.y, maxWorldLimits.y),
            Mathf.Clamp(targetPosition.z, minWorldLimits.z, maxWorldLimits.z)
        );

        transform.position = clampedPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(
            (minWorldLimits + maxWorldLimits) / 2, 
            maxWorldLimits - minWorldLimits        
        );
    }
}