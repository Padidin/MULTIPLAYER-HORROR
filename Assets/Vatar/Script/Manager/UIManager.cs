using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject interactTextObject; 
    public Text notificationText;       

    void Awake()
    {
        Instance = this;
    }

    public void ShowInteractText(bool show)
    {
        if (interactTextObject != null)
            interactTextObject.SetActive(show);
    }

    public void ShowNotification(string message)
    {
        StopAllCoroutines();
        StartCoroutine(ShowNotifRoutine(message));
    }

    private IEnumerator ShowNotifRoutine(string msg)
    {
        if (notificationText != null)
        {
            notificationText.text = msg;
            notificationText.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            notificationText.gameObject.SetActive(false);
        }
    }
}
