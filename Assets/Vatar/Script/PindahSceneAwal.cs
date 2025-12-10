using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PindahSceneAwal : MonoBehaviour
{
    public string namaScene;
    void Start()
    {
        SceneManager.LoadScene(namaScene);
    }
}
