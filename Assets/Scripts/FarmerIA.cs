using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FarmerIA : MonoBehaviour
{

    public float gatheringDistance = 1f;
    public float grindingTime = 1f;
    public float farmingTime = 1f;
    public Transform field;
    public Transform Mill;

    private Growth growthScript;
    private NavMeshAgent navmesh;
    private bool farming, grinding;
    private Transform currentDest;

    void Start()
    {
        navmesh = GetComponent<NavMeshAgent>();
        farming = true;
        grinding = false;

        growthScript = field.gameObject.GetComponent<Growth>();
    }

    void Update()
    {
        if(farming)
        {
            if(!currentDest)
            {
                SetTarget();
            }
            
            else if(AtDestination())
            {
                growthScript.Gather(currentDest.gameObject);
                StartCoroutine(FarmingCoroutine());
            }

            else
            {
                SetTarget();
            }
        }
        
        else if(grinding)
        {
            navmesh.SetDestination(Mill.position);

            if(AtDestination(3f))
            {
                StartCoroutine(GrindingCoroutine());
            }
        }
    }

    IEnumerator GrindingCoroutine()
    {
        grinding = false;
        
        yield return new WaitForSeconds(grindingTime);

        farming = true;
    }

    IEnumerator FarmingCoroutine()
    {   
        farming = false;

        yield return new WaitForSeconds(farmingTime);

        grinding = true;
    }

    private void SetTarget()
    {
        currentDest = GetNearestPlantation();
        if(currentDest is null)
        {
            return;
        } else {
            navmesh.SetDestination(currentDest.position);
        }
    }

    bool AtDestination()
    {
        return AtDestination(gatheringDistance);
    }

    bool AtDestination(float range)
    {
        float dist = Vector3.Distance(transform.position, navmesh.destination);
        return (dist <= range);
    }

    Transform GetNearestPlantation()
    {
        
        if(field.childCount <= 0)
        {
            return null;
        }

        Transform nearest = null;
        float mindist = 1000f;
        foreach (Transform child in field)
        {
            if (child.name.Contains("Weat"))
            {
                float dist = Vector3.Distance(transform.position, child.position);
                if(dist < mindist)
                {
                    nearest = child;
                    mindist = dist;
                }
            }
        }

        return nearest;
    }
}
