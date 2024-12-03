using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RgbFilterObject : MonoBehaviour
{
    [field: SerializeField] public RGBSTATE FilterLayer { get; set; }
    [field: SerializeField] public bool CollisionWhenHidden = false;
    [field: SerializeField] public bool ActiveOnColour = true;
    [field: SerializeField] public FadeStyle FadeInStyle = new FadeStyle(RGB_FILTER_FADE_STYLE.INSTANT_FADE, 0.0f, 0.0f, 1.0f, new RevealStyle());
    [field: SerializeField] public FadeStyle FadeOutStyle = new FadeStyle(RGB_FILTER_FADE_STYLE.INSTANT_FADE, 0.0f, 0.0f, 0.0f, new RevealStyle());

    public bool IsDisplayed;

    public bool Filterable = true;
    private MeshRenderer meshRenderer = null;
    private TextMeshPro textMesh = null;
    private Laser laser = null;

    void Start(){
        meshRenderer = transform.gameObject.GetComponent<MeshRenderer>();
        textMesh = transform.gameObject.GetComponent<TextMeshPro>();
        laser = transform.gameObject.GetComponent<Laser>();

        if(meshRenderer) Debug.Log("Found MeshRenderer on FilterObject");
        if(textMesh) Debug.Log("Found Textmesh on Filterobject");
        if(laser) Debug.Log("Found laser on FilterObject");

    }

    public void Hide()
    {
        StopAllCoroutines();
        switch (FadeOutStyle.Style)
        {
            case RGB_FILTER_FADE_STYLE.INSTANT_FADE:
                HandleInstantFade();
                HandleFadeExit(FadeOutStyle.RevealStyle);
                break;
            case RGB_FILTER_FADE_STYLE.SLOW_FADE:
                this.transform.gameObject.SetActive(true);
                StartCoroutine(HandleSlowFade(0.0f, FadeOutStyle.StartDelay, FadeOutStyle.FadeDuration, FadeOutStyle));
                break;
        }
    }
    public void Show()
    {
        StopAllCoroutines();
        switch (FadeInStyle.Style)
        {
            case RGB_FILTER_FADE_STYLE.INSTANT_FADE:
                HandleInstantShow();
                HandleFadeExit(FadeInStyle.RevealStyle);
                break;
            case RGB_FILTER_FADE_STYLE.SLOW_FADE:
                this.transform.gameObject.SetActive(true);
                StartCoroutine(HandleSlowFade(1.0f, FadeInStyle.StartDelay, FadeInStyle.FadeDuration, FadeInStyle));
                break;
        }
    }

    private void HandleInstantFade()
    {
        if (transform){
            MeshRenderer mesh;
            BoxCollider collider;
            transform.gameObject.TryGetComponent<MeshRenderer>(out mesh);
            transform.gameObject.TryGetComponent<BoxCollider>(out collider);

            if(mesh != null){
                mesh.enabled = false;
                if(mesh.material.HasProperty("_Color")){
                    Color newColor = mesh.material.color;
                    newColor.a = 0.0f;
                    mesh.material.color = newColor;
                }
            }
            

            if (collider != null)
            {
                if(CollisionWhenHidden)collider.enabled = true;
                else collider.enabled = false;
            }

            if(laser != null){
                Debug.Log("Line renderer hide");
                laser.SetLaserWidth(0.0f, 0.0f);
                if(CollisionWhenHidden) laser.isActive = true;
                else laser.isActive = false;
            }
        }
    }


    private void HandleInstantShow()
    {
        if (transform){
            MeshRenderer mesh;
            BoxCollider collider;
            transform.gameObject.TryGetComponent<MeshRenderer>(out mesh);
            transform.gameObject.TryGetComponent<BoxCollider>(out collider);

            if(mesh != null){
                mesh.enabled = true;
                if(mesh.material.HasProperty("_Color")){
                    Color newColor = mesh.material.color;
                    newColor.a = 1.0f;
                    mesh.material.color = newColor;
                }
            }

            if (collider) collider.enabled = true;

            if(laser != null){
                Debug.Log("Line renderer show");
                laser.SetLaserWidth(laser.WidthWhenLaserOn, laser.WidthWhenLaserOn);
                laser.isActive = true;
                
            }
        }
    }

    /// <summary>
    /// Starts a slow fade on the renderer for the given renderer (textmesh, meshrenderer, linerenderer)
    /// </summary>
    /// <param name="targetValue">the alpha to fade to</param>
    /// <param name="startDelay">Delay in seconds</param>
    /// <param name="fadeDuration">Fade duration in seconds</param>
    /// <param name="fadeStyle">fade specific settings</param>
    /// <returns></returns>
    private IEnumerator HandleSlowFade(float targetValue, float startDelay, float fadeDuration, FadeStyle fadeStyle)
    {
        yield return new WaitForSeconds(startDelay);


        float elapsedTime = 0.0f;
        Color targetTextMeshColor = new Color();
        Color oldTextMeshColor = new Color();
        if(textMesh != null){
            textMesh.enabled = true;
            oldTextMeshColor = textMesh.color;
            targetTextMeshColor = textMesh.color;
            targetTextMeshColor.a = targetValue;
        }

        Color targetMeshColor = new Color();
        Color oldMeshColor = new Color();
        if(meshRenderer && !textMesh){
            meshRenderer.enabled = true;
            oldMeshColor = meshRenderer.material.color;
            targetMeshColor = meshRenderer.material.color;
            targetMeshColor.a  = targetValue;
        }

        Vector2 oldLaserWidth = new Vector2();
        if(laser) {
            oldLaserWidth = laser.GetLaserWidth();
        }

        while(elapsedTime <=  fadeDuration){
            elapsedTime += Time.deltaTime;
            if(textMesh) textMesh.color = Color.Lerp(oldTextMeshColor, targetTextMeshColor, elapsedTime / fadeDuration);
            if (meshRenderer) meshRenderer.material.color = Color.Lerp(oldMeshColor, targetMeshColor, elapsedTime / fadeDuration);
            if (laser) {
                laser.SetLaserWidth(Mathf.Lerp(oldLaserWidth.x, targetValue * laser.WidthWhenLaserOn, elapsedTime/ fadeDuration), 
                                    Mathf.Lerp(oldLaserWidth.y, targetValue * laser.WidthWhenLaserOn, elapsedTime/ fadeDuration)
                );
            }


            // Change collision here
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            if(!CollisionWhenHidden && boxCollider){
                if(meshRenderer.material.color.a >= fadeStyle.CollisionThresholdPercentage) boxCollider.enabled = true;
                else boxCollider.enabled = false;
            }

            MeshCollider meshCollider = GetComponent<MeshCollider>();
            if(!CollisionWhenHidden && meshCollider){
                if(meshRenderer.material.color.a >= fadeStyle.CollisionThresholdPercentage) meshCollider.enabled = true;
                else meshCollider.enabled = false;
            }

            if( !CollisionWhenHidden && laser){
                Vector2 currentWidth = laser.GetLaserWidth();
                if(currentWidth.x >= fadeStyle.CollisionThresholdPercentage) laser.isActive = true;
                else laser.isActive = false;
            }
            
            
            yield return null;
        }
        HandleFadeExit(fadeStyle.RevealStyle);
        Debug.Log("SlowFade finished");
    }

    private void HandleFadeExit(RevealStyle revealStyle)
    {
        switch(revealStyle.Style){
            case RGB_FILTER_REVEAL_STYLE.DESTROY_AFTER: StartCoroutine(DestroyObjectAfterSeconds(revealStyle.DestroyDelay));
                                                        break;
            case RGB_FILTER_REVEAL_STYLE.ONE_SHOT: Filterable = false;
                                                   break;
            case RGB_FILTER_REVEAL_STYLE.CONTINUAL: break; // Do nothing, default implementation is continual.
        }
    }

    private IEnumerator DestroyObjectAfterSeconds(float destroyDelay)
    {
        yield return new WaitForSeconds(destroyDelay);
        DestroyImmediate(this);
    }
}

[Serializable]
public struct FadeStyle
{
    [SerializeField] public RGB_FILTER_FADE_STYLE Style;
    [SerializeField] public float FadeDuration;
    [SerializeField] public float StartDelay;
    [SerializeField] public float CollisionThresholdPercentage;
    [SerializeField] public RevealStyle RevealStyle;

    public FadeStyle(RGB_FILTER_FADE_STYLE requiredStyle, float requiredDuration, float requiredDelay, float collisionThreshold, RevealStyle revealStyle)
    {
        Style = requiredStyle;
        FadeDuration = requiredDuration;
        StartDelay = requiredDelay;
        CollisionThresholdPercentage = Math.Clamp(collisionThreshold, 0.0f, 1.0f);
        RevealStyle = revealStyle;
    }
}

[Serializable]
public struct RevealStyle
{
    [SerializeField] public RGB_FILTER_REVEAL_STYLE Style;
    [SerializeField] public float DestroyDelay;

    public RevealStyle(RGB_FILTER_REVEAL_STYLE style, float requiredDestroyDelay)
    {
        Style = style;
        DestroyDelay = requiredDestroyDelay;
    }

}

public enum RGB_FILTER_REVEAL_STYLE
{
    CONTINUAL,
    DESTROY_AFTER,
    ONE_SHOT,

}

public enum RGB_FILTER_FADE_STYLE
{
    INSTANT_FADE,
    SLOW_FADE
}
