using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Linq;

public class EnnemyIA : MonoBehaviour
{
    private Transform Mills;
    private Transform Farmers;
    private NavMeshAgent navmesh;
    private float range = 20f;
    private float rangeAttack = 3f;
    private Transform lifebar;
    private Image lifeImg;
    private float currentHP;
    private float maxHP = 150f;
    private Transform target;
    private bool canHit = true;
    private float attackDamage = 40f;
    private float attackSpeed = 2f;
    private float nextHit = 0.0f;
    InterfaceHandle inter;

    void Start()
    {
        inter = GameObject.Find("Interface").GetComponent<InterfaceHandle>();
        inter.AddEnnemy();
        navmesh = GetComponent<NavMeshAgent>();
        Farmers = GameObject.FindWithTag("farmers").transform;
        Mills = GameObject.FindWithTag("mills").transform;

        currentHP = maxHP;
        lifebar = transform.Find("LifeBar");
        lifeImg = lifebar.Find("front").GetComponent<Image>();
    }

    void Update()
    {
        UpdateLifebar();

        Transform closestFarmer = ClosestFarmerInRange();
        if(closestFarmer)
        {
            target = closestFarmer;
            navmesh.SetDestination(closestFarmer.position);
        }

        else
        {
            Transform closestMill = ClosestMill();
            if(closestMill)
            {
                Transform attackPoint = closestMill.Find("attackPoint");
                target = attackPoint;
                navmesh.SetDestination(attackPoint.position);
            }
        }

        if(!canHit)
        {
            if (Time.time > nextHit )
            {
                nextHit = Time.time + 1/attackSpeed;
                canHit = true;
            }
        }

        else if(CloseToTarget())
        {
            HitTarget();
            canHit = false;
        }
    }

    private bool CloseToTarget()
    {
        float dist = Vector3.Distance(transform.position, navmesh.destination);
        return (dist <= rangeAttack);
    }

    private void HitTarget()
    {
        if(!target)
        {
            Debug.Log("No target, abort hitting");
            return;
        }

        FarmerIA farmer = target.GetComponent<FarmerIA>();
        if(farmer)
        {
            farmer.GetHit(attackDamage);
            return;
        }

        Building building = target.parent.GetComponent<Building>();
        if(building)
        {
            building.GetHit(attackDamage);
            FarmerIA.InitDeposits();
        }
    }

    private Transform ClosestMill()
    {
        float minDist = 1000f;
        Transform nearestMill = null;
        foreach (Transform mill in Mills)
        {
            float dist = Vector3.Distance(transform.position, mill.position);
            if(dist < minDist)
            {
                minDist = dist;
                nearestMill = mill;
            }
        }
        return nearestMill;
    }

    private Transform ClosestFarmerInRange()
    {
        float minDist = 1000f;
        Transform nearestFarmer = null;
        foreach (Transform farmer in Farmers)
        {
            float dist = Vector3.Distance(transform.position, farmer.position);
            if(farmer.name.Contains("Farmer") && dist < range)
            {
                if(dist < minDist)
                {
                    minDist = dist;
                    nearestFarmer = farmer;
                }
            }
        }
        return nearestFarmer;
    }

    private void UpdateLifebar()
    {
        lifebar.rotation = Camera.main.transform.rotation;
        lifeImg.fillAmount = currentHP/maxHP;
    }

    public void GetShooted(float damages)
    {
        currentHP -= damages;
        if(currentHP <= 0)
        {
            currentHP = 0;
            inter.RemoveEnnemy();
            Destroy(gameObject);
        }
    }
}
