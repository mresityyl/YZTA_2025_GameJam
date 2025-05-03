using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    public List<DialogueLine> lines;
}
