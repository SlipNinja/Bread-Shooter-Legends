using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerSpawner : MonoBehaviour
{
    public GameObject farmerPrefab;
    public Transform field;
    public Transform farmers;
    private Transform spawn;
    private Vector3 spawnPos;
    void Start()
    {
        spawn = transform.Find("shrine");
        spawnPos = spawn.position + new Vector3(0, 1f, 0);

        CreateNewFarmer();
    }

    void Update()
    {
        
    }

    public void CreateNewFarmer()
    {
        GameObject newFarmer = Instantiate(farmerPrefab, spawnPos, Quaternion.identity);
        newFarmer.GetComponent<FarmerIA>().field = field;
        newFarmer.transform.SetParent(farmers);
    }
}
