using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DangerousZone : MonoBehaviour
{
    IDamagable damagableObject;
//    public enum ZoneType
//    {
//        Heat,
//        Radiation
//    }
//      
//    public List<IDamagable> enteredObjects = new List<IDamagable>();
//    public float damage;
//    float timer;

    void OnTriggerEnter2D(Collider2D other)
    { 
        if ((damagableObject = other.GetComponent<IDamagable>()) != null)
            damagableObject.TakeDamage(10000f, 10000f, null);
        //enteredObjects.Add(other.GetComponent<IDamagable>());
    }

//    void OnTriggerExit2D(Collider2D other)
//    {
//        //enteredObjects.Remove(other.GetComponent<IDamagable>());
//    }

//    void Update()
//    { 
//        if (enteredObjects.Count > 0)
//        {
//            if ((timer += Time.deltaTime) >= 0.1f)
//            {
//                timer = 0f;
//                foreach (var obj in enteredObjects.ToList())
//                {
//                    obj.TakeDamage(damage, damage);
//                }
//            }
//        }
//    }
}
