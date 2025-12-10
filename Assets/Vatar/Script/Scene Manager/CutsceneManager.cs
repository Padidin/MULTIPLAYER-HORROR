using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public string namaScene;
    public GameObject buttonSkip;

    private void Update()
    {
        if (buttonSkip.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(namaScene);
            }
        }
    }
}
