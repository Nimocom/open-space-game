using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shell : MonoBehaviour 
{
    public int shellID;

    public AudioClip fireSound;

    public float shieldDamageMlt;
    public float hullDamageMlt;
    public float relaodTime;
    public int poolCapacity;
    public float energyUsingMlt;

    IDamagable damagable;
    Character character;
    RaycastHit2D hit;


    Vector3 direction;
    Vector3 velocity;
    Vector3 newPos;
    Vector3 oldPos;    

    [SerializeField]
    Transform impactEffect;

    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    float shellLifeTime;
    [SerializeField]
    float shellSpeed;
    [SerializeField]
    float hullDamage;
    [SerializeField]
    float shieldDamage;

    float timer;

    void Awake () 
    {     
        impactEffect = Instantiate(impactEffect);
    }

    void OnEnable()
    {
        newPos = transform.position;
        oldPos = newPos;         
        velocity = shellSpeed * transform.right;
    }

    void Update () 
    {
        timer += Time.deltaTime;
        if (timer >= shellLifeTime)
        {
            timer = 0f;
            gameObject.SetActive(false);
        }

        
        newPos += transform.right * velocity.magnitude * Time.deltaTime;
        direction = newPos - oldPos;
        float distance = direction.magnitude;

        if (distance > 0) 
        {
            if((hit = Physics2D.Raycast(oldPos, direction, distance, layerMask)))
            {        
                hit.collider.GetComponent<IDamagable>().TakeDamage(hullDamage, shieldDamage, character);
                //Instantiate(impactEffect, hit.point, Quaternion.identity);
                impactEffect.position = hit.point;
                impactEffect.gameObject.SetActive(true);
                //Destroy(gameObject);
                timer = 0f;
                gameObject.SetActive(false);
            }
        }

        oldPos = transform.position;
        transform.position = newPos;  
    }

    public void InitializeShell(float hullDamage, float shieldDamage)
    {
        this.hullDamage = hullDamage;
        this.shieldDamage = shieldDamage;
    }

    public void InitializeShell(Character character)
    {
        this.character = character;
    }
}


