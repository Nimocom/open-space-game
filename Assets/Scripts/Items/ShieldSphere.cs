using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSphere : MonoBehaviour,IDamagable
{
    Hull hull;

    void Awake()
    {
        hull = transform.root.GetComponent<Hull>();
    }

    public void TakeDamage(float hullDamage, float shieldDamage, Character character)
    {
        hull.shield.Reflect(shieldDamage);
    }
}
