using System.Collections.Generic;
using UnityEngine;


public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private bool hasTriggered = false;

    public void TriggerDialogue()
    {
        if (!hasTriggered) // 只有未触发过时才会触发对话
        {
            DialogueManager.Instance.StartDialogue(dialogue);
            hasTriggered = true; // 对话触发后将标志设置为 true
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            UnityEngine.Debug.Log("检查到碰撞体");
            TriggerDialogue();
        }
    }
}

[System.Serializable]
public class DialogueCharacter
{
    public string name;
    //public Sprite icon;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}