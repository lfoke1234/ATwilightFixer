using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] GameObject startButton;
    public TMP_Text txtName;
    public TMP_Text txtSentence;
    Queue<string> sentences = new Queue<string>();
    public Animator anim;

    private string currentSentence = "";
    private bool isTyping = false;

    public void Begin(Dialogue info)
    {
        anim.SetBool("isOppen", true);
        sentences.Clear();
        txtName.text = info.name;
        foreach (var sentence in info.sentence)
        {
            sentences.Enqueue(sentence);
        }
        Next();
    }

    public void Next()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            CompleteSentence();
            isTyping = false;
        }
        else if (sentences.Count > 0)
        {
            StartTypingSentence(sentences.Dequeue());
        }
        else
        {
            End();
        }
    }


    void StartTypingSentence(string sentence)
    {
        currentSentence = sentence;
        txtSentence.text = string.Empty;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        foreach (var item in sentence)
        {
            txtSentence.text += item;
            yield return new WaitForSeconds(0.02f);
        }
        isTyping = false;
    }

    void CompleteSentence()
    {
        StopAllCoroutines();
        txtSentence.text = currentSentence;
    }


    private void End()
    {
        txtSentence.text = string.Empty;
        startButton.SetActive(true);
        anim.SetBool("isOppen", false);
    }
}
