using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredWireBehavour : MonoBehaviour
{
    bool mouseDown = false;
    public PoweredWireStats powerWireS;
    LineRenderer line;
    private float zDist;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        powerWireS = gameObject.GetComponent<PoweredWireStats>();
        line = gameObject.GetComponentInParent<LineRenderer>();
        zDist = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        MoveWire();
        line.SetPosition(2,new Vector3(gameObject.transform.localPosition.x -.1f, gameObject.transform.localPosition.y, 0));
        line.SetPosition(1,new Vector3(gameObject.transform.localPosition.x -.4f, gameObject.transform.localPosition.y, 0));
    }
    void OnMouseDown()
    {
        mouseDown = true;
    }
    void OnMouseOver()
    {
        powerWireS.movable =true;
    }
    void OnMouseExit()
    {
        if(!powerWireS.moving)
            powerWireS.movable =false;
    }
    void OnMouseUp()
    {
        mouseDown = false;
        if(!powerWireS.connected)
            gameObject.transform.localPosition = powerWireS.startPosition;
        if(powerWireS.connected)
            gameObject.transform.position = powerWireS.connectedPosition;
    }
    void MoveWire()
    {
        if(mouseDown && powerWireS.movable)
        {
            powerWireS.moving =true;
            float mouseX = Input.mousePosition.x;
            float mouseY = Input.mousePosition.y;

            gameObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mouseX, mouseY, zDist));
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, transform.parent.transform.position.z);
        }
        else 
            powerWireS.moving = false;
    }
}
