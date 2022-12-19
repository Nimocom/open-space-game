using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyGenerator : Device
{
    public float maximumEnergy;
    public float currentEnergy;

    [SerializeField]
    public float recoveryPower;

    float timer;

	void Update ()
    {
        if (isInstalled)
        {
            timer += Time.deltaTime;
            if (timer >= 0.2f && currentEnergy < maximumEnergy)
            {
                timer = 0f;
                currentEnergy += recoveryPower * efficiency;
                if (currentEnergy > maximumEnergy)
                    currentEnergy = maximumEnergy;
                
                ApplyDepreciation(1024);
            }
        }
    }

    public override void InstallModule(Hull hull)
    {
        base.InstallModule(hull);
        hull.energyGenerator = this;
    }

    public override void DeinstallModule()
    {
        hull.energyGenerator = null;
        base.DeinstallModule();
    }

    public override string ToString()
    {
        return base.ToString() + LocaManager.textUnits["maxenergy"] + maximumEnergy.ToString("0.00") + "\n" + 
            LocaManager.textUnits["curenergy"] + currentEnergy.ToString("0.00") + "\n" + 
            LocaManager.textUnits["recpower"] + recoveryPower.ToString("0.00") + "\n";
    }

    public override List<string> GetSerializableData()
    {
        List<string> serializedData = base.GetSerializableData();

        EnergyGeneratorData energyGeneratorData = new EnergyGeneratorData()
        {
            maximumEnergy = this.maximumEnergy,
            currentEnergy = this.currentEnergy,
            recoveryPower = this.recoveryPower,
            timer = this.timer
        };

        serializedData.Add(JsonUtility.ToJson(energyGeneratorData));
        return serializedData;
    }

    public override void SetSerializableData(List<string> data)
    {
        EnergyGeneratorData energyGeneratorData = JsonUtility.FromJson<EnergyGeneratorData>(data[data.Count - 1]);

        maximumEnergy = energyGeneratorData.maximumEnergy;
        currentEnergy = energyGeneratorData.currentEnergy;
        recoveryPower = energyGeneratorData.recoveryPower;
        timer = energyGeneratorData.timer;

        data.Remove(data[data.Count - 1]);
        base.SetSerializableData(data);
    }

    public static EnergyGenerator GenerateEnergyGenerator(int level)
    {
        float quality = Random.Range(1f, 2.4f);

        EnergyGenerator energyGenerator = Instantiate(DataBase.energyGenerator);

        energyGenerator.weight = Random.Range(18f, 26f);
        energyGenerator.level = level;
        energyGenerator.cost = (int)(9600f * level * quality);

        energyGenerator.maximumEnergy = 82f * level * quality;
        energyGenerator.currentEnergy = energyGenerator.maximumEnergy;
        energyGenerator.recoveryPower = 0.62f * level * quality;

        return energyGenerator;
    }
}
