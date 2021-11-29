using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InterfaceHandle : MonoBehaviour
{

    private int ammos = 6;
    private int weat = 8;
    private int farmers = 0;
    private int ennemies = 0;
    private int wave = 0;
    private int waveMax = 0;

    public int farmerCost = 4;
    public int ammoCost = 8;
    public bool hasLost = false;
    public AudioSource shotSound;
    public AudioSource blankShotSound;

    private Text buyFarmer;
    private Text buyAmmos;
    private Text waveCount;
    private Text ennemiesCount;
    private Text ammosDisplay;
    private Text farmersDisplay;
    private Text weatDisplay;
    private Transform ennemiesDisplay;

    void Start()
    {
        //shotSound = GetComponent<AudioSource>();        
        ennemiesDisplay = transform.Find("ennemiesDisplay");
        waveCount = ennemiesDisplay.Find("waveText").GetComponent<Text>();
        ennemiesCount = ennemiesDisplay.Find("ennemiesText").GetComponent<Text>();
        buyFarmer = transform.Find("farmerButton").Find("CostText").GetComponent<Text>();
        buyAmmos = transform.Find("ammoButton").Find("CostText").GetComponent<Text>();
        ammosDisplay = transform.Find("ammoDisplay").Find("Text").GetComponent<Text>();
        farmersDisplay = transform.Find("farmerDisplay").Find("Text").GetComponent<Text>();
        weatDisplay = transform.Find("moneyDisplay").Find("Text").GetComponent<Text>();
    }

    void Update()
    {
        farmerCost = 4 + (int)(farmers/4);
        buyFarmer.text = "Cost : " + farmerCost.ToString();
        buyAmmos.text = "Cost : " + ammoCost.ToString();
        ammosDisplay.text = ammos.ToString();
        farmersDisplay.text = farmers.ToString();
        weatDisplay.text = weat.ToString();

        ennemiesCount.text = ennemies.ToString() + " ennemies left";
        waveCount.text = "WAVE " + wave.ToString() + " / " + waveMax.ToString();

        if(ennemies > 0)
        {
            ennemiesDisplay.gameObject.SetActive(true);
        } else {
            ennemiesDisplay.gameObject.SetActive(false);
        }

        if(farmers <= 0 && weat < farmerCost)
        {
            hasLost = true;
            //Time.timeScale = 0f;
            SceneManager.LoadScene("defeatScene");
        }
    }

    public void ShotSound()
    {
        shotSound.Play();
    }

    public void BlankShotSound()
    {
        blankShotSound.Play();
    }

    public void SetMaxWave(int max)
    {
        waveMax = max;
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

    public void AddEnnemy()
    {
        ennemies++;
    }

    public void RemoveEnnemy()
    {
        ennemies--;
    }

    public void IncrementWave()
    {
        wave ++;
    }
}
