using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.SceneManagement;

//source: https://www.youtube.com/watch?v=vY0Sk93YUhA&ab_channel=ShapedbyRainStudios

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Canvas abilityPopUp;

    private Story currentStory;

    private bool hasGivenAbilityBefore = false;
    public bool dialogueIsPlaying;
    private static DialogueManager instance;
    private PlayerMovement player;

    private void Awake()
    {

        if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");

        }
        dialoguePanel.SetActive(false);

        instance = this;
        player = FindAnyObjectByType<PlayerMovement>();
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON, string name)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        nameText.text = name;

        ContinueStory();
    }

    public IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialoguePanel.SetActive(false);

        if (SceneManager.GetActiveScene().name == "Level_Final")
        {
            Debug.Log("Should open end UI");
            GameObject.Find("EndGameManager").GetComponent<EndGameManager>().displayEndGameUI();
        }

        if (!hasGivenAbilityBefore)
        {
            hasGivenAbilityBefore = true;
            player.getNewAbility();
        }
        else
        {
            dialogueIsPlaying = false;
        }
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
        }
        else
        {
            if (!abilityPopUp.enabled)
            {
                StartCoroutine(ExitDialogueMode());
            }
            
        }
    }
}
