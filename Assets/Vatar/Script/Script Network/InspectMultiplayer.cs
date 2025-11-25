using UnityEngine;

public class InspectMultiplayer : MonoBehaviour
{
    [Header("Komponent Inspect")]
    public Camera inspectCamera;
    public Transform inspectHolder;
    public Transform dropPoint;
    public string namaInspect;
    public string namaHolder;
    public float zoomSpeed = 2f;

    public Playere player;

    [Header("Item dan UI")]
    public GameObject currentItem;
    public bool isInspecting = false;
    public GameObject CrossBar;
    public GameObject UiItem;
    public GameObject UiNotHoldingAnyItem;
    public GameObject UiIndikator;

    void Awake()
    {
        GameObject inspectObject = GameObject.Find(namaInspect);
        inspectHolder = GameObject.Find(namaHolder).transform;

        if (inspectObject != null)
        {
            inspectCamera = inspectObject.GetComponent<Camera>();
        }
    }

    void Start()
    {

    }

    void Update()
    {
        CheckItemHold();

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (isInspecting)
                CloseInspect();
            else
                OpenInspect();
        }

        // zoom pakai scroll
        if (isInspecting)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            inspectCamera.fieldOfView -= scroll * zoomSpeed;
            inspectCamera.fieldOfView = Mathf.Clamp(inspectCamera.fieldOfView, 20f, 60f);
        }
        else
        {
            player.canWalk = true;
        }
    }
    void OpenInspect()
    {
        if (currentItem == null)
        {
            UiNotHoldingAnyItem.SetActive(true);
        }
        else
        {
            UiItem.SetActive(true);
            RotateInspectMulti itemRotate = currentItem.GetComponent<RotateInspectMulti>();
            itemRotate.bisaRotate = true;
        }

        CrossBar.SetActive(false);
        UiIndikator.SetActive(false);
        player.canWalk = false;
        isInspecting = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void CloseInspect()
    {
        isInspecting = false;
        UiItem.SetActive(false);
        UiNotHoldingAnyItem.SetActive(false);
        CrossBar.SetActive(true);
        UiIndikator.SetActive(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (currentItem != null)
        {
            RotateInspectMulti itemRotate = currentItem.GetComponent<RotateInspectMulti>();
            itemRotate.bisaRotate = false;
        }
    }

    void CheckItemHold()
    {
        if (inspectHolder.childCount > 0)
        {
            GameObject firstChild = inspectHolder.GetChild(0).gameObject;

            if (firstChild != null)
            {
                currentItem = firstChild;
                
                if (currentItem.GetComponent<RotateInspectMulti>() == null)
                {
                    currentItem.AddComponent<RotateInspectMulti>();
                }
            }
        }
        else
        {
            currentItem = null;
        }
    }

    public void DropItem()
    {
        CloseInspect();
        currentItem.transform.position = dropPoint.position;
        currentItem.transform.SetParent(null);
    }

}
