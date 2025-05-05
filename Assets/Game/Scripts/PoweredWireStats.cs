using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Color { blue, red, yellow, purple};
public class PoweredWireStats : MonoBehaviour
{

    public bool movable = false;
    public bool moving = false;
    public Vector3 startPosition;
    public Color objectColor;
    public bool connected = false;
    public Vector3 connectedPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
