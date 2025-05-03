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

    public bool buttonTextAnimation;

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
        StartCoroutine(TypeLine(line.text));
    }

    void ShowChoices(List<DialogueChoice> choices)
    {
        choicesContainer.SetActive(true);

        foreach (DialogueChoice choice in choices)
        {
            GameObject buttonObj = Instantiate(choiceButtonPrefab, choicesContainer.transform);
            Button button = buttonObj.GetComponent<Button>();
            button.interactable = false;

            if (!buttonTextAnimation)
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
                    // Normal diyalo�a devam et
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
        }

        // Se�enek varsa g�ster
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

        // Butonu ilk ba�ta t�klanamaz yap
        button.interactable = false;

        // Her harfi s�rayla yaz
        for (int i = 0; i < fullText.Length; i++)
        {
            textComponent.text += fullText[i];
            yield return new WaitForSeconds(0.015f); // Yazma h�z�n� ayarlamak i�in
        }

        // Yaz� tamamland���nda buton t�klanabilir hale gelsin
        button.interactable = true;
    }
}
