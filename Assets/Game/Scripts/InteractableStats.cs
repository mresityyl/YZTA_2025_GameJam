using UnityEngine;

public class InteractableStats : MonoBehaviour
{
    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public Vector3 snapPosition;
    [HideInInspector] public bool connected = false;

    void Start()
    {
        // Başlangıç pozisyonunu sakla
        startPosition = transform.position;
    }
}