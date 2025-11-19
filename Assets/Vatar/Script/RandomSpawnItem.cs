using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawnItem : MonoBehaviour
{
    public Transform[] spawnPoint;

    private void Start()
    {
        int totalTitik = spawnPoint.Length;
        int randomIndex = Random.Range(0, totalTitik);

        Transform titikSpawnTerpilih = spawnPoint[randomIndex];

        transform.parent = titikSpawnTerpilih;
        transform.position = titikSpawnTerpilih.position;

    }

}
