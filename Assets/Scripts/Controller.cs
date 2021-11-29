using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    
    public bool onUI = false;

    public Texture2D cursorTexture;

    private float dragSpeed = 50f;
    private float zoomSpeed = 3f;
    private float ClickDeltaTime = 0.15f;
    private float downClickTime = 0f;
    private float damages = 50f;

    private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 cursorOffset;
    private InterfaceHandle inter;
    private Transform cameraTransform;
    private Camera cam;
    private bool click = false;
    private float nextShot = 0.0f;
    private float shotCooldown = 0.5f;
 
    void Start()
    {
        inter = GameObject.Find("Interface").GetComponent<InterfaceHandle>();

        cameraTransform = transform.Find("MainCamera");
        cam = cameraTransform.GetComponent<Camera>();

        cursorOffset = new Vector2(cursorTexture.width/2, cursorTexture.height/2);

        ChangeCursor(false);
    }
 
    void Update()
    {
        ZoomLevel();

        if (Input.GetMouseButtonDown (0))
        {
            downClickTime = Time.time;
        }

        if (Input.GetMouseButtonUp (0))
        {
            click = false;
            if(Time.time - downClickTime <= ClickDeltaTime)
            {
                click = true;
            }

            if(Time.time > nextShot && !onUI && click)
            {
                nextShot = Time.time + shotCooldown;
                Shoot();
            }
        }

        if(Input.GetMouseButton(0))
        {
            if(Time.time - downClickTime > ClickDeltaTime)
            {
                float mouseX = Input.GetAxis("Mouse X") * dragSpeed * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * dragSpeed * Time.deltaTime;

                //mouseX = Mathf.Clamp(mouseX, -50, 50);
                //mouseY = Mathf.Clamp(mouseY, -50, 50);

                Vector3 newPos = new Vector3(mouseX, 0, mouseY);

                transform.Translate(-newPos);

                float newX = Mathf.Clamp(transform.position.x, -100f, 50f);
                float newZ = Mathf.Clamp(transform.position.z, -80f, 20f);

                Vector3 clampedPos = new Vector3(newX, transform.position.y, newZ);

                transform.position = clampedPos;
            }
        }
    }

    private Collider RaycastAtMouse()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 1000f))
        {
            //Debug.Log(hit.collider.name);
            return hit.collider;
        }

        return null;
    }

    private void ZoomLevel()
    {
        float scrollDelta = Input.mouseScrollDelta.y * zoomSpeed;
        float newY = Mathf.Clamp(transform.position.y - scrollDelta, 30f, 120f);

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void Shoot()
    {
        if(inter.GetAmmosCount() <= 0)
        {
            // No ammo
            inter.BlankShotSound();
            return;
        }

        inter.ShotSound();
        inter.RemoveAmmo();

        Collider collider = RaycastAtMouse();

        if(collider)
        {
            if(collider.name.Contains("Ennemy"))
            {
                collider.GetComponent<EnnemyIA>().GetShooted(damages);
            }

            else if(collider.name.Contains("Farmer"))
            {
                collider.GetComponent<FarmerIA>().GetHit(damages);
            }
        }
    }

    public void ChangeCursor(bool reset)
    {
        if(reset)
        {
            Cursor.SetCursor(null, cursorOffset, cursorMode);
            return;
        }

        Cursor.SetCursor(cursorTexture, cursorOffset, cursorMode);
    }
}
