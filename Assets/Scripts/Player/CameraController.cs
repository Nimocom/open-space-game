using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    public static CameraController inst;
    public float fallowSpeed = 5f;
    public float rotationSpeed = 5f;
    public float sideScrollSpeed = 10f;
    public bool isMoovable;
    float orthS;
    bool isMoving;
    MapMarker marker;
    Vector3 camPos;
    Hull playerHull;
    Coroutine centerToTarget;
    Camera mainCamera;

    void Awake()
    {
        inst = this;
        orthS = 8.5f; 
        marker = Camera.main.GetComponent<MapMarker>();
        mainCamera = Camera.main;
        marker.isActive = false;
        mainCamera.GetComponent<UnityStandardAssets.ImageEffects.BlurOptimized>().enabled = Settings.settingsData.graphicSettings.imageEffects;
    }

	void Start () 
    {
       
	}
	
	void Update()
    {

        if (Input.GetKeyDown(CKeys.loadGame))
            GameManager.LoadGame();
        if (Input.GetKeyDown(KeyCode.F1))
            StarSystem.GenerateNPC(-1,Character.Fraction.Military);
        if (Input.GetKeyDown(KeyCode.F2))
            StarSystem.GenerateNPC(-1,Character.Fraction.Pirate);

        {
            if (isMoovable && isMoving)
            {
                if (Input.mousePosition.x >= Screen.width - 10f)
                    transform.Translate(Vector3.down * sideScrollSpeed * Time.deltaTime);
                if (Input.mousePosition.x <= 0.3f)
                    transform.Translate(Vector3.up * sideScrollSpeed * Time.deltaTime);
                if (Input.mousePosition.y >= Screen.height - 10f)
                    transform.Translate(Vector3.right * sideScrollSpeed * Time.deltaTime);
                if (Input.mousePosition.y <= 0.3f)
                    transform.Translate(Vector3.left * sideScrollSpeed * Time.deltaTime);
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                orthS -= Input.GetAxis("Mouse ScrollWheel")*1.8f;
                orthS = Mathf.Clamp(orthS, 3.5f, 12f);
            }

            if (Input.GetKeyDown(CKeys.allowCameraMoving))
            {
                isMoving = !isMoving;
                marker.isActive = isMoving;
                UIManager.inst.freeCamText.enabled = isMoving;
            }
        }

        if (playerHull && !isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(playerHull.transform.position.x, playerHull.transform.position.y, transform.position.z), fallowSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, playerHull.transform.rotation, rotationSpeed * Time.deltaTime);
        }           

        mainCamera.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, orthS, 5f * Time.deltaTime);
        camPos = mainCamera.transform.localPosition;
        camPos.x = Mathf.Lerp(camPos.x, orthS - 1f, 5f * Time.deltaTime);
        mainCamera.transform.localPosition = camPos;
    }

    public void CenterToTarget(Transform target)
    {
        if (centerToTarget != null)
            StopCoroutine(centerToTarget);
        centerToTarget = StartCoroutine(LerpCamera(target));
    }

    IEnumerator LerpCamera(Transform target)
    {
        Vector2 targetPosition = target.position + transform.right * -5f;
        while (((Vector2)transform.position - targetPosition).sqrMagnitude > 0.1f)
        {
            transform.position = Vector2.Lerp(transform.position, targetPosition, 12f * Time.unscaledDeltaTime);
            yield return null;
        }
    }

    public void InitializeCamera(Hull hull)
    {
        playerHull = hull;
        transform.position =(Vector2) hull.transform.position;
        transform.rotation = hull.transform.rotation;
    }
}
