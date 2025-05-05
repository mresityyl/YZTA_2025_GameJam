using UnityEngine;

public class Draggable: MonoBehaviour
{
    private bool mouseDown = false;
    private bool movable = false;
    private float zDist;
    private InteractableStats stats;

    void Start()
    {
        stats = GetComponent<InteractableStats>();
        // Kamera ile nesne arası mesafeyi alıyoruz
        zDist = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
    }

    void Update()
    {
        MoveObject();
    }

    void OnMouseDown()
    {
        mouseDown = true;
    }

    void OnMouseOver()
    {
        // Üstüne gelince taşınabilir hâle getir
        movable = true;
    }

    void OnMouseExit()
    {
        // Eğer taşınmıyorsa, çıkınca taşınamaz yap
        if (!mouseDown) movable = false;
    }

    void OnMouseUp()
    {
        mouseDown = false;
        // Yerleşmemişse startPosition'a dön
        if (!stats.connected)
            transform.position = stats.startPosition;
        else
            transform.position = stats.snapPosition;
    }

    private void MoveObject()
    {
        if (mouseDown && movable)
        {
            // Fareye takip ettir
            Vector3 mousePoint = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                zDist
            );
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePoint); 
            worldPos.z = transform.position.z;
            transform.position = worldPos;
        }
    }
}