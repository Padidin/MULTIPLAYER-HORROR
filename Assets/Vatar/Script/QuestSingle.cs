using cakeslice;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestSingle : MonoBehaviour
{
    public Transform InspectHolder;

    [Header("Objek Di Meja")]
    public GameObject letakPisau;
    public GameObject letakPel;
    public GameObject letakVHS1;
    public GameObject letakVHS2;
    public GameObject letakFoto;
    public GameObject letakJurnal;

    [Header("Item Objektif")]
    public GameObject Pisau;
    public GameObject Pel;
    public GameObject VHS1;
    public GameObject VHS2;
    public GameObject Foto;
    public GameObject Jurnal;

    [Header("Outline")]
    public Outline[] outlinePisau;
    public Outline[] outlinePel;
    public Outline[] outlineVHS1;
    public Outline[] outlineVHS2;
    public Outline[] outlineFoto;
    public Outline[] outlineJurnal;

    [Header("Kondisi Item")]
    public bool itemPertama = false;
    public bool itemKedua = false;
    public bool itemKetiga = false;
    public bool itemKeempat = false;
    public bool itemKelima = false;
    public bool itemKeenam = false;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Pisau)
        {
            Pisau.transform.position = letakPisau.transform.position;
            Pisau.transform.rotation = letakPisau.transform.rotation;
            itemPertama = true;
        }

        if (other.gameObject == Pel)
        {
            Pel.transform.position = letakPel.transform.position;
            Pel.transform.rotation = letakPel.transform.rotation;
            itemKedua = true;
        }

        if (other.gameObject == VHS1)
        {
            VHS1.transform.position = letakVHS1.transform.position;
            VHS1.transform.rotation = letakVHS1.transform.rotation;
            itemKetiga = true;
        }

        if (other.gameObject == VHS2)
        {
            itemKeempat = true;
        }

        if (other.gameObject == Foto)
        {
            itemKelima = true;
        }

        if (other.gameObject == Jurnal)
        {
            itemKeenam = true;
        }
    }

    private void Update()
    {
        statusOutlineItem();
        if (itemPertama && itemKedua && itemKetiga && itemKeempat && itemKelima && itemKeenam)
        {

        }
    }

    void statusOutlineItem()
    {
        if (InspectHolder.childCount > 0)
        {
            GameObject children = InspectHolder.GetChild(0).gameObject;

            if (children == Pisau)
            {
                foreach (Outline line in outlinePisau)
                {
                    line.eraseRenderer = false;
                }
            }

            if (children == Pel)
            {
                foreach (Outline line in outlinePel)
                {
                    line.eraseRenderer = false;
                }
            }

            if (children == VHS1)
            {
                foreach (Outline line in outlineVHS1)
                {
                    line.eraseRenderer = false;
                }
            }

            if (children == VHS2)
            {
                foreach (Outline line in outlineVHS2)
                {
                    line.eraseRenderer = false;
                }
            }

            if (children == Foto)
            {
                foreach (Outline line in outlineFoto)
                {
                    line.eraseRenderer = false;
                }
            }

            if (children == Jurnal)
            {
                foreach (Outline line in outlineJurnal)
                {
                    line.eraseRenderer = false;
                }
            }
        }
        else
        {
            foreach (Outline line in outlinePisau)
            {
                line.eraseRenderer = true;
            }

            foreach (Outline line in outlinePel)
            {
                line.eraseRenderer = true;
            }

            foreach (Outline line in outlineVHS1)
            {
                line.eraseRenderer = true;
            }

            foreach (Outline line in outlineVHS2)
            {
                line.eraseRenderer = true;
            }

            foreach (Outline line in outlineFoto)
            {
                line.eraseRenderer = true;
            }

            foreach (Outline line in outlineJurnal)
            {
                line.eraseRenderer = true;
            }
        }

    }
}
