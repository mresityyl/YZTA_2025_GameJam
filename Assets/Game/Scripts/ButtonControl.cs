using UnityEngine;

public class ButtonControl : MonoBehaviour
{
    public GameObject pcbPrefab;
    public GameObject screwPrefab;
    private int pcbCount = 0;
    private int screwCount = 0;
    private const int MaxPCBs = 1;
    private const int MaxScrews = 4;
    private Camera mainCamera;
     private UnityEngine.Vector3 spawnOffset = new UnityEngine.Vector3(-10, -1, 3);
    void Start()
    {
        mainCamera = Camera.main;
    }
    public void OnClick_InsertPCB()
    {
        if(pcbCount < MaxPCBs)
        {
            UnityEngine.Vector3 pos = mainCamera.transform.position + spawnOffset;
            Instantiate(pcbPrefab, pos, Quaternion.Euler(0f, 0f, 90f));
            pcbCount++;
        }

    }
    public void OnClick_InsertScrew()
    {
        if (screwCount < MaxScrews)
        {
            UnityEngine.Vector3 pos = mainCamera.transform.position + spawnOffset;
            Instantiate(screwPrefab, pos, UnityEngine.Quaternion.identity);
            screwCount++;
        }
    }
    public void OnObjectDestroyed(GameObject obj)
    {
        if (obj.CompareTag("PCB")) pcbCount = Mathf.Max(0, pcbCount - 1);
        if (obj.CompareTag("Screw")) screwCount = Mathf.Max(0, screwCount - 1);
    }
}
