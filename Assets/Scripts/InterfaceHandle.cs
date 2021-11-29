using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceHandle : MonoBehaviour
{

    private int ammos = 6;
    private int weat = 10;
    private int farmers = 0;

    public int farmerCost = 5;
    public int ammoCost = 10;

    private Text buyFarmer;
    private Text buyAmmos;
    private Text ammosDisplay;
    private Text farmersDisplay;
    private Text weatDisplay;
    public bool hasLost = false;

    void Start()
    {
        buyFarmer = transform.Find("farmerButton").Find("CostText").GetComponent<Text>();
        buyAmmos = transform.Find("ammoButton").Find("CostText").GetComponent<Text>();
        ammosDisplay = transform.Find("ammoDisplay").Find("Text").GetComponent<Text>();
        farmersDisplay = transform.Find("farmerDisplay").Find("Text").GetComponent<Text>();
        weatDisplay = transform.Find("moneyDisplay").Find("Text").GetComponent<Text>();
    }

    void Update()
    {
        buyFarmer.text = "Cost : " + farmerCost.ToString();
        buyAmmos.text = "Cost : " + ammoCost.ToString();
        ammosDisplay.text = ammos.ToString();
        farmersDisplay.text = farmers.ToString();
        weatDisplay.text = weat.ToString();

        if(farmers <= 0 && weat < farmerCost)
        {
            hasLost = true;
            Time.timeScale = 0f;
            //deathMenu.SetActive(true);
        }
    }

    public void BuyAmmos()
    {
        if(weat >= ammoCost)
        {
            weat -= ammoCost;
            ammos += 6;
        }
    }

    public int GetFarmersCount()
    {
        return farmers;
    }

    public int GetAmmosCount()
    {
        return ammos;
    }

    public int GetWeatCount()
    {
        return weat;
    }

    public void AddWeat()
    {
        weat += 1;
    }

    public void AddFarmer()
    {
        farmers += 1;
    }

    public void AddAmmos()
    {
        ammos += 6;
    }

    public void RemoveAmmo()
    {
        ammos -= 1;
    }

    public void RemoveFarmer()
    {
        farmers -= 1;
    }

    public void RemoveWeat(int quantity = 1)
    {
        weat -= quantity;
        if(weat < 0)
        {
            weat = 0;
        }
    }
}
