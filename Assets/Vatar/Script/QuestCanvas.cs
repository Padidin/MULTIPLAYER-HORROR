using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestCanvas : MonoBehaviour
{
    public TextMeshProUGUI textQuest;
    public string textName;
    public int itemTerkumpul;
    public int maxItem = 6;
    private void Start()
    {
        itemTerkumpul = 0;

        UpdateUI();
    }

    private void Update()
    {

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
