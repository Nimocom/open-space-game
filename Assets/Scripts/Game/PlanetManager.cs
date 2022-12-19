using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using System.Linq;

public class PlanetManager : MonoBehaviour
{
    public static PlanetManager inst;

    public static Module[] shopItems = new Module[68];

    public static CharacterData characterData;

    public static Hull characterHull;

    [SerializeField]
    AudioClip[] planetMusic;

    [SerializeField]
    Transform planetShop;

    [SerializeField]
    Renderer backGround;

    [SerializeField]    
    Text planetPopulationText;
    [SerializeField]
    Text hullRepairCostText;
    [SerializeField]
    Text playerCreditsText;
    [SerializeField]
    Text playerNameText;
    [SerializeField]
    Text planetNameText;

    bool isBackgroundFading;

    AudioSource aSource;

    void Awake()
    {
        inst = this;
        planetPopulationText.text = GameManager.currentPlanetData.population;
        aSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        characterData = GameManager.currentSystemData.mainPlayer.characterData;
        if (GameManager.currentPlanetData != null)
        {
            backGround.sharedMaterial.mainTexture = GameManager.currentPlanetData.textures[Random.Range(0, GameManager.currentPlanetData.textures.Length)];
            planetNameText.text = LocaManager.textUnits[GameManager.currentPlanetData.planetName.ToLower()];
            aSource.clip = planetMusic[Random.Range(0, planetMusic.Length)];
            aSource.time = 1.5f;
            aSource.Play();
        }

        CreateShipTemplate();
        FillShop();
        UIManager.inst.InitializeStatusBars(characterHull);

        playerCreditsText.text = characterData.credits.ToString("####");
        playerNameText.text = characterData.name;
    

        if (characterHull.health < characterHull.maximumHealth)
            hullRepairCostText.text = ((int)(characterHull.cost * (1 - (characterHull.health / characterHull.maximumHealth)))).ToString();
        else
            hullRepairCostText.text = "--";
    }

    void FillShop()
    {
        int amount = Random.Range(36, 68);
        for (int i = 0; i < amount; i++)
        {
//            int level = GameManager.currentSystemData.mainPlayer.characterData.level + Random.Range(-3, 4);
//            if (level < 1)
//                level = 1;
//            if (level > 10)
//                level = 10;
            shopItems[i] = Module.GenerateModule(Random.Range(7, 11));
        }
        ModuleComparer<Module> mc = new ModuleComparer<Module>();
        System.Array.Sort(shopItems, mc);
    }

    void Update()
    {
        if (Input.GetKeyDown(CKeys.inventorySwitch))
            SwitchShopWindow();

        if (Input.GetKeyDown(CKeys.saveGame))
        {
            GameManager.currentSystemData.systemIndex = SceneManager.GetActiveScene().buildIndex;
        }
    }

    public void SwitchShopWindow()
    {
        UIManager.inst.SwitchInventory();
        StopAllCoroutines();
        StartCoroutine(FadeBackground());
    }

    public void RepairHull()
    {
        if (characterData.credits >= characterHull.cost * (1 - (characterHull.health / characterHull.maximumHealth)))
        {
            characterHull.health = characterHull.maximumHealth;
            DecreaseCredits(characterHull.cost * (int)(1 - (characterHull.health / characterHull.maximumHealth)));
            hullRepairCostText.text = "--";
        }
    }

    public void RepairItem()
    {
        if (ServiceCell.currentModule && ServiceCell.currentModule.health < 100f && characterData.credits >= ServiceCell.currentModule.cost * (1 - (ServiceCell.currentModule.health / 100f)))
        {
            ServiceCell.currentModule.RepairModule();
            ServiceCell.inst.RefreshCell();
            DecreaseCredits(ServiceCell.currentModule.cost * (1 - (ServiceCell.currentModule.health / 100f)));
        }
    }

    public void SellItem()
    {
        if (ServiceCell.currentModule)
        {
            IncreaseCredits(ServiceCell.currentModule.cost - (ServiceCell.currentModule.cost * (1 - (ServiceCell.currentModule.health / 100f))));
          //  Destroy(ServiceCell.currentModule.gameObject);
            ServiceCell.currentModule = null;
            UIManager.draggedItem = null;
            ServiceCell.inst.RefreshCell();

            for (int i = 0; i < shopItems.Length; i++)
            {
                if (shopItems[i] == null)
                    shopItems[i] = ServiceCell.currentModule;
             
            }
        }
    }

    public void DecreaseCredits(float credits)
    {
        characterData.credits -= (int)credits;
        playerCreditsText.text = characterData.credits.ToString();
        ;
    }

    public void IncreaseCredits(float credits)
    {
        characterData.credits += (int)credits;
        playerCreditsText.text = characterData.credits.ToString();
    }

    IEnumerator FadeBackground()
    {
     //   StopAllCoroutines();
        isBackgroundFading = !isBackgroundFading;
        float timeElapsed = 0f;
        ;

        while ((timeElapsed += Time.deltaTime) <= 2f)
        { 
            yield return null;
            timeElapsed += Time.deltaTime;
            if (isBackgroundFading)
                backGround.material.color = Color.Lerp(backGround.material.color, new Color32(50, 50, 50, 255), 4f * Time.unscaledDeltaTime);
            else
                backGround.material.color = Color.Lerp(backGround.material.color, Color.white, 2f * Time.unscaledDeltaTime);
        }
    }

    void CreateShipTemplate()
    {
        Hull hull = Instantiate(DataBase.hullTemplate);
        hull.SetSerializableData(GameManager.currentSystemData.mainPlayer.shipData.stringList);

        //Character player = Instantiate(DataBase.mainPlayer);
        //player.GetComponent<PlayerController>().enabled = false;
        //player.SetCharacterData(GameManager.currentSystemData.mainPlayer.characterData);
        //hull.InitializeHull(player);

        //hull.GetComponent<CompositeCollider2D>().enabled = false;

        UIManager.currentHull = hull;
        characterHull = hull;
    }

    void SavePlayer()
    {
        GameManager.currentSystemData.mainPlayer.characterData = characterData;
        GameManager.currentSystemData.mainPlayer.shipData.stringList = characterHull.GetSerializableData();
   
        string jsonString = JsonUtility.ToJson(GameManager.currentSystemData);
        File.WriteAllText(Application.streamingAssetsPath + "/SavedData.json", jsonString);
    }

    public void FlyOut()
    {
        if (characterHull.energyGenerator && characterHull.engine)
        {
            Time.timeScale = 1f;
            SavePlayer();
            GameManager.LoadGame();
        }
    }
}
