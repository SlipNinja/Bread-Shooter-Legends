using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Controller controller;

    void Start()
    {
        controller = GameObject.Find("MainCamera").GetComponent<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("ENTER");
        controller.onUI = true;
        controller.ChangeCursor(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("EXIT");
        controller.onUI = false;
        controller.ChangeCursor(false);
    }
}
