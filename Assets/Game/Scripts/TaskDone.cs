using UnityEngine;

public class TaskDone : MonoBehaviour
{
    static public TaskDone Instance;

    public int count;
    public bool done = false;
    private int onCount = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Instance = this;
    }

    public void SwitchChange(int points)
    {
        onCount = onCount + points;
        print(onCount);
        if(onCount == count)
        {
            done = true;
        }
        else done = false;
    }
}
