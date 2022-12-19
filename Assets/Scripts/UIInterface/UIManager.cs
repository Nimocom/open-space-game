using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager inst;

    public static MovableCell draggedItem;
    public static bool isShopItem;


    public  MovableCell movableCell;
    public Sprite defaultSprite;
    public Texture2D arrowCursor;
    public Texture2D aimCursor;
    public GameObject infoPanel;
    public GameObject inventoryGO;
    public Transform targetSelector;
    public Transform targetArrow;
    public Transform planetPoint;
    public Text infoPanelText;
    public Text autoRotationText;
    public Text freeCamText;
    public Text gameSavedText;
    public Slider enemyHealth;
    public Slider enemyShield;
    public Slider shieldBar;
    public Slider healthBar;
    public Slider energyBar;
    public static Hull currentHull;

    Coroutine inventoryFading;
    Coroutine savedGameTextFading;

    Hull currentTarget;
    bool targetSelected;
    Renderer rend;
    Camera mainCamera;

    float prevShieldValuy;
    float prevEnergyValue;
    float prevHealthValue;

    void Awake()
    {
        inst = this;   
        mainCamera = Camera.main;
    }

    void Start()
    {
        CloseInfoPanel();
    }

    public void ShowInfoPanel(string info)
    {
        infoPanel.SetActive(true);
        infoPanelText.text = info;
    }

    public void CloseInfoPanel()
    {
        infoPanel.SetActive(false);
        infoPanelText.text = "";
    }

    public void SetTarget(Hull hull)
    {
        if (currentTarget != hull)
        {
            rend = hull.GetComponentInChildren<Renderer>();
            currentTarget = hull;
            if (rend.isVisible)
                targetSelector.gameObject.SetActive(true);
            else
                targetArrow.gameObject.SetActive(true);
            targetSelected = true;
            enemyHealth.maxValue = currentTarget.maximumHealth;
            if (currentTarget.shield)
                enemyShield.maxValue = currentTarget.shield.maximumShield;
            else
            {
                enemyShield.maxValue = 0f;
                enemyShield.value = 0f;
            }
        }
    }

    public void ResetTarget()
    {
        targetSelected = false;
        targetSelector.gameObject.SetActive(false);
        targetArrow.gameObject.SetActive(false);
        currentTarget = null;
    }

    void LateUpdate()
    {
        if (targetSelected)
        {
            if (currentTarget)
            {
                if (rend.isVisible)
                {
                    if (targetArrow.gameObject.activeSelf)
                        targetArrow.gameObject.SetActive(false);
                    
                    if (!targetSelector.gameObject.activeSelf)
                        targetSelector.gameObject.SetActive(true);
                    
                    targetSelector.position = mainCamera.WorldToScreenPoint(currentTarget.transform.position);
                    enemyHealth.value = Mathf.Lerp(enemyHealth.value, currentTarget.health, 12f * Time.unscaledDeltaTime);

                    if (currentTarget.shield)
                        enemyShield.value = Mathf.Lerp(enemyShield.value, currentTarget.shield.currentShield, 12f * Time.unscaledDeltaTime);
                }
                else
                {
                    if (targetSelector.gameObject.activeSelf)
                        targetSelector.gameObject.SetActive(false);
                    if (!targetArrow.gameObject.activeSelf)
                        targetArrow.gameObject.SetActive(true);
                    
                    Vector3 targetOnScreen = mainCamera.WorldToScreenPoint(currentTarget.transform.position);
                    targetOnScreen.x = Mathf.Clamp(targetOnScreen.x, 20, Screen.width - 20f);
                    targetOnScreen.y = Mathf.Clamp(targetOnScreen.y, 20, Screen.height - 20f);
                    //targetArrow.LookAt(Camera.main.WorldToScreenPoint(currentTarget.transform.position));
                    targetArrow.rotation = Quaternion.LookRotation(mainCamera.WorldToScreenPoint(currentTarget.transform.position) - new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
                    targetArrow.position = targetOnScreen;
                }
            }
            else
            {
                ResetTarget();
            }
        }

        if (currentHull)
        {
            if (currentHull.shield)
                shieldBar.value = Mathf.Lerp(shieldBar.value, currentHull.shield.currentShield, 20f * Time.unscaledDeltaTime);
            else
                shieldBar.value = 0f;
            
            if (currentHull.energyGenerator)
                energyBar.value = Mathf.Lerp(energyBar.value, currentHull.energyGenerator.currentEnergy, 20f * Time.unscaledDeltaTime);
            else
                energyBar.value = 0f;
            healthBar.value = Mathf.Lerp(healthBar.value, currentHull.health, 20f * Time.unscaledDeltaTime);
        }
    }

    public void SwitchInventory()
    {
        if (inventoryGO.activeSelf && !draggedItem)
        {
            CloseInfoPanel();
            inventoryGO.SetActive(false);
            if (inventoryFading != null)
                StopCoroutine(inventoryFading);
            inventoryFading = StartCoroutine(SetTimeScale(1f));
            SetCursorToAim();
        }
        else
        {
            inventoryGO.SetActive(true);
            if (inventoryFading != null)
                StopCoroutine(inventoryFading);
            inventoryFading = StartCoroutine(SetTimeScale(0f));
            SetCursorToArrow();
        }
    }

    IEnumerator SetTimeScale(float scale)
    {
        float timeElapsed = 0f;
        ;

        while ((timeElapsed += Time.unscaledDeltaTime) <= 2f)
        { 
            yield return null;
            timeElapsed += Time.deltaTime;
            Time.timeScale = Mathf.Lerp(Time.timeScale, scale, 6f * Time.unscaledDeltaTime);
        }
    }

    //    public void SetTimeScale(float timeScale)
    //    {
    //        Time.timeScale = timeScale;
    //    }

    public void SetCursorToAim()
    {
        Cursor.SetCursor(aimCursor, new Vector2(64f, 64f), CursorMode.Auto);
    }

    public void SetCursorToArrow()
    {
        Cursor.SetCursor(arrowCursor, Vector2.zero, CursorMode.Auto);
    }

    public void InitializeStatusBars(Hull hull)
    {
        currentHull = hull;
        if (currentHull.shield)
            shieldBar.maxValue = currentHull.shield.maximumShield;
        if (currentHull.energyGenerator)
            energyBar.maxValue = currentHull.energyGenerator.maximumEnergy;
        healthBar.maxValue = currentHull.maximumHealth;
    }

    public void ShowGameSavedText()
    {
        UIManager.inst.gameSavedText.enabled = true;
        if (savedGameTextFading != null)
            StopCoroutine(savedGameTextFading);
        savedGameTextFading = StartCoroutine(FadeGSText());
    }

    IEnumerator FadeGSText()
    {
        float timer = 0f;
        Color color = UIManager.inst.gameSavedText.color;
        color.a = 255f;
        UIManager.inst.gameSavedText.color = color;
        while ((timer += Time.deltaTime) < 5f)
        {
            yield return null;
            color.a = Mathf.Lerp(color.a, 0f, 3f * Time.deltaTime);
            UIManager.inst.gameSavedText.color = color;
        }
        UIManager.inst.gameSavedText.enabled = false;
    }
}
