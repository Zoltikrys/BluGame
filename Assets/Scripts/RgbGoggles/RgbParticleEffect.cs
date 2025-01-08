using UnityEngine;

public class RgbParticleEffect : MonoBehaviour{
    [field: SerializeField] public Gradient RedGradient {get; set;}
    [field: SerializeField] public Gradient GreenGradient {get; set;}
    [field: SerializeField] public Gradient BlueGradient {get; set;}
    private RgbFilterObject rgbComponent;
    private ParticleSystem particleSystemComponent;
    private RGBSTATE colorState;
    private RGBSTATE prevState;
    private ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule;

    void Start(){
        if(TryGetComponent<RgbFilterObject>(out rgbComponent)) colorState = rgbComponent.FilterLayer;
        if(TryGetComponent<ParticleSystem>(out particleSystemComponent)) colorOverLifetimeModule = particleSystemComponent.colorOverLifetime;

        prevState = colorState;

        if(particleSystemComponent && rgbComponent){
            if(colorState == RGBSTATE.R) colorOverLifetimeModule.color = new ParticleSystem.MinMaxGradient(RedGradient);
            if(colorState == RGBSTATE.G) colorOverLifetimeModule.color = new ParticleSystem.MinMaxGradient(GreenGradient);
            if(colorState == RGBSTATE.B) colorOverLifetimeModule.color = new ParticleSystem.MinMaxGradient(GreenGradient);
        }
        particleSystemComponent.Stop();
        particleSystemComponent.Clear();
        particleSystemComponent.Play();

    }

    void Update(){
        if(particleSystemComponent && rgbComponent){
            colorState = rgbComponent.FilterLayer;
            if(colorState == RGBSTATE.R) colorOverLifetimeModule.color = new ParticleSystem.MinMaxGradient(RedGradient);
            else if(colorState == RGBSTATE.G) colorOverLifetimeModule.color = new ParticleSystem.MinMaxGradient(GreenGradient);
            else if(colorState == RGBSTATE.B) colorOverLifetimeModule.color = new ParticleSystem.MinMaxGradient(BlueGradient);
            else{
                particleSystemComponent.Stop();
                particleSystemComponent.Clear();
            }

            if(prevState != colorState){
                particleSystemComponent.Stop();
                particleSystemComponent.Clear();
                particleSystemComponent.Play();
                prevState = colorState;
            }
        }
    }
}