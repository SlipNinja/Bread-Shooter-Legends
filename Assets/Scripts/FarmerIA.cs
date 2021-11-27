using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class FarmerIA : MonoBehaviour
{
    public Transform field;
    public GameObject Mills;

    private float gatheringDistance = 1f;
    private float grindingTime = 6f;
    private float farmingTime = 3f;
    private Growth growthScript;
    private NavMeshAgent navmesh;
    private bool farming, grinding;
    private Transform currentDest;
    private static Dictionary<Transform, bool> millDeposits = null;

    void Start()
    {
        navmesh = GetComponent<NavMeshAgent>();
        farming = true;
        grinding = false;

        growthScript = field.gameObject.GetComponent<Growth>();

        if(millDeposits is null)
        {
            Debug.Log("COUCOU");
            //Gathering deposit data
            millDeposits = new Dictionary<Transform, bool>();
            foreach (Transform mill in Mills.transform)
            {
                if(!mill.name.Contains("Mill"))
                {
                    continue;
                }

                foreach (Transform deposit in mill.transform)
                {
                    if(!deposit.name.Contains("deposit"))
                    {
                        continue;
                    }

                    millDeposits.Add(deposit, true);
                }
            }
        }
    }

    void Update()
    {
        if(farming)
        {
            if(!currentDest)
            {
                SetFarmingTarget();
            }
            
            else if(AtDestination())
            {
                if(growthScript.Gather(currentDest.gameObject, farmingTime))
                {
                    StartCoroutine(FarmingCoroutine());
                }

                else
                {
                    SetFarmingTarget();
                }
            }

            else
            {
                SetFarmingTarget();
            }
        }
        
        else if(grinding)
        {
            //navmesh.SetDestination(Mill.position);

            if(!currentDest)
            {
                SetGrindingTarget();
            }

            else if(AtDestination())
            {
                StartCoroutine(GrindingCoroutine());
            }

            else
            {
                SetGrindingTarget();
            }
        }
    }

    IEnumerator GrindingCoroutine()
    {
        grinding = false;
        millDeposits[currentDest] = false;
        
        yield return new WaitForSeconds(grindingTime);

        millDeposits[currentDest] = true;
        farming = true;
    }

    IEnumerator FarmingCoroutine()
    {   
        farming = false;

        Debug.Log(navmesh.avoidancePriority + " Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(farmingTime);
        Debug.Log(navmesh.avoidancePriority + " Finished Coroutine at timestamp : " + Time.time);

        grinding = true;
    }

    private void SetFarmingTarget()
    {
        currentDest = GetNearestPlantation();
        if(currentDest is null)
        {
            navmesh.ResetPath();
        } else {
            navmesh.SetDestination(currentDest.position);
        }
    }

    private void SetGrindingTarget()
    {
        currentDest = GetNearestFreeMillDeposit();
        if(currentDest is null)
        {
            navmesh.ResetPath();
        } else {
            navmesh.SetDestination(currentDest.position);
        }
    }

    bool AtDestination()
    {
        float dist = Vector3.Distance(transform.position, navmesh.destination);
        return (dist <= gatheringDistance);
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

    Transform GetNearestFreeMillDeposit()
    {
        
        if(millDeposits.Count <= 0)
        {
            Debug.Log("No deposits found");
            return null;
        }

        List<KeyValuePair<Transform, bool>> freeDeposits = millDeposits.Where(freeDeposit => freeDeposit.Value == true).ToList();

        if(freeDeposits.Count <= 0)
        {
            Debug.Log("No free deposit to grind");
            return null;
        }

        Debug.Log(freeDeposits.Count);

        Transform nearest = null;
        float mindist = 1000f;
        foreach (KeyValuePair<Transform, bool> deposit in freeDeposits)
        {
            float dist = Vector3.Distance(transform.position, deposit.Key.position);
            if(dist < mindist)
            {
                nearest = deposit.Key;
                mindist = dist;
            }
        }

        return nearest;
    }
}
