using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainer : MonoBehaviour, IDamagable
{
    public Module module;

    Vector3 point;
    Hull hull;

    [SerializeField]
    GameObject explosionPrefab;
    [SerializeField]
    float lerpSpeed;

    bool isCaptured;
    bool isMoving;

    float health;
    float timer;
	

	void Update () 
    {
        timer += Time.deltaTime;
        if (timer >= 30 && !isCaptured)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0, 0, 0), 12f * Time.deltaTime);
            if (transform.localScale.magnitude <= 0)
                Destroy(gameObject);
        }
        transform.Rotate(new Vector3(8f, -2f, 10f) * 3f * Time.deltaTime);

        if (isCaptured)
        {
            if (hull != null)
            {
                transform.position = Vector3.Lerp(transform.position, hull.gateWay.position, lerpSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, hull.gateWay.position) <= 0.1f)
                {
                    if (module.LoadToCargoHold(hull))
                        Destroy(gameObject);
                    else
                    {
                        hull = null;
                        isCaptured = false;   
                        timer = 0f;
                    }
                }
            }
            else
            {
                hull = null;
                isCaptured = false;   
                timer = 0f;
            }
        }
        else
        if (isMoving)
        {
                transform.position = Vector3.Lerp(transform.position, point, lerpSpeed * Time.deltaTime);
        }
	}

    public void Capture(Hull hull)
    {
        if (!isCaptured)
        {
            this.hull = hull;
            isCaptured = true;
        }
    }

    public  void TakeDamage(float hullDamage, float shieldDamage, Character character)
    {
        health -= hullDamage;
        if (health <= 0f)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void OnMouseEnter()
    {
        UIManager.inst.ShowInfoPanel(module.ToString());
    }

    void OnMouseExit()
    {
        UIManager.inst.CloseInfoPanel();
    }

    public void InitializeLoot(Module loot, bool move = false)
    {
        module = loot;
        module.transform.parent = transform;
        module.transform.position = transform.position;

        if (move)
        {
            isMoving = true;
            point = transform.position + (Vector3)Random.insideUnitCircle * 1.6f;
        }
    }

    public static ItemContainer GenerateContainer(Module.ModuleType type, int level)
    {
        return null;
    }
}
