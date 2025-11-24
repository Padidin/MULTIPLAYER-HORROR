using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;

public class QuestCanvas : MonoBehaviour
{
    public bool questMuncul;
    public PlayableDirector timelineQuest;

    public TextMeshProUGUI textQuest;
    public string textName;
    public int itemTerkumpul;
    public int maxItem = 6;

    private void Start()
    {
        timelineQuest.Stop();

        itemTerkumpul = 0;
        UpdateUI();
    }
    private void Update()
    {
        if (questMuncul)
        {
            timelineQuest.Play();
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
