using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FarmerIA : MonoBehaviour
{

    public float gatheringDistance = 1f;
    public float grindingTime = 3f;
    public Transform field;
    public Transform Mill;

    private NavMeshAgent navmesh;
    private bool farming, grinding;
    private Transform currentDest;

    void Start()
    {
        navmesh = GetComponent<NavMeshAgent>();
        farming = true;
        grinding = false;
    }

    void Update()
    {
        if(farming)
        {
            if(!currentDest)
            {
                currentDest = GetNearestPlantation();
                if(currentDest is null)
                {
                    return;
                } else {
                    navmesh.SetDestination(currentDest.position);
                }
            }

            if(AtDestination())
            {
                Destroy(currentDest.gameObject);
                farming = false;
                grinding = true;
            }
        }
        
        else if(grinding)
        {
            navmesh.SetDestination(Mill.position);

            if(AtDestination())
            {
                StartCoroutine(GrindingCoroutine());
                grinding = false;
            }
        }

        
    }

    IEnumerator GrindingCoroutine()
    {
        yield return new WaitForSeconds(grindingTime);

        farming = true;
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
