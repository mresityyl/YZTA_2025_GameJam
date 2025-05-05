using UnityEngine;

public class SetWorldBoundries : MonoBehaviour
{
    void Awake()
    {
        var bounds = GetComponent<SpriteRenderer>().bounds;
        Globals.worldBounds = bounds;
    }
}
