using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairDroid : Device 
{
    [SerializeField]
    float maxRecoveryPercent;
    [SerializeField]
    float afterDamageTime;
    [SerializeField]
    float recoveryPower;

    float timer;
	
	void Update () 
    {
        if (isInstalled)
        {
            timer += Time.deltaTime;
            if (timer >= 0.16f && (hull.health*100/hull.maximumHealth) < maxRecoveryPercent)
            {   
                hull.health += recoveryPower * efficiency;
                timer = 0f;

                ApplyDepreciation(126);
            }
        }
	}

    public void Damaged()
    {
        timer = 0f;
        timer = -afterDamageTime;
    }

    public override void InstallModule(Hull hull)
    {
        base.InstallModule(hull);
        hull.repairDroid = this;
    }

    public override void DeinstallModule()
    {
        hull.repairDroid = null;   
        base.DeinstallModule();
    }

    public override List<string> GetSerializableData()
    {
        List<string> data = base.GetSerializableData();
        
        RepairDroidData repairDroidData = new RepairDroidData()
        {
            maxRecoveryPercent = this.maxRecoveryPercent,
            afterDamageTime = this.afterDamageTime,
            recoveryPower = this.recoveryPower,
            timer = this.timer
        };

        data.Add(JsonUtility.ToJson(repairDroidData));
        return data;
    }

    public override void SetSerializableData(List<string> data)
    {
        RepairDroidData repairDroidData = JsonUtility.FromJson<RepairDroidData>(data[data.Count - 1]);

        maxRecoveryPercent = repairDroidData.maxRecoveryPercent;
        afterDamageTime = repairDroidData.afterDamageTime;
        recoveryPower = repairDroidData.recoveryPower;
        timer = repairDroidData.timer;

        data.Remove(data[data.Count - 1]);
        base.SetSerializableData(data);
    }

    public override string ToString()
    {
        return base.ToString() + LocaManager.textUnits["recpower"] + recoveryPower.ToString("0.00") + "\n" +
        LocaManager.textUnits["afterdamtime"] + afterDamageTime.ToString("0.00") + "\n" +
        LocaManager.textUnits["maxrecpercent"] + maxRecoveryPercent.ToString("0.00");
    }

    public static RepairDroid GenerateRepairDroid(int level)
    {
        RepairDroid repairDroid = Instantiate(DataBase.repairDroid);

        float quality = Random.Range(1f, 2.2f);

        repairDroid.weight = Random.Range(16f, 22f);
        repairDroid.level = level;
        repairDroid.cost = (int)(6200 * level * quality);

        repairDroid.maxRecoveryPercent = 34f * quality + (level * 3.8f);
        repairDroid.afterDamageTime = Random.Range(2f, 3.8f);
        repairDroid.recoveryPower = 2f * level * quality;

        return repairDroid;
    }
}
