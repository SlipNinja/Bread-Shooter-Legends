using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Linq;

public class FarmerIA : MonoBehaviour
{
    public Transform field;

    private static GameObject Mills;
    private float gatheringDistance = 2f;
    private float grindingDistance = 2f;
    private float grindingTime = 3f;
    private float farmingTime = 1f;
    private Growth growthScript;
    private NavMeshAgent navmesh;
    private bool farming, grinding;
    private Transform currentDest;
    private static Dictionary<Transform, bool> millDeposits = null;
    private Transform lifebar;
    private Image lifeImg;
    private float currentHP;
    private float maxHP = 100f;
    private InterfaceHandle inter;

    void Start()
    {
        inter = GameObject.Find("Interface").GetComponent<InterfaceHandle>();

        currentHP = maxHP;
        lifebar = transform.Find("LifeBar");
        lifeImg = lifebar.Find("front").GetComponent<Image>();

        Mills = GameObject.FindWithTag("mills");

        navmesh = GetComponent<NavMeshAgent>();
        farming = true;
        grinding = false;

        growthScript = field.gameObject.GetComponent<Growth>();

        if(millDeposits is null)
        {
            InitDeposits();
        }
    }

    public static void InitDeposits()
    {
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

    void Update()
    {
        UpdateLifebar();

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

    private void UpdateLifebar()
    {
        lifebar.rotation = Camera.main.transform.rotation;
        lifeImg.fillAmount = currentHP/maxHP;
    }

    public void GetHit(float damages)
    {
        currentHP -= damages;
        if (currentHP <= 0)
        {
            currentHP = 0;
            inter.RemoveFarmer();
            Destroy(transform.gameObject);
        }
    }

    IEnumerator GrindingCoroutine()
    {
        grinding = false;
        millDeposits[currentDest] = false;

        yield return new WaitForSeconds(grindingTime);

        inter.AddWeat();
        millDeposits[currentDest] = true;
        farming = true;
    }

    IEnumerator FarmingCoroutine()
    {   
        farming = false;

        yield return new WaitForSeconds(farmingTime);

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

        if(grinding)
        {
            return (dist <= grindingDistance);
        }

        else if(farming)
        {
            return (dist <= gatheringDistance);
        }

        else
        {
            Debug.Log("Has target but no state");
            return false;
        }
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

        //Debug.Log(freeDeposits.Count);

        Transform nearest = null;
        float mindist = 1000f;
        foreach (KeyValuePair<Transform, bool> deposit in freeDeposits)
        {
            if(deposit.Key)
            {
                float dist = Vector3.Distance(transform.position, deposit.Key.position);
                if(dist < mindist)
                {
                    nearest = deposit.Key;
                    mindist = dist;
                }
            }
        }

        return nearest;
    }
}
