using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Device
{
    public float maximumShield;
    public float currentShield;

    [SerializeField]
    float recoveryPower;
    [SerializeField]
    float activationTime;

    [SerializeField]
    Color color;

    float timer;

    GameObject shieldShpere;
    Renderer shieldRenderer;

    bool isNotActive;

	void Update ()
    {
        if (isInstalled && !isNotActive)
        {
            color.a -= Time.deltaTime * 2.5f;
            timer += Time.deltaTime;

            if (timer >= 0.16f && currentShield < maximumShield)
            {
                timer = 0f;
                currentShield += recoveryPower * efficiency;

                if (currentShield > maximumShield)
                    currentShield = maximumShield;
                
                ApplyDepreciation(2048);
            }

            if (shieldRenderer)
                shieldRenderer.material.color = color;
        }
    }

    public void Reflect(float shieldDamage)
    {
        currentShield -= shieldDamage;
        color.a = 1f;

        if (currentShield <= 0f)
        {
            currentShield = -1f;
            isNotActive = true;
            shieldShpere.SetActive(false);
            StartCoroutine(ActivateShield());
        }
    }

    IEnumerator ActivateShield()
    {
        yield return new WaitForSeconds(activationTime);

        if (shieldShpere)
        {
            shieldShpere.SetActive(true);
            //isInstalled = true;
            isNotActive = false;
        }
    }
        
    public override void InstallModule(Hull hull)
    {
        base.InstallModule(hull);
        hull.shield = this;
        shieldShpere = hull.shieldSphere;
        shieldShpere.SetActive(true);
        shieldRenderer = shieldShpere.GetComponent<Renderer>();
        shieldRenderer.material.color = color;
    }

    public override void DeinstallModule()
    {
        hull.shield = null;
        shieldShpere.SetActive(false);
        shieldShpere = null;
        shieldRenderer = null;
        base.DeinstallModule();
    }

    public override List<string> GetSerializableData()
    {
        List<string> serializedData = base.GetSerializableData();

        ShieldData shieldData = new ShieldData()
        {
            maximumShield = this.maximumShield,
            currentShield = this.currentShield,
            recoveryPower = this.recoveryPower,
            activationTime = this.activationTime,
            timer = this.timer
        };

        serializedData.Add(JsonUtility.ToJson(shieldData));
        return serializedData;
    }

    public override void SetSerializableData(List<string> data)
    {
        ShieldData shieldData = JsonUtility.FromJson<ShieldData>(data[data.Count - 1]);

        maximumShield = shieldData.maximumShield;
        currentShield = shieldData.currentShield;
        recoveryPower = shieldData.recoveryPower;
        activationTime = shieldData.activationTime;
        timer = shieldData.timer;

        data.Remove(data[data.Count - 1]);
        base.SetSerializableData(data);
    }

    public override string ToString()
    {
        return base.ToString() + LocaManager.textUnits["maxshield"] + maximumShield.ToString("0.00") + "\n" +
            LocaManager.textUnits["curshield"] + currentShield.ToString("0.00") + "\n" +
        LocaManager.textUnits["recpower"] + recoveryPower.ToString("0.00");
    }

    public static Shield GenerateShield(int level)
    {
        Color[] colorArray = new Color[]
        {
            Color.red,
            Color.white,
            Color.blue,
            Color.green,
            Color.yellow,
            Color.cyan,
            Color.magenta
        };
        
        Shield shield = Instantiate(DataBase.shield);

        float quality = Random.Range(1f, 2.6f);

        shield.weight = Random.Range(22f, 26f);
        shield.level = level;
        shield.cost = (int)(7500 * level * quality);

        shield.color = colorArray[Random.Range(0, colorArray.Length)];
        shield.maximumShield = 180f * level * quality;
        shield.currentShield = shield.maximumShield;
        shield.recoveryPower = 1.5f * level * quality;
        shield.activationTime = Random.Range(4f, 6.5f);

        return shield;
    }
}
