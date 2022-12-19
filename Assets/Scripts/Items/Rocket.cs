using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour 
{
    public int rocketShellID;

    [SerializeField]
    GameObject impactEffect;

    [SerializeField]
    Transform trail;

    [SerializeField]
    LayerMask layer;

    [SerializeField]
    float rocketLifeTime;

    float rotationPower;
    float movementSpeed;
    float shieldDamage;
    float hullDamage;

    IDamagable damagable;
    Character character;
    RaycastHit2D hit;
    Transform target;

    Vector3 velocity;
    Vector3 newPos;
    Vector3 oldPos;    

    void Start () 
    {
        newPos = transform.position;
        oldPos = newPos;         
        velocity = movementSpeed * transform.right;
        Destroy(gameObject, rocketLifeTime);
    }

    void Update () 
    {        
        newPos += transform.right * velocity.magnitude * Time.deltaTime;
        Vector3 direction = newPos - oldPos;
        float distance = direction.magnitude;

        if (distance > 0) 
        {
            if((hit = Physics2D.Raycast(oldPos, direction, distance, layer)))
            {        
                hit.collider.GetComponent<IDamagable>().TakeDamage(hullDamage, shieldDamage, character);
                Instantiate(impactEffect, hit.point, Quaternion.identity);
                if(trail)
                trail.parent = null;
                Destroy(gameObject);
            }
        }

        if (target)
        {
            transform.right = Vector2.Lerp(transform.right, (target.position - transform.position).normalized, rotationPower * Time.deltaTime);
        }

        oldPos = transform.position;
        transform.position = newPos;       
    }
       

    public void InitializeRocket(float hullDamage, float shieldDamage, float speed, float rotSpeed, Transform target, Character player)
    {
        this.shieldDamage = shieldDamage;
        this.hullDamage = hullDamage;

        this.rotationPower = rotSpeed;
        this.movementSpeed = speed;
        this.target = target;

        character = player;
    }
}
