using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIController : MonoBehaviour
{
    public enum Status
    {
        Patrolling,
        Hunting,
        Charging,
        Escaping,
        Fighting
    }
         
    public Character character;
      
    public Hull hull;

    [SerializeField]    
    Transform currentTarget;

    [SerializeField]    
    LayerMask shipsLayer;

    [SerializeField]
    Status currentStatus;



    [SerializeField]
    string busyDialogueID;
    [SerializeField]
    string playerEnemyDialogueID;
    [SerializeField]    
    string freeDialogueID;

    float rocketLaunchingTimer;
    float maneuerTimer;
    float distance;

    bool isGettingBack;
    bool isRotating;
    bool isManuerInversed;
    bool isObstacleDetected; 
    bool isTargetInFront;

    int activeWeaponSlot;
    int randomInt;

    Vector3 currentDirection;
    Vector2 currentPoint;

    WaitForSeconds waitForSeconds = new WaitForSeconds(0.16f);

    System.Random random;

    void Awake()
    {
        character = GetComponent<Character>();
    }

    void Start()
    {
        hull = GetComponent<Character>().hull;
        random = new System.Random(gameObject.GetInstanceID());
        character.name = LocaManager.locaData.characterNames[random.Next(0, LocaManager.locaData.characterNames.Count)] + " " +
        LocaManager.locaData.characterSurnames[random.Next(0, LocaManager.locaData.characterSurnames.Count)];
        //activeWeaponSlot = 1;

        StartCoroutine(GetRandom());
        StartCoroutine(ChangeWeapon());
        GetPatrollingPoint();
        StartCoroutine(CheckObstacle());
        StartCoroutine(CheckForFriendlyFire());

        currentStatus = Status.Patrolling;
    }

    void Update()
    {
        if (!currentTarget)
            SetStatus(Status.Patrolling);
        
        switch (currentStatus)
        {
            case Status.Patrolling:
                GetNearestEnemy();
                if (currentTarget)
                    SetStatus(Status.Hunting);
                if (Vector2.Distance(transform.position, currentPoint) > 1f)
                    MoveTo(currentPoint);
                else
                    GetPatrollingPoint();
                break;

            case Status.Hunting:
                if ((distance = Vector2.Distance(transform.position, currentTarget.position)) > 15f)
                {
                    MoveTo(currentTarget.position);
                    GetNearestEnemy();
                }
                else
                {
                    SetStatus(Status.Fighting);
                }
                break;

            case Status.Fighting:
                
                if((distance = Vector2.Distance(transform.position, currentTarget.position)) > 20f)
                {
                    SetStatus(Status.Hunting);
                }
                else
                {
                    if ((hull.health * 100f / hull.maximumHealth) < 10f || (hull.energyGenerator.currentEnergy * 100f / hull.energyGenerator.maximumEnergy) < 10f)
                        SetStatus(Status.Escaping);
                    else
                        Attack();
                    // MoveBack();
                }
                break;

            case Status.Escaping:
                Maneuer();
                if ((distance = Vector2.Distance(transform.position, currentTarget.position)) < 16f)
                {
                    MoveBack();
                    //   if((hull.enerGen.currentEnergy * 100f / hull.enerGen.maximumEnergy) > 10f)
                    Shoot();
                }
                LookAtTarget(currentTarget.position);
                GetNearestEnemy();
                if ((hull.health * 100f / hull.maximumHealth) > 30f && (hull.energyGenerator.currentEnergy * 100f / hull.energyGenerator.maximumEnergy) > 30f)
                    SetStatus(Status.Patrolling);
                break;
//
//            case Status.Charging:
//                if (hull.enerGen.currentEnergy > 30f && hull.health > 30f)
//                    SetStatus(Status.Patrolling);
//                break;
        } 
    }



    void SetStatus(Status status)
    {
        currentStatus = status;
    }

    void GetPatrollingPoint()
    {
        currentPoint = new Vector2(Random.Range(-120f, 120f), Random.Range(-160f, 160f));
    }

    void FixedUpdate()
    {
        if (isRotating)
        {
            hull.rigBody.AddTorque(Vector3.Cross(hull.transform.right,  currentDirection).z * hull.engine.rotationPower);
            isRotating = false;
        }
    }

    void LookAtTarget(Vector2 target)
    {
        isRotating = true;
        currentDirection = (target - (Vector2)hull.transform.position).normalized;    
    }

    void Maneuer()
    {
        maneuerTimer += Time.deltaTime;
        if (maneuerTimer >= randomInt)
        {
            isManuerInversed = !isManuerInversed;
            maneuerTimer = 0f;
        }

        if (isManuerInversed)
            MoveLeft();
        else
            MoveRight();
    }

    void MoveForward()
    {
        if (!isObstacleDetected)
            hull.engine.moveForward = true;
        else
        {
            hull.engine.moveBack = true;
            MoveRight();
        }
    }

    void MoveRight()
    {
        if (!isObstacleDetected)
            hull.engine.moveRight = true;
        else
        {
            hull.engine.moveLeft = true;
            MoveBack();
        }
    }

    void MoveLeft()
    {
        if (!isObstacleDetected)
            hull.engine.moveLeft = true;
        else
        {
            hull.engine.moveRight = true;
            MoveBack();
        }
    }

    void MoveBack()
    {
        if (!isObstacleDetected)
            hull.engine.moveBack = true;
        else
        {
            hull.engine.moveLeft = true;
        }
    }

    void Attack()
    {
        // RaycastHit2D hit;
        LookAtTarget(currentTarget.position);
        //  if(((hit = Physics2D.CircleCast(transform.position + transform.right, 0.3f, transform.right, 10f)) && hit.transform.root == target) || !hit.transform)
        // curWeapon.Shoot();
        if(isTargetInFront)
        Shoot();
        if (distance > 5f)
            MoveForward();
        if (distance < 1f)
            MoveBack();
        else
            Maneuer();
    }

    void Shoot()
    {
        hull.weapons[activeWeaponSlot].Shoot();
        if (randomInt > 5 && hull.weapons[3])
            hull.weapons[3].Shoot(currentTarget);

    }

    void MoveTo(Vector2 newPoint)
    {
        LookAtTarget(newPoint);
        MoveForward();
    }

    IEnumerator ChangeWeapon()
    {
        while (true)
        {
            yield return new WaitForSeconds(random.Next(3, 12));
            int randomSlot = random.Next(1, 4);
            if (hull.weapons[randomSlot])
                activeWeaponSlot = randomSlot;
        }
    }

    void GetNearestEnemy()
    {
        float nearestDist = 9999f;
        float curDist = 0;

        foreach (var otherCharacter in GameManager.currentSystemPlayers)
        {
            if (character != otherCharacter && character.GetRelation(otherCharacter.fraction) < -0.1f)
            { 
                curDist = Vector2.Distance(transform.position, otherCharacter.transform.position);
                if (curDist <= nearestDist)
                {
                    nearestDist = curDist;
                    currentTarget = otherCharacter.transform.root;
                }
            }
        }
    }

    IEnumerator GetRandom()
    {
        while (true)
        {
            yield return new WaitForSeconds(randomInt = random.Next(2, 8));
        }
    }

    IEnumerator CheckObstacle()
    {
        while (true)
        {
            isObstacleDetected = Physics2D.CircleCast((Vector2) hull.transform.position + hull.rigBody.velocity.normalized, 0.42f, hull.rigBody.velocity, 1.2f, shipsLayer);
            yield return waitForSeconds;
        }
    }

    IEnumerator CheckForFriendlyFire()
    {
        RaycastHit2D hit;
        while(true)
        {
            hit = Physics2D.CircleCast(transform.position + transform.right, 0.42f, transform.right, 10f, shipsLayer);
            isTargetInFront = (!hit || hit.transform == currentTarget);  
            yield return waitForSeconds;
        }
    }

    public void DialogueRequest()
    {
        if (currentStatus == Status.Fighting || currentStatus == Status.Escaping)
        {
            if (currentTarget == PlayerController.inst.transform.root)
                DialogueManager.inst.StartDialogue(playerEnemyDialogueID);
            else
                DialogueManager.inst.StartDialogue(busyDialogueID);
        }
        else
            DialogueManager.inst.StartDialogue(freeDialogueID);
    }

    //    void OnDestroy()
    //    {
    //        SerializeManager.currentSystemPlayers.Remove(playerInformation);
    //    }
}
