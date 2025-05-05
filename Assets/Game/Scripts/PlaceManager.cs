using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlaceManager : MonoBehaviour
{
    public SpriteRenderer ok;
    public int acceptedID;
    private Vector3 connectPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        connectPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        var draggable = collision.GetComponent<DraggableObject>();
        if (draggable == null) return;

        if (draggable.objectID == acceptedID)
        {
            // Eşleşirse snap & dondur
            collision.transform.position = connectPos; 

            var rb = collision.GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeAll; 

            // Başarı işareti
            if (ok != null) ok.color = new UnityEngine.Color(0,1,0);    
            
            TaskManager.Instance.MarkTaskDone(acceptedID);
        }
        else
        {
            // Eşleşmezse geri gönder
            collision.transform.position = draggable.originalPosition;
        }
    }
}
