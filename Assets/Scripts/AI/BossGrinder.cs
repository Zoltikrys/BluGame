using UnityEngine;

public class BossGrinder : MonoBehaviour
{
    [field: SerializeField] public KnockbackEffect KnockbackEffect;
    public void OnTriggerEnter(Collider collider){
        Debug.Log("Collision");

        HealthManager healthManager;
        Rigidbody rigidbody;
        CharacterController characterController;
        collider.gameObject.TryGetComponent<HealthManager>(out healthManager);
        collider.gameObject.TryGetComponent<Rigidbody>(out rigidbody);
        collider.gameObject.TryGetComponent<CharacterController>(out characterController);

        if(healthManager){
            healthManager.Damage();
        }
        if(KnockbackEffect != null){
            if(rigidbody) KnockbackEffect.ApplyKnockback(rigidbody, transform.forward);
            if(characterController)KnockbackEffect.ApplyKnockback(characterController, transform.forward);
        }
    }
}
