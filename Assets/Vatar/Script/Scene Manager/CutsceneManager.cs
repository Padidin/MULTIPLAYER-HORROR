using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public string namaScene;
    public int delaySkip;
    public GameObject buttonSkip;

    public void Start()
    {
        buttonSkip.SetActive(false);
        StartCoroutine(DelaySkip());
    }

    IEnumerator DelaySkip()
    {
        yield return new WaitForSeconds(delaySkip);
        buttonSkip.SetActive(true);
    }

    public void PerpindahanScene()
    {
        SceneManager.LoadScene(namaScene);
    }
}
