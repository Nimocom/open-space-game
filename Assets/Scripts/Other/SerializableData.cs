using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSerializableData
{
    public List<string> stringList;
    public int templateID;
}
    
[System.Serializable]
public class PlayerSerializableData
{
    public CharacterData characterData = new CharacterData();
    public ItemSerializableData shipData = new ItemSerializableData();
}

[System.Serializable]
public class ContainerSerializableData
{
    public Vector3 position;
    public Quaternion rotation;
    public ItemSerializableData moduleData  = new ItemSerializableData();
}

[System.Serializable]
public class PlanetSerializableData
{
    public int planetIndex;
    public Quaternion rotation;
    public Vector3 position;
}

[System.Serializable]
public class SystemSerializeData
{
    public int sceneIndex;
    public int systemIndex;
    public List<PlayerSerializableData> npc = new List<PlayerSerializableData>();
    public List<PlanetSerializableData> planetTransforms = new List<PlanetSerializableData>();
    public List<ContainerSerializableData> containers = new List<ContainerSerializableData>();
    public PlayerSerializableData mainPlayer;
}

    
[System.Serializable]
public class CurrentPlanetData
{
    public string planetName;
    public string population;
    public Texture[] textures;
}


[System.Serializable]
public class CharacterData
{
    public Character.Fraction fraction;
    public string characterID;
    public string name;
    public int avatarID;
    public int credits;
    public int level;
}
    
[System.Serializable]
public class ItemData
{
    public int templateID;
    public float health;
    public float weight;
    public string name;
    public int level;
    public int cost;

}

[System.Serializable]
public class ShipData
{
    public List<ItemSerializableData> cargoHoldModules = new List<ItemSerializableData>();
    public List<ItemSerializableData> installedModules = new List<ItemSerializableData>();
    public float maximumCargoPoints;
    public HullPartsData partsData;
    public float maximumHealth;
    public Quaternion rotation;
    public Vector3 position;
}

[System.Serializable] 
public class HullPartsData
{
    public int cabinePartID;
    public int frontPartID;
    public int rearPartID;
    public int wingPartID;
    public int basePartID;
}

[System.Serializable]
public class ModuleData
{
    public float efficiency;
}

[System.Serializable]
public class WeaponData
{
    public float hullDamage;
    public float shieldDamage;
    public float energyUsing;
    public float reloadTime;
    public float reloadingTimer;
    public int slotIndex;
}

[System.Serializable]
public class BlasterData
{
    public int poolCount;
    public int shellID;
}

[System.Serializable]
public class LaserData
{
    public float particlesPower;
    public float soundPitch;

    public Color laserColor;
}

[System.Serializable]
public class RocketLauncherData
{

    public float movementSpeed;
    public float rotationPower;
    public float timer;

    public int rocketShellID;
}

[System.Serializable]
public class DeviceData
{
    
}

[System.Serializable]
public class EngineData
{
    public float movementPower;
    public float rotationPower;
}

[System.Serializable]
public class EnergyGeneratorData
{
    public float maximumEnergy;
    public float currentEnergy;
    public float recoveryPower;
    public float timer;
}

[System.Serializable]
public class RepairDroidData
{
    public float maxRecoveryPercent;
    public float afterDamageTime;
    public float recoveryPower;
    public float timer;
}

[System.Serializable]
public class ShieldData
{
    public float maximumShield;
    public float currentShield;
    public float recoveryPower;
    public float activationTime;
    public float timer;

    public Color color;
}


