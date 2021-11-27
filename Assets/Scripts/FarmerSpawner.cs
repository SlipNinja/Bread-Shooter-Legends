using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class FarmerSpawner : MonoBehaviour
{
    public GameObject farmerPrefab;
    public Transform field;
    public Transform farmers;
    private Transform spawn;
    private Vector3 spawnPos;
    private int priority = 1;
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
        newFarmer.GetComponent<NavMeshAgent>().avoidancePriority = priority;
        newFarmer.transform.SetParent(farmers);
        
        priority = (priority >= 99) ? 1 : priority + 1;
    }
}
