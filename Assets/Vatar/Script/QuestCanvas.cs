using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestCanvas : MonoBehaviour
{
    public bool questMuncul;
    public GameObject timelineQuest;

    public TextMeshProUGUI textQuest;
    public string textName;
    public int itemTerkumpul;
    public int maxItem = 6;

    private void Start()
    {
        timelineQuest.SetActive(false);

        itemTerkumpul = 0;
        UpdateUI();
    }
    private void Update()
    {
        if (questMuncul)
        {
            timelineQuest.SetActive(true);
        }
    }

    public void AddProgress(int jumlah)
    {
        itemTerkumpul += jumlah;

        UpdateUI();
    }

    public void UpdateUI()
    {
        textQuest.text = $"{textName} ({itemTerkumpul}) ";

    }
}
