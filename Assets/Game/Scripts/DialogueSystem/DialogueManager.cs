using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject choicesContainer;
    public GameObject choiceButtonPrefab;

    DialogueLine line;

    public DialogueObject firstDialog;
    private DialogueObject currentDialogue;
    private int currentLineIndex;

    void Start()
    {
        StartDialogue(firstDialog);
    }

    public void StartDialogue(DialogueObject dialogue)
    {
        currentDialogue = dialogue;
        currentLineIndex = 0;
        ShowLine();
    }

    void ShowLine()
    {
        line = currentDialogue.lines[currentLineIndex];

        speakerNameText.text = line.speakerName;

        StopAllCoroutines();
        if (line.animateText)
        {
            dialogueText.text = "";
            StartCoroutine(TypeLine(line.text));
        }
        else
        {
            dialogueText.text = line.text;

            foreach (Transform child in choicesContainer.transform)
                Destroy(child.gameObject);
            // Seçenek varsa göster
            if (line.choices != null && line.choices.Count > 0)
            {
                ShowChoices(line.choices);
            }
            else
            {
                choicesContainer.SetActive(false);
            }
        }
    }

    void ShowChoices(List<DialogueChoice> choices)
    {
        choicesContainer.SetActive(true);

        foreach (DialogueChoice choice in choices)
        {
            GameObject buttonObj = Instantiate(choiceButtonPrefab, choicesContainer.transform);
            Button button = buttonObj.GetComponent<Button>();

            if(currentDialogue.lines[currentLineIndex].istThought)
                buttonObj.GetComponent<Image>().color = new Color32(241, 213, 174, 255);

            button.interactable = false;

            if (!currentDialogue.lines[currentLineIndex].istThought)
            {
                button.interactable = true;
                buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = choice.choiceText;
            }
            else
            {
                TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
                StartCoroutine(TypeButtonText(buttonText, choice.choiceText, button));
            }

            buttonObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (choice.canPlayPuzzle)
                {
                    SceneManager.LoadScene(choice.nextScene);
                }
                else
                {
                    // Normal diyaloða devam et
                    StartDialogue(choice.nextDialogue);
                }
            });
        }
    }

    IEnumerator TypeLine(string lines)
    {
        foreach (Transform child in choicesContainer.transform)
            Destroy(child.gameObject);

        dialogueText.text = "";

        foreach (char c in lines)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(0.02f);
            //yield return new WaitForSeconds(0.001f);

        }

        // Seçenek varsa göster
        if (line.choices != null && line.choices.Count > 0)
        {
            ShowChoices(line.choices);
        }
        else
        {
            choicesContainer.SetActive(false);
        }
    }
    IEnumerator TypeButtonText(TextMeshProUGUI textComponent, string fullText, Button button)
    {
        textComponent.text = "";

        // Butonu ilk baþta týklanamaz yap
        button.interactable = false;

        // Her harfi sýrayla yaz
        for (int i = 0; i < fullText.Length; i++)
        {
            textComponent.text += fullText[i];
            yield return new WaitForSeconds(0.015f);
            //yield return new WaitForSeconds(0.001f);
        }

        // Yazý tamamlandýðýnda buton týklanabilir hale gelsin
        button.interactable = true;
    }
}
