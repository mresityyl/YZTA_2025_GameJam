using System;
using System.Collections;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TaskManager : MonoBehaviour
{

    [SerializeField] 
    private TMP_Text taskCounterText;   // Drag your TaskCounterText here in the Inspector 
    
    public GameObject panelUI;
    private int completedpush = 0;
    private int totalpush = 3;
    private int completedpanel = 0;
    private int totalpanel = 1;  
    public static TaskManager Instance { get; private set; }
    [Tooltip("0–2: place puzzles, 3: cable puzzle")]
    private bool[] tasksDone = new bool[4];
    public GameObject task;
    [SerializeField] private GameObject toolsBar;
    private TaskDone taskDone;
    public PlayerMovement move;
    private GameObject currentTask;
    private Camera mainCamera;
    private UnityEngine.Vector3 spawnOffset = new UnityEngine.Vector3(0, -1, 3);
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        mainCamera = Camera.main;
    }
    void Start()
    {
        // Initialize display
        UpdateTaskCounterUI();
    }
    public void MarkTaskDone(int taskID)
    {
        if (taskID < 0 || taskID >= tasksDone.Length) return;
        if (tasksDone[taskID]) return;           // Zaten tamamlandıysa atla

        tasksDone[taskID] = true;
        Debug.Log($"Task {taskID} done!");
    	if(taskID >= 0 && taskID <= 2)
	    {
	        completedpush = Mathf.Clamp(completedpush + 1, 0, totalpush);
            UpdateTaskCounterUI();
	    }
	    else
	    {
	        completedpanel = Mathf.Clamp(completedpanel + 1, 0, totalpanel);
            UpdateTaskCounterUI();
	    }

        // Tüm görevler tamamlandı mı?
        if (AllTasksCompleted())
            StartCoroutine(InstantiateScrewPuzzle());
    }

    private bool AllTasksCompleted()
    {
        foreach (var done in tasksDone)
            if (!done) return false;
        return true;
    }
    private void UpdateTaskCounterUI()
    {
        // Using C# string interpolation to form "X/Y"
        taskCounterText.text = $"Yapilmasi Gerekenler:\n1. Yerlerinden cikmis olan elektronik bilesenleri yerlerine yerlestir. ({completedpush}/{totalpush})\n2. Elektrik kacagi olan yeri bul ve tamir et. ({completedpanel}/{totalpanel})";
    }

    private IEnumerator InstantiateScrewPuzzle()
    {
        // İsterseniz küçük bir bekleme koyabilirsiniz
        yield return new WaitForSecondsRealtime(0.5f);

        UnityEngine.Vector3 pos = mainCamera.transform.position + spawnOffset;
        currentTask = Instantiate(task, pos, UnityEngine.Quaternion.identity);
        panelUI.SetActive(false);
        toolsBar.SetActive(true);
        Transform bg = currentTask.transform.Find("BackGround");
        taskDone = bg.GetComponent<TaskDone>();
        move.move = false;
        Debug.Log("Screw puzzle instantiated!");

        StartCoroutine(WatchGameCompletion());
    }
    private IEnumerator WatchGameCompletion()
    {
        yield return new WaitUntil(() => taskDone.done);

        Scenecontroller.Instance.LoadNextScene("Chapter1End");

    }
}
