using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossBarUI : MonoBehaviour
{
    public static CrossBarUI instane;

    public bool TouchItem;
    public GameObject UICross;
    public GameObject UIHand;

    private void Awake()
    {
        instane = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TouchItem)
        {
            UIHand.SetActive(true);
            UICross.SetActive(false);
        }
        else
        {
            UIHand.SetActive(false);
            UICross.SetActive(true);
        }
    }
}
