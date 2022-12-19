using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : Item 
{
    public enum ModuleType
    {
        Weapon,
        Device
    }

    public ModuleType moduleType;
    public bool isInstalled;

    [SerializeField]
    AnimationCurve efficiencyAddiction;

    [SerializeField]
    protected Hull hull;

    [SerializeField]
    protected float efficiency;

    public void ApplyDepreciation(int maxValue)
    {
        if (Random.Range(0, maxValue) == 0)
        {
            health  -= 1f;

            if (health < 0.0f)
                health = 0.0f;
            efficiency = efficiencyAddiction.Evaluate(health * 0.01f);
        }
    }

    public virtual void InstallModule(Hull hull, int slot)
    {
        InstallModule(hull);
    }

    public virtual void InstallModule(Hull hull)
    {
        for (int i = 0; i < 8; i++)
            if (hull.installedModules[i] == null)
            {
                hull.installedModules[i] = this;
                break;
            }
        
        this.hull = hull;
        isInstalled = true;
    }

    public virtual void DeinstallModule()
    {
        for (int i = 0; i < 8; i++)
            if (hull.installedModules[i] == this)
            {
                hull.installedModules[i] = null;
                break;
            }
        
        transform.parent = null;
        hull = null;
        isInstalled = false;    
    }
        
    public bool LoadToCargoHold(Hull hull)
    {
        foreach (var slot in hull.cargoHoldModules)
            if (slot == null)
            {
                for (int i = 0; i < 104; i++)
                    if (hull.cargoHoldModules[i] == null)
                    {
                        hull.cargoHoldModules[i] = this;
                        break;
                    }
                
                transform.position = hull.cargoHoldContainer.position;
                transform.parent = hull.cargoHoldContainer;
                this.hull = hull;
                return true;
            }
        return false;
    }

    public bool LoadToCargoHold(Hull hull, int slotIndex)
    {
        hull.cargoHoldModules[slotIndex] = this;
        transform.position = hull.cargoHoldContainer.position;
        transform.parent = hull.cargoHoldContainer;
        return true;
    }

    public override List<string> GetSerializableData()
    {
        List<string> serializedData = base.GetSerializableData();
        serializedData.Add(JsonUtility.ToJson(new ModuleData(){ efficiency = this.efficiency }));
        return serializedData;
    }

    public override void SetSerializableData(List<string> data)
    {
        ModuleData moduleData = JsonUtility.FromJson<ModuleData>(data[data.Count - 1]);
        this.efficiency = moduleData.efficiency;
        data.Remove(data[data.Count - 1]);
        base.SetSerializableData(data);
    }

    public override string ToString()
    {
        return LocaManager.textUnits[moduleType.ToString().ToLower()] + "\n" +  base.ToString();
    }

    public void RepairModule()
    {
        efficiency = 1f;
        health = 100f;
    }

    public static Module GenerateModule(int level)
    {
        int type = Random.Range(0, 2);
        if (type == 0)
            return Weapon.GenerateWeapon(level);
        else
            return Device.GenerateDevice(level);

    }
}
