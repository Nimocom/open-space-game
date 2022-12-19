using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Weapon
{
    [SerializeField]
    ParticleSystem particlesLeft;
    [SerializeField]
    ParticleSystem particlesRight;

    [SerializeField]
    Transform impactEffect;

    [SerializeField]
    Color laserColor;

    [SerializeField]
    LayerMask layer;
    [SerializeField]
    float rayDistance;
    [SerializeField]
    float particlesPower;
    [SerializeField]
    float soundPitch;

    float timerLeft;
    float timerRight;

    bool isRayCasting;

    LineRenderer lRendLeft;
    LineRenderer lRenRight;
    RaycastHit2D leftHit;
    RaycastHit2D rightHit;
    RaycastHit2D curLeftHit;
    RaycastHit2D curRightHit;
    IDamagable curLeftTarget;
    IDamagable curRightTarget;

    Transform impLeft, impRight;
    ParticleSystem.EmissionModule emisModuleLeft;
    ParticleSystem.ShapeModule shapeModuleLeft;
    ParticleSystem.EmissionModule emisModuleRight;
    ParticleSystem.ShapeModule shapeModuleRight;

    void Awake()
    {
        emisModuleLeft = particlesLeft.emission;
        shapeModuleLeft = particlesLeft.shape;
        emisModuleRight = particlesRight.emission;
        shapeModuleRight = particlesRight.shape;
        lRendLeft = transform.GetChild(0).GetComponent<LineRenderer>();
        lRenRight = transform.GetChild(1).GetComponent<LineRenderer>();
        impLeft = Instantiate<Transform>(impactEffect);
        impRight = Instantiate<Transform>(impactEffect);
        impLeft.parent = transform;
        impRight.parent = transform;
        impLeft.gameObject.SetActive(false);
        impRight.gameObject.SetActive(false);
        lRendLeft.enabled = false; 
        lRenRight.enabled = false;
    }
	
    void Start()
    {
        lRendLeft.material.SetColor("_EmissionColor", laserColor);
        lRenRight.material.SetColor("_EmissionColor", laserColor);

        ParticleSystem.MainModule mainLeft = lRendLeft.GetComponent<ParticleSystem>().main;
        mainLeft.startColor = laserColor;
        ParticleSystem.MainModule mainRight = lRenRight.GetComponent<ParticleSystem>().main;
        mainRight.startColor = laserColor;
    }

    void Update()
    {
        if (isInstalled)
        {
            reloadTimer += Time.deltaTime;
            timerLeft += Time.deltaTime;
            timerRight += Time.deltaTime;

            if (isRayCasting && hull.energyGenerator.currentEnergy >= energyUsing)
            { 
                particlesLeft.transform.rotation = hull.leftSlot.rotation;
                particlesRight.transform.rotation = hull.rightSlot.rotation;
                ApplyDepreciation(266);
                audioSource.pitch = Mathf.Lerp(audioSource.pitch, soundPitch, 16f * Time.deltaTime);
                lRendLeft.SetPosition(0, hull.leftSlot.position);
                lRenRight.SetPosition(0, hull.rightSlot.position);
                lRendLeft.enabled = true;
                lRenRight.enabled = true;
                lRenRight.widthMultiplier = Mathf.Lerp(lRenRight.widthMultiplier, 0.01f, 12f * Time.deltaTime);
                lRendLeft.widthMultiplier = Mathf.Lerp(lRendLeft.widthMultiplier, 0.01f, 12f * Time.deltaTime);

                if (reloadTimer >= 0.03f)
                {
                    hull.energyGenerator.currentEnergy -= energyUsing;
                    reloadTimer = 0f;
                }

                if (leftHit = Physics2D.Raycast(hull.leftSlot.position, hull.leftSlot.right, rayDistance, layer))
                {
                    lRendLeft.SetPosition(1, leftHit.point);
                   
                    if (!impLeft.gameObject.activeSelf)
                        impLeft.gameObject.SetActive(true);
                    impLeft.position = leftHit.point;

                    if (leftHit.collider != curLeftHit.collider)
                    {
                        curLeftHit = leftHit;
                        curLeftTarget = curLeftHit.collider.GetComponent<IDamagable>() ?? null;
                    }
                    if (timerLeft >= 0.03f && curLeftTarget != null)
                    {
                        curLeftTarget.TakeDamage(hullDamage * efficiency, shieldDamage * efficiency, character);
                        timerLeft = 0f;
                    }

                    particlesLeft.transform.position = hull.leftSlot.position + (hull.leftSlot.right * leftHit.distance * 0.5f);
                    shapeModuleLeft.radius = leftHit.distance * 0.5f;
                    emisModuleLeft.rateOverTimeMultiplier = leftHit.distance * particlesPower;
                }
                else
                {
                    lRendLeft.SetPosition(1, hull.leftSlot.position + (hull.leftSlot.right * rayDistance));
                    if (impLeft.gameObject.activeSelf)
                        impLeft.gameObject.SetActive(false);
                    
                    particlesLeft.transform.position = hull.leftSlot.position + (hull.leftSlot.right * rayDistance * 0.5f);
                    shapeModuleLeft.radius = rayDistance * 0.5f;
                    emisModuleLeft.rateOverTimeMultiplier = rayDistance * particlesPower;
                }

                if (rightHit = Physics2D.Raycast(hull.rightSlot.position, hull.rightSlot.right, rayDistance, layer))
                {
                    lRenRight.SetPosition(1, rightHit.point);

                    if (!impRight.gameObject.activeSelf)
                        impRight.gameObject.SetActive(true);
                    impRight.position = rightHit.point;

                    if (rightHit.collider != curRightHit.collider)
                    {
                        curRightHit = rightHit;
                        curRightTarget = curRightHit.collider.GetComponent<IDamagable>() ?? null;
                    }
                    if (timerRight >= 0.03f && curRightTarget != null)
                    {
                        curRightTarget.TakeDamage(hullDamage * efficiency, shieldDamage * efficiency, character);
                        timerRight = 0f;
                    }

                    particlesRight.transform.position = hull.rightSlot.position + (hull.rightSlot.right * rightHit.distance * 0.5f);
                    shapeModuleRight.radius = rightHit.distance * 0.5f;
                    emisModuleRight.rateOverTimeMultiplier = rightHit.distance * particlesPower;
                }
                else
                {
                    lRenRight.SetPosition(1, hull.rightSlot.position + (hull.rightSlot.right * rayDistance));
                    if (impRight.gameObject.activeSelf)
                        impRight.gameObject.SetActive(false);

                    particlesRight.transform.position = hull.rightSlot.position + (hull.rightSlot.right * rayDistance * 0.5f);
                    shapeModuleRight.radius = rayDistance * 0.5f;
                    emisModuleRight.rateOverTimeMultiplier = rayDistance * particlesPower;
                }
                isRayCasting = false;
            }
            else
            {
                lRendLeft.enabled = false; 
                lRenRight.enabled = false;
                lRenRight.widthMultiplier = 0f;
                lRendLeft.widthMultiplier = 0f;
                audioSource.pitch = Mathf.Lerp(audioSource.pitch, 0f, 12f * Time.deltaTime);
                emisModuleLeft.rateOverTime = 0;
                emisModuleRight.rateOverTime = 0;
                if (impLeft.gameObject.activeSelf)
                    impLeft.gameObject.SetActive(false);
                if (impRight.gameObject.activeSelf)
                    impRight.gameObject.SetActive(false);
            }
        }
    }

    public override void Shoot()
    {
        isRayCasting = true;
    }
        
    public override void InstallModule(Hull hull, int slot)
    {
        base.InstallModule(hull, slot);
        particlesLeft.gameObject.SetActive(true);
        particlesRight.gameObject.SetActive(true);
        particlesLeft.transform.rotation = hull.leftSlot.rotation;
        particlesRight.transform.rotation = hull.rightSlot.rotation;
        emisModuleLeft.rateOverTime = 0;
    }

    public override void InstallModule(Hull hull)
    {
        base.InstallModule(hull, slotIndex);
        particlesLeft.transform.rotation = hull.leftSlot.rotation;
        particlesRight.transform.rotation = hull.rightSlot.rotation;
    }

    public override void DeinstallModule()
    {
        particlesLeft.gameObject.SetActive(false);
        particlesRight.gameObject.SetActive(false);
        impLeft.gameObject.SetActive(false);
        impRight.gameObject.SetActive(false);
        lRendLeft.enabled = false; 
        lRenRight.enabled = false;
        base.DeinstallModule();
    }

    public override List<string> GetSerializableData()
    {
        List<string> data = base.GetSerializableData();

        LaserData laserData = new LaserData()
        {
            particlesPower = this.particlesPower,
            soundPitch = this.soundPitch,
            laserColor = this.laserColor
        };

        data.Add(JsonUtility.ToJson(laserData));
        return data;
    }

    public override void SetSerializableData(List<string> data)
    {
        LaserData laserData = JsonUtility.FromJson<LaserData>(data[data.Count - 1]);

        particlesPower = laserData.particlesPower;
        soundPitch = laserData.soundPitch;
        laserColor = laserData.laserColor;

        data.Remove(data[data.Count - 1]);
        base.SetSerializableData(data);
    }

    public static Laser GenerateLaser(int level)
    {
        Color[] colorArray = new Color[]
        {
            Color.red,
            Color.green,
            Color.cyan,
            Color.white,
            Color.yellow,
            Color.magenta
        };
        
        Laser laser = Instantiate(DataBase.laser);

        float quality = Random.Range(1f, 2.6f);

        laser.weight = Random.Range(24f, 32f);
        laser.level = level;
        laser.cost = (int)(9200 * level * quality);

        laser.hullDamage = 1.48f * level * quality;
        laser.shieldDamage = 1.4f * level * quality;
        laser.energyUsing = 0.26f * level * quality;

        laser.particlesPower = 8.2f * level * quality;
        laser.soundPitch = Random.Range(2, 4.8f);
        laser.laserColor = colorArray[Random.Range(0, colorArray.Length)];

        return laser;
    }
}
