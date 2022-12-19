using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Blaster : Weapon
{

    [SerializeField]
    Shell[] shellsPool;

    [SerializeField]
    int shellID;

    int poolCount;

    void Start()
    {
        audioSource.clip = DataBase.gunShells[shellID].fireSound;
        shellsPool = new Shell[poolCount];

        for (int i = 0; i < poolCount; i++)
        {
            Shell temp = Instantiate(DataBase.gunShells[shellID]);
            temp.InitializeShell(hullDamage, shieldDamage);
            shellsPool[i] = temp;
            temp.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isInstalled)
            reloadTimer += Time.deltaTime;
    }

    public override void Shoot()
    {
        if (reloadTimer >= reloadTime / efficiency && hull.energyGenerator.currentEnergy >= energyUsing)
        {    
            audioSource.Play();
            ApplyDepreciation(36);

            for (int i = 0; i < shellsPool.Length; i++)
                if (!shellsPool[i].gameObject.activeInHierarchy)
                {
                    shellsPool[i].transform.position = hull.leftSlot.position;
                    shellsPool[i].transform.rotation = hull.leftSlot.rotation;
                    shellsPool[i].InitializeShell(character);
                    shellsPool[i].gameObject.SetActive(true);
                    break;
                }

            for (int i = 0; i < shellsPool.Length; i++)
                if (!shellsPool[i].gameObject.activeInHierarchy)
                {
                    shellsPool[i].transform.position = hull.rightSlot.position;
                    shellsPool[i].transform.rotation = hull.rightSlot.rotation;
                    shellsPool[i].InitializeShell(character);
                    shellsPool[i].gameObject.SetActive(true);
                    break;
                }
            
            reloadTimer = 0f;
            hull.energyGenerator.currentEnergy -= energyUsing;
        }
    }

    public override List<string> GetSerializableData()
    {
        List<string> serializedData = base.GetSerializableData();

        BlasterData blasterData = new BlasterData()
        {
            shellID = this.shellID,
            poolCount = this.poolCount
        };
        
        serializedData.Add(JsonUtility.ToJson(blasterData));
        return serializedData;
    }

    public override void SetSerializableData(List<string> data)
    {
        BlasterData blasterData = JsonUtility.FromJson<BlasterData>(data[data.Count - 1]);

        shellID = blasterData.shellID;
        poolCount = blasterData.poolCount;

        data.Remove(data[data.Count - 1]);
        base.SetSerializableData(data);
    }

    void OnDestroy()
    {
        for (int i = 0; i < poolCount; i++)
            if(shellsPool[i])
            Destroy(shellsPool[i].gameObject, 1f);    
    }

    public static Blaster GenerateBlaster(int level)
    {
        Blaster blaster = Instantiate(DataBase.blaster);
        float quality = Random.Range(1.2f, 2.6f);

        blaster.weight = Random.Range(18f, 26f); 
        blaster.level = level;
        blaster.cost =(int)(4500f * level * quality);

        Shell shell = DataBase.gunShells.ElementAt(Random.Range(0, DataBase.gunShells.Count)).Value;

        blaster.shieldDamage = shell.shieldDamageMlt * level * quality;
        blaster.hullDamage = shell.hullDamageMlt * level * quality;
        blaster.reloadTime = shell.relaodTime;

        blaster.energyUsing = shell.energyUsingMlt * quality * level;
        blaster.poolCount = shell.poolCapacity;
        blaster.shellID = shell.shellID;


        return blaster;
    }
}
