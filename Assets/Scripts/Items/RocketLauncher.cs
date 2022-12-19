using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RocketLauncher : Weapon
{
    public int rocketShellID;

    [SerializeField]
    float movementSpeed;
    [SerializeField]
    float rotationPower;

    Rocket rocketPrefab;

    void Start()
    {
        rocketPrefab = Instantiate(DataBase.rocketShells[rocketShellID], transform);
        rocketPrefab.gameObject.SetActive(false);
    }

    void Update()
    {
        reloadTimer += Time.deltaTime;
    }

    public override void Shoot(Transform target)
    {

        if (reloadTimer >= reloadTime / efficiency)
        {
            audioSource.Play();

            Rocket rocket = Instantiate<Rocket>(rocketPrefab, hull.centerSlot.position, hull.centerSlot.rotation);
            rocket.InitializeRocket(hullDamage, shieldDamage, movementSpeed, rotationPower, target, character);
            rocket.gameObject.SetActive(true);

            reloadTimer = 0f;

            ApplyDepreciation(16);
        }
    }

    public override void InstallModule(Hull hull)
    {
        InstallModule(hull, 3);
    }

    public override void InstallModule(Hull hull, int slot)
    {
        base.InstallModule(hull, 3);
    }

    public override List<string> GetSerializableData()
    {
        List<string> data = base.GetSerializableData();

        RocketLauncherData rocketLauncherData = new RocketLauncherData()
        {
            rocketShellID = this.rocketShellID,
            movementSpeed = this.movementSpeed,
            rotationPower = this.rotationPower,
            timer = this.reloadTime
        };

        data.Add(JsonUtility.ToJson(rocketLauncherData));
        return data;
    }

    public override void SetSerializableData(List<string> data)
    {
        RocketLauncherData rocketLauncherData = JsonUtility.FromJson<RocketLauncherData>(data[data.Count - 1]);

        rocketShellID = rocketLauncherData.rocketShellID;
        movementSpeed = rocketLauncherData.movementSpeed;
        rotationPower = rocketLauncherData.rotationPower;
        reloadTimer = rocketLauncherData.timer;

        data.Remove(data[data.Count - 1]);
        base.SetSerializableData(data);
    }

    public override string ToString()
    {
        return base.ToString() + LocaManager.textUnits["speed"] + movementSpeed.ToString("0.00") + "\n" +
        LocaManager.textUnits["rotspeed"] + rotationPower.ToString("0.00");
    }

    public static RocketLauncher GenerateRocketLauncher(int level)
    {
        RocketLauncher rocketLauncher = Instantiate(DataBase.rocketLauncher);

        float quality = Random.Range(1f, 2.4f);

        rocketLauncher.weight = Random.Range(20f, 26f);
        rocketLauncher.level = level;
        rocketLauncher.cost = (int)(7800 * level * quality);

        rocketLauncher.hullDamage = 86f * level * quality;
        rocketLauncher.shieldDamage = 72f * level * quality;
        rocketLauncher.reloadTime = Random.Range(0.86f, 1f);

        rocketLauncher.rocketShellID = DataBase.rocketShells.ElementAt(Random.Range(0, DataBase.rocketShells.Count)).Key;
        rocketLauncher.movementSpeed = Random.Range(8f, 12f);
        rocketLauncher.rotationPower = Random.Range(16, 18f);

        return rocketLauncher;
    }

}
