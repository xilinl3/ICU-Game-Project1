using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    //public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;
    private Queue<DialogueLine> lines;
    public bool isDialogueActive = false;
    public float typingSpeed = 1.0f;
    public Animator animator;
    
    public Canvas DialogueCanvas;
    private bool isTyping = false; 
    private bool isSentenceComplete = false;
    private string currentSentence = ""; // 当前正在显示的句子
    private float timeBetweenLines = 1.0f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        lines = new Queue<DialogueLine>();
        if (DialogueCanvas.gameObject.activeSelf)
        {
            DialogueCanvas.gameObject.SetActive(false);
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;
        DialogueCanvas.gameObject.SetActive(true);

        //animator.Play("show");

        lines.Clear();

        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        //characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;
        currentSentence = currentLine.line;

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        isTyping = true; // 设置为正在打字
        isSentenceComplete = false; // 句子尚未完成
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        isSentenceComplete = true;

        // 如果玩家没有按下空格键，自动等待几秒后显示下一行对话
        yield return new WaitForSeconds(timeBetweenLines);
        if (isSentenceComplete) // 再次检查是否已经完成
        {
            DisplayNextDialogueLine();
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        DialogueCanvas.gameObject.SetActive(false);
        //animator.Play("hide");
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // 如果正在逐字显示，直接显示整行
                StopAllCoroutines();
                dialogueArea.text = currentSentence; // 直接显示当前完整句子
                isTyping = false; // 停止逐字显示
                isSentenceComplete = true; // 设置句子已完成显示
            }
            else if (isSentenceComplete)
            {
                // 如果句子已经完成显示，跳到下一行对话
                DisplayNextDialogueLine();
            }
        }
    }
}