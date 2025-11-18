using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestCanvas : MonoBehaviour
{
    public bool questMuncul;
    public GameObject timelineQuest;

    private void Start()
    {
        timelineQuest.SetActive(false);
    }
    private void Update()
    {
        if (questMuncul)
        {
            timelineQuest.SetActive(true);
        }
    }

}
