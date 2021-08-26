using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region Variables;
    private static DialogueManager instance;

    public Text nameText;
    public Text dialogueText;

    public Animator animator = null;

    private Queue<string> sentences;

    public event Action OnStartDialogue;
    public event Action OnEndDialogue;
    #endregion Variables

    #region Properties
    public static DialogueManager Instance => instance;
    #endregion Properties

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        OnStartDialogue?.Invoke();

        animator?.SetBool("IsOpen", true);

        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
            sentences.Enqueue(sentence);

        DisplayNextSentence();
    } 

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            OnEndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();

        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = string.Empty;

        yield return new WaitForSeconds(0.25f);

        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        animator?.SetBool("IsOpen", false);

        OnEndDialogue?.Invoke();
    }
}
