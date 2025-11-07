using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPlayer : MonoBehaviour
{
    public GameObject letakPisau;
    public GameObject letakPel;
    public Outline[] outlinePisau;
    public Outline[] outlinePel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pisau"))
        {
            letakPisau.SetActive(true);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Pel"))
        {
            letakPel.SetActive(true);
            Destroy(other.gameObject);
        }
    }

    public void GrabObject(string bendaApa)
    {
        if (bendaApa == "Pisau")
        {
            foreach (var objek in outlinePisau)
            {
                objek.eraseRenderer = false;
            }
        }else if (bendaApa == "Pel")
        {
            foreach (var objek in outlinePel)
            {
                objek.eraseRenderer = false;
            }
        }
    }

    public void DropObject(string bendaApa)
    {
        if (bendaApa == "Pisau")
        {
            foreach (var objek in outlinePisau)
            {
                objek.eraseRenderer = true;
            }
        }else if (bendaApa == "Pel")
        {
            foreach (var objek in outlinePel)
            {
                objek.eraseRenderer = true;
            }
        }
    }
}
