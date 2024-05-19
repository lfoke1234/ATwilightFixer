using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue info;

    public void Trigger()
    {
        var system = FindObjectOfType<DialogueSystem>();
        system.Begin(info);
    }
}
