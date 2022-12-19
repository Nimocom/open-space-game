using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Module
{
    public enum WeaponType
    {
        Blaster,
        Laser,
        Vertex,
        RocketLauncher
    }
        
    public WeaponType weaponType;



    [SerializeField]
    protected Character character;

    [SerializeField]
    protected AudioSource audioSource;   
    [SerializeField]
    protected float hullDamage;
    [SerializeField]
    protected float shieldDamage;
    [SerializeField]
    protected float energyUsing;
    [SerializeField]
    protected float reloadTimer;
    [SerializeField]
    protected float reloadTime;
    [SerializeField]
    protected int slotIndex;

    public virtual void Shoot()
    {
    }

    public virtual void Shoot(Transform target)
    {
    }

    public override void InstallModule(Hull hull, int slot)
    {
        base.InstallModule(hull);
        hull.weapons[slot] = this;
        transform.position = hull.weaponsContainer.position;
        transform.parent = hull.weaponsContainer;
        slotIndex = slot;
        character = hull.character;
        audioSource.enabled = true;
    }

    public override void InstallModule(Hull hull)
    {
        InstallModule(hull, slotIndex);
    }

    public override void DeinstallModule()
    {
        character = null;
        audioSource.enabled = false;
        hull.weapons[slotIndex] = null;
        slotIndex = -1;
        base.DeinstallModule();
    }

    public override List<string> GetSerializableData()
    {
        List<string> serializedData = base.GetSerializableData();
        WeaponData weaponData = new WeaponData()
        {
            reloadingTimer = this.reloadTimer,
            shieldDamage = this.shieldDamage,
            energyUsing = this.energyUsing,
            hullDamage = this.hullDamage,
            reloadTime = this.reloadTime,
            slotIndex = this.slotIndex,
                    
        };
        serializedData.Add(JsonUtility.ToJson(weaponData));
        return serializedData;
    }

    public override void SetSerializableData(List<string> data)
    {
        WeaponData weaponData = JsonUtility.FromJson<WeaponData>(data[data.Count - 1]);

        reloadTimer = weaponData.reloadingTimer;
        shieldDamage = weaponData.shieldDamage;
        energyUsing = weaponData.energyUsing;
        hullDamage = weaponData.hullDamage;
        reloadTime = weaponData.reloadTime;
        slotIndex = weaponData.slotIndex;

        data.Remove(data[data.Count - 1]);
        base.SetSerializableData(data);
    }

    public override string ToString()
    {
        return LocaManager.textUnits[weaponType.ToString().ToLower()] + "\n" + base.ToString() + 
            LocaManager.textUnits["hulldamage"] + hullDamage.ToString("0.00") + "\n" +
            LocaManager.textUnits["shielddamage"] + shieldDamage.ToString("0.00") + "\n" + 
            LocaManager.textUnits["enerusing"] + energyUsing.ToString("0.00") + "\n" +
            LocaManager.textUnits["reltime"] + reloadTime.ToString("0.00") + "\n";
    }

    public static Weapon GenerateWeapon(int level)
    {
        int type = Random.Range(0, 3);

        if (type == 0)
            return Blaster.GenerateBlaster(level);
        else if (type == 1)
            return Laser.GenerateLaser(level);
        else
            return RocketLauncher.GenerateRocketLauncher(level);
    }
}