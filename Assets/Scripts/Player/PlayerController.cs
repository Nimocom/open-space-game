using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityStandardAssets.ImageEffects;

public class PlayerController : MonoBehaviour 
{
    public static PlayerController inst;

    public Hull hull;
    public Character character;

    public bool isControlBlocked;

    [SerializeField]
    LineRenderer planetLineRenderer;

    [SerializeField]
    LayerMask itemContainerMask;
    [SerializeField]
    LayerMask targetMask;

    Transform currentTarget;
    Transform currentPlanet;

    Vector3 currentDirection;

    [SerializeField]    
    int activeWeaponSlot;

    bool isAutoRotatingEnabled;

    bool isPlanetReached;
    bool isRotating;
    bool isLanding;


    void Awake()
    {
        character = GetComponent<Character>();
        inst = this;
    }

	void Update ()
    {
        if (!isControlBlocked)
        {
            if (Input.GetKey(CKeys.attack) && hull.energyGenerator && hull.weapons[activeWeaponSlot])
                hull.weapons[activeWeaponSlot].Shoot();

            if (Input.GetKey(CKeys.launchRocket) && hull.weapons[3])
                hull.weapons[3].Shoot(currentTarget);
            
            if (hull.engine)
            if (isAutoRotatingEnabled || Input.GetKey(CKeys.rotateShip))
            {        
                isRotating = true;
                currentDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;     
            }

            if (Input.GetKeyDown(CKeys.autoRotateSwitch))
            {
                isAutoRotatingEnabled = !isAutoRotatingEnabled;
                UIManager.inst.autoRotationText.enabled = isAutoRotatingEnabled;
            }

            if (Input.GetKeyDown(CKeys.selectTarget))
            {
                Collider2D target;
                if(target = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.3f, targetMask))
                if (target.transform != currentTarget && target.transform.root != transform.root)
                {
                    currentTarget = target.transform.root;
                    UIManager.inst.SetTarget(target.transform.root.GetComponent<Hull>());
                }
                else
                {
                    currentTarget = null;
                    UIManager.inst.ResetTarget();
                }
                    
            }

            if (Input.GetKeyDown(CKeys.captureContainer))
            {
                Collider2D container;
                if (container = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.15f, itemContainerMask))
                {
                    if(Vector2.Distance(container.transform.position, transform.position) < 3f)
                        container.GetComponent<ItemContainer>().Capture(hull);
                }
            }

            if (Input.GetKeyDown(CKeys.landingMode))
            {
                GetNearestPlanet();
            }

            if(hull.engine)
            {
                if (Input.GetKey(CKeys.moveForward))
                    hull.engine.moveForward = true;
                if(Input.GetKey(CKeys.moveBack))
                    hull.engine.moveBack = true;
                if (Input.GetKey(CKeys.moveRight))
                    hull.engine.moveRight = true;
                if(Input.GetKey(CKeys.moveLeft))
                    hull.engine.moveLeft = true;
            }   

            if (Input.GetKeyDown(CKeys.weapOne))
                activeWeaponSlot = 0;
            if (Input.GetKeyDown(CKeys.weapTwo))
                activeWeaponSlot = 1;
            if (Input.GetKeyDown(CKeys.weapThree))
                activeWeaponSlot = 2;

            if (Input.GetKeyDown(CKeys.saveGame))
            {
                GameManager.SaveGame();
            }

            if (Input.GetKeyDown(CKeys.getNearestEnemy))
            {
                GetNearestEnemy();
            }

            if (Input.GetKeyDown(CKeys.communication))
            {
                if (currentTarget)
                {
                    currentTarget.GetComponentInChildren<AIController>().DialogueRequest();
                    isControlBlocked = true;
                    CameraController.inst.CenterToTarget(currentTarget);
                }
            }
        }


        if (Input.GetKeyDown(CKeys.inventorySwitch))
        {
            UIManager.inst.SwitchInventory();
            isControlBlocked = !isControlBlocked;
        }

        if (isLanding)
        {
            planetLineRenderer.SetPosition(0, transform.position);
            planetLineRenderer.SetPosition(1, (Vector2) currentPlanet.transform.position);
            if (Vector2.Distance(hull.transform.position, currentPlanet.transform.position) < 0.3f || isPlanetReached)
            {
                planetLineRenderer.enabled = false;
//                UIManager.inst.planetPoint.gameObject.SetActive(false);
                isPlanetReached = true;
                isControlBlocked = true;
                hull.transform.localScale = Vector3.Lerp(hull.transform.localScale, Vector3.zero, 6f * Time.deltaTime);
                if (hull.transform.localScale.magnitude <= 0.1f)
                {
                    isLanding = false;
                    GameManager.SaveGame();
                    SceneManager.LoadSceneAsync(2);
                }
            }
        }
	}

    void FixedUpdate()
    {
        if (isRotating)
        {
            hull.rigBody.AddTorque(Vector3.Cross(transform.right, currentDirection).z * hull.engine.rotationPower);
            isRotating = false;
        }
    }

    void GetNearestPlanet()
    {
        isLanding = !isLanding;
        if (isLanding)
        {
            float distance = 9999f;
            float currentDistance = 0f;
            foreach (var planet in GameManager.currentSystemPlanets)
            {
                currentDistance = Vector2.Distance(transform.position, planet.transform.position);
                if (currentDistance < distance)
                {
                    distance = currentDistance;
                    currentPlanet = planet.transform;
                    GameManager.SetActivePlanet(planet);
                }
            }
            planetLineRenderer.enabled = true;
        }
        else
        {
            planetLineRenderer.enabled = false;
//            UIManager.inst.planetPoint.gameObject.SetActive(false);
            currentPlanet = null;
        }
    }

    void GetNearestEnemy()
    {
        float distance = 9999f;
        float currentDistance = 0f;
        foreach (var player in GameManager.currentSystemPlayers)
        {
            if (character.GetRelation(player.fraction) <= -0.6f)
            {
                currentDistance = Vector2.Distance(transform.position, player.transform.position);
                if (currentDistance < distance)
                {
                    distance = currentDistance;
                    currentTarget = player.transform.root;
                }
            }
        }

        if(currentTarget)
        UIManager.inst.SetTarget(currentTarget.root.GetComponent<Hull>());
    }

    public void InitializeController(Hull hull)
    {
        this.hull = hull;
    }
}
