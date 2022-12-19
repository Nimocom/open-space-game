using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Engine : Device
{
    Rigidbody2D rBody2D;

    public bool moveForward;
    public bool moveRight;
    public bool moveLeft;
    public bool moveBack;

    public float rotationPower;
    public float movementPower;


    [SerializeField]
    ParticleSystem trail;

    ParticleSystem.EmissionModule emission;
    ParticleSystem.MainModule main;


    public AudioSource aSource;

    void Awake()
    {
        trail = Instantiate<ParticleSystem>(trail, transform.position, Quaternion.identity, transform);
        main = trail.main;
        emission = trail.emission;
    }

    void LateUpdate()
    {
        if (isInstalled)
        {
            if (moveForward)
            {     
                aSource.pitch = Mathf.Lerp(aSource.pitch, 0.65f, 12f * Time.deltaTime);
                main.startSizeMultiplier = Mathf.Lerp(main.startSizeMultiplier, 0.18f, 5f * Time.deltaTime);
            }
            else if (moveBack)
            {                  
                aSource.pitch = Mathf.Lerp(aSource.pitch, 0.35f, 12f * Time.deltaTime);
                main.startSizeMultiplier = Mathf.Lerp(main.startSizeMultiplier, 0.12f, 5f * Time.deltaTime);
            }
            else if (moveRight || moveLeft)
            {                    
                aSource.pitch = Mathf.Lerp(aSource.pitch, 0.45f, 12f * Time.deltaTime);
                main.startSizeMultiplier = Mathf.Lerp(main.startSizeMultiplier, 0.06f, 5f * Time.deltaTime);
            }
            else
            {
                aSource.pitch = Mathf.Lerp(aSource.pitch, 0.25f, 12f * Time.deltaTime);
                main.startSizeMultiplier = Mathf.Lerp(main.startSizeMultiplier, 0.03f, 5f * Time.deltaTime);
            }
        }
        else
            aSource.pitch = Mathf.Lerp(aSource.pitch, 0f, 12f * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (isInstalled)
        {
            if (moveForward)
            { 
                rBody2D.AddForce(rBody2D.transform.right * (movementPower * efficiency));
                moveForward = false;
            }
            if (moveRight)
            {
                rBody2D.AddForce(-rBody2D.transform.up * (movementPower * efficiency) / 1.5f);
                moveRight = false;
            }
            if (moveLeft)
            {
                rBody2D.AddForce(rBody2D.transform.up * (movementPower * efficiency) / 1.5f);
                moveLeft = false;
            }
            if (moveBack)
            {
                rBody2D.AddForce(-rBody2D.transform.right * (movementPower * efficiency) / 1.8f);
                moveBack = false;
            }
        }
    }

    public override void InstallModule(Hull hull)
    {
        base.InstallModule(hull);
        aSource.enabled = true;
        hull.engine = this;
        rBody2D = hull.GetComponent<Rigidbody2D>();
        trail.transform.parent = hull.trailContainer;
        trail.transform.localPosition = Vector3.zero;
        trail.transform.rotation = hull.trailContainer.rotation;
        emission.enabled = true;

    }

    public override void DeinstallModule()
    {
        aSource.enabled = false;
        hull.engine = null;
        rBody2D = null;
        trail.transform.parent = transform;
        trail.transform.localPosition = Vector2.zero;
        emission.enabled = false;
        base.DeinstallModule();
    }

    public override string ToString()
    {
        return base.ToString() + LocaManager.textUnits["enginepower"] + movementPower.ToString("0.00") + "\n" +
        LocaManager.textUnits["rotspeed"] + rotationPower.ToString("0.00");
    }

    public override List<string> GetSerializableData()
    {
        List<string> serializedData = base.GetSerializableData();

        EngineData engineData = new EngineData()
        {
            movementPower = this.movementPower,
            rotationPower = this.rotationPower
        };

        serializedData.Add(JsonUtility.ToJson(engineData));
        return serializedData;
    }

    public override void SetSerializableData(List<string> data)
    {
        EngineData engineData = JsonUtility.FromJson<EngineData>(data[data.Count - 1]);

        movementPower = engineData.movementPower;
        rotationPower = engineData.rotationPower;

        data.Remove(data[data.Count - 1]);
        base.SetSerializableData(data);
    }

    public static Engine GenerateEngine(int level)
    {
        Engine engine = Instantiate(DataBase.engine);

        float quality = Random.Range(1f, 2.6f);

        engine.weight = Random.Range(26f, 32f);
        engine.level = level;
        engine.cost = (int)(4800 * level * quality);

        engine.movementPower = 420f * quality * level;
        engine.rotationPower = 380f * quality * level;

        return engine;
    }
}
