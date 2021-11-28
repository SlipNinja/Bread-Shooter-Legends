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
    private InterfaceHandle inter;
    void Start()
    {
        inter = GameObject.Find("Interface").GetComponent<InterfaceHandle>();
        spawn = transform.Find("shrine");
        spawnPos = spawn.position + new Vector3(0, 1f, 0);

        CreateNewFarmer(true);
    }

    void Update()
    {
        
    }

    public void CreateNewFarmer(bool force = false)
    {
        if((inter.GetWeatCount() >= inter.farmerCost) || force)
        {
            inter.RemoveWeat(inter.farmerCost);
            GameObject newFarmer = Instantiate(farmerPrefab, spawnPos, Quaternion.identity);
            newFarmer.GetComponent<FarmerIA>().field = field;
            newFarmer.GetComponent<NavMeshAgent>().avoidancePriority = priority;
            newFarmer.transform.SetParent(farmers);
            
            priority = (priority >= 99) ? 1 : priority + 1;

            inter.AddFarmer();
        }
    }

    public void CreateNewFarmer()
    {
        CreateNewFarmer(false);
    }
}
