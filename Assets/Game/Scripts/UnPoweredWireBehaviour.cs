using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class UnPoweredWireBehaviour : MonoBehaviour
{
    UnPoweredWireStats unPoweredWireS;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        unPoweredWireS = gameObject.GetComponent<UnPoweredWireStats>();
    }

    // Update is called once per frame
    void Update()
    {
        ManageLight();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PoweredWireStats>())
        {
            PoweredWireStats poweredWireS = collision.GetComponent<PoweredWireStats>();
            if(poweredWireS.objectColor == unPoweredWireS.objectColor)
            {
                poweredWireS.connected = true;
                unPoweredWireS.connected = true;
                poweredWireS.connectedPosition = gameObject.transform.position;
                TaskDone.Instance.SwitchChange(1);
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<PoweredWireStats>())
        {
            PoweredWireStats poweredWireS = collision.GetComponent<PoweredWireStats>();
            if(poweredWireS.objectColor == unPoweredWireS.objectColor)
            {
                poweredWireS.connected = false;
                unPoweredWireS.connected = false;
                TaskDone.Instance.SwitchChange(-1);
            }

        }
    }

    void ManageLight()
    {
        if(unPoweredWireS.connected)
        {
            unPoweredWireS.poweredLight.SetActive(true);
            unPoweredWireS.unPoweredLight.SetActive(false);
        }
        else
        {
            unPoweredWireS.poweredLight.SetActive(false);
            unPoweredWireS.unPoweredLight.SetActive(true);
        }
    }
}
