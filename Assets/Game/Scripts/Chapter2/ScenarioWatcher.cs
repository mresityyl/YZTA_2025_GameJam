using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ScenarioWatcher : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public bool isImageActive;
    public GameObject imageToActivate;
    private void Update()
    {
        //Müþteriyi getir
        if (!isImageActive && messageText.text.StartsWith("Ah,"))
        {
            imageToActivate.SetActive(true);
        }
    } 
}
