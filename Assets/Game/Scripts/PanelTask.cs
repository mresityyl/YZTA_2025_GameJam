using System;
using System.Collections;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class PanelTask : MonoBehaviour
{
    private int taskID = 3;
    public GameObject task;
    private TaskDone taskDone;
    public PlayerMovement move;
    public GameObject mainCamera;
    public SpriteRenderer ok;
    public ParticleSystem panelParticle;
    private GameObject currentTask;
    private UnityEngine.Vector3 taskPosition;
    private UnityEngine.Quaternion taskRotation;
    bool play = true;

    // Update is called once per frame
    void Update()
    {
        taskPosition = new UnityEngine.Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - 1, mainCamera.transform.position.z + 3);
        taskRotation = UnityEngine.Quaternion.identity;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && play)
        {
            move = collision.gameObject.GetComponent<PlayerMovement>();
            if(currentTask == null)
            {
                currentTask = Instantiate(task,taskPosition,taskRotation,mainCamera.transform);
                Transform bg = currentTask.transform.Find("Background");
                taskDone = bg.GetComponent<TaskDone>();
                move.move = false;

                StartCoroutine(WatchTaskCompletion());
            }
        }
        
    }
    private IEnumerator WatchTaskCompletion()
    {
        // taskDone.done = true olana kadar bekle
        yield return new WaitUntil(() => taskDone.done);

        panelParticle.Stop();

        // Paneli yok et ve karakteri serbest bırak
        Destroy(currentTask);
        move.move = true;

        // Işığı yeşile çevir
        ok.color = new UnityEngine.Color(0,1,0);
        TaskManager.Instance.MarkTaskDone(taskID);

        // Bir daha tetiklenmesin
        play = false;
    }
}
