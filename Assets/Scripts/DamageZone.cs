using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public Collider damageCollider;
    public int damageAmount;
    public string targetTag;
    public List<Collider> colliderTargets = new List<Collider>();
    // Start is called before the first frame update
    void Start()
    {
        damageCollider.enabled = false;
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy") && !colliderTargets.Contains(other))
        {
            colliderTargets.Add(other);
            var Enemy = other.GetComponent<Enemy>();
            if(Enemy != null)
            {
                Enemy.TakeDamage(damageAmount); 
            }
        }
    }
    public void BeginAttack()
    {
        colliderTargets.Clear();
        damageCollider.enabled = true;
    }
    public void EndAttack()
    {
        colliderTargets.Clear();
        damageCollider.enabled = false;
    }
}
