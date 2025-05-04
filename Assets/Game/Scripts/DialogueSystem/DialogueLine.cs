using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(2, 5)] public string text;
    public bool animateText;

    public bool istThought;
    //public Sprite portrait;
    public List<DialogueChoice> choices;
    //public DialogueObject nextDialogue;
}
