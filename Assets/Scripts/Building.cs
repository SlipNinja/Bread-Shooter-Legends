using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    private Transform lifebar;
    private Image lifeImg;
    private float currentHP;
    private float maxHP = 200f;
    
    void Start()
    {
        currentHP = maxHP;
        lifebar = transform.Find("LifeBar");
        lifeImg = lifebar.Find("front").GetComponent<Image>();
    }

    void Update()
    {
        UpdateLifebar();
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
            Destroy(transform.gameObject);
        }
    }
}
