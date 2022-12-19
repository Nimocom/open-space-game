using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertexer : Weapon
{
    [SerializeField]
    GameObject wavePref;
    [SerializeField]
    float waveRadius;
    [SerializeField]
    float distMlt;
    [SerializeField]
    LayerMask layerMask;

	void Update () 
    {
        if (isInstalled)
            reloadTimer += Time.deltaTime;
	}

    public override void Shoot()
    {
        if (reloadTimer >= reloadTime/efficiency && hull.energyGenerator.currentEnergy >= energyUsing)
        {
            ApplyDepreciation(16);
            reloadTimer = 0f;
            hull.energyGenerator.currentEnergy -= energyUsing;
            Instantiate(wavePref, hull.transform.position, hull.transform.rotation);
           
            foreach(var other in Physics2D.OverlapCircleAll(hull.transform.position, waveRadius, layerMask))
            {
                float dist = Vector2.Distance(transform.position, other.transform.position);
                other.GetComponent<IDamagable>().TakeDamage((hullDamage / dist) * distMlt, (shieldDamage / dist) * distMlt, character);
            }
        }
    }
}
