using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Device : Module 
{
    public enum DeviceType
    {
        EnergyGenerator,
        Engine,
        RepairDroid,
        Shield
    }

    public DeviceType deviceType;

    public override string ToString()
    {
        return LocaManager.textUnits[deviceType.ToString().ToLower()] + "\n" + base.ToString();
    }

    public override void InstallModule(Hull hull)
    {
        base.InstallModule(hull);
        transform.position = hull.devicesContainer.position;
        transform.parent = hull.devicesContainer;
    }

    public static Device GenerateDevice(int level)
    {
        int type = Random.Range(0, 4);
        if (type == 0)
            return EnergyGenerator.GenerateEnergyGenerator(level);
        else if (type == 1)
            return Engine.GenerateEngine(level);
        else if (type == 2)
            return RepairDroid.GenerateRepairDroid(level);
        else
            return Shield.GenerateShield(level);
    }
}
