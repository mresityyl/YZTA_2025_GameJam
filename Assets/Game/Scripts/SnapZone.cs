using System.Collections.Generic;
using UnityEngine;

public class SnapZone : MonoBehaviour
{
    [SerializeField] private string acceptedTag;
    private static HashSet<InteractableStats> connectedSet = new HashSet<InteractableStats>();
    void OnTriggerEnter2D(Collider2D other)
    {
        var drag = other.GetComponent<Draggable>();
        var stats = other.GetComponent<InteractableStats>();
        if (other.gameObject.CompareTag(acceptedTag))
        {
            if (drag != null && stats != null)
            {
                if (connectedSet.Add(stats))  // Add(): true ise yeni ekleme oldu 
                {
                    stats.snapPosition = transform.position;
                    stats.connected    = true;
                    other.transform.position = stats.snapPosition;

                    // Sayaç artışı
                    TaskDone.Instance.SwitchChange(1);
            
                    // Rigidbody varsa dondur
                    var rb = other.GetComponent<Rigidbody2D>();
                    if (rb) rb.constraints = RigidbodyConstraints2D.FreezeAll;
                } 
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        // 1. Sadece doğru tag'li objelere bak
        if (!collision.CompareTag(acceptedTag))
            return; // diğeriyle işimiz yok :contentReference[oaicite:4]{index=4}

        // 2. InteractableStats al
        var stats = collision.GetComponent<InteractableStats>();
        if (stats == null)
            return;

        // 3. Eğer gerçekten bağlı set’te varsa, çıkar ve sayaç azalt
        if (connectedSet.Remove(stats))  // Remove(): true ise listeden çıkarıldı :contentReference[oaicite:5]{index=5}
        {
            stats.connected = false;
            TaskDone.Instance.SwitchChange(-1);
        }
    }
}
