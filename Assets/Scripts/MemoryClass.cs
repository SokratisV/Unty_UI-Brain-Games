using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoryClass : MonoBehaviour {

    public bool amIActive = false;
    public static Sprite revealedIncorrectSprite;
    public static Sprite hiddenSprite;
    public static Sprite revealedCorrectSprite;
    private GameObject containerPanel;
    private static PopulateGrid populateScript;
    public static int howManyCorrectCards = 4;
    private static int correctCounter = 0;
    public static int score;
    public static int attempts;
    private TextMeshProUGUI attemptsText;
    private TextMeshProUGUI scoreText;
    private static bool allowScoreChanges = true;
    private new AudioSource audio;
    public AudioClip correctSound;
    public AudioClip incorrectSound;

    private void Start()
    {
        attemptsText = GameObject.FindGameObjectWithTag("AttemptsText").GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        containerPanel = GameObject.FindGameObjectWithTag("PanelContainer");
        if (populateScript == null)
        {
            populateScript = GetComponentInParent<PopulateGrid>();
        }
        audio = GetComponent<AudioSource>();
    }
    public void RevealCard()
    {
        if (amIActive)
        {
            GetComponent<Image>().sprite = revealedCorrectSprite;
            GetComponent<Button>().interactable = false;
            correctCounter++;
        }   
        else
        {
            IncreaseAttempts();
            RevealAllCards();
            StartCoroutine(RestartGame());
        }
        if (correctCounter == howManyCorrectCards)
        {
            WonRound();
        }
    }
    private void RevealAllCards()
    {
        audio.clip = incorrectSound;
        audio.Play();
        Transform temp;
        for (int i = 0; i < containerPanel.transform.childCount; i++)
        {
            temp = containerPanel.transform.GetChild(i);
            if (temp.GetComponentInChildren<MemoryClass>().amIActive)
            {
                temp.GetChild(0).GetComponent<Image>().sprite = revealedCorrectSprite;
            }
            else
            {
                temp.GetChild(0).GetComponent<Image>().sprite = revealedIncorrectSprite;
            }
            temp.GetComponentInChildren<Button>().interactable = false;
        }
    }
    private IEnumerator RestartGame()
    {
        correctCounter = 0;
        yield return new WaitForSeconds(2);
        populateScript.RestartGame();
    }
    private void WonRound()
    {
        IncreaseScore();
        IncreaseAttempts();
        audio.clip = correctSound;
        audio.Play();
        Transform temp;
        for (int i = 0; i < containerPanel.transform.childCount; i++)
        {
            temp = containerPanel.transform.GetChild(i);
            temp.GetComponentInChildren<Button>().interactable = false;
        }
        StartCoroutine(RestartGame());
    }
    public void ResetScoreAndAttempts()
    {
        ResetScore();
        ResetAttempts();
        StartCoroutine(IdleTime());
    }
    private void ResetScore()
    {
        score = 0;
        try
        {
            scoreText.GetComponent<ResetValue>().ResetToDefault();
        }
        catch (System.NullReferenceException)
        {
        }
    }
    private void ResetAttempts()
    {
        attempts = 0;
        try
        {
            attemptsText.GetComponent<ResetValue>().ResetToDefault();
        }
        catch (System.NullReferenceException)
        {

        }
    }
    private IEnumerator IdleTime()
    {
        allowScoreChanges = false;
        yield return new WaitForSeconds(1);
        allowScoreChanges = true;
    }
    private void IncreaseAttempts()
    {
        if (allowScoreChanges)
        {
            attempts++;
            attemptsText.text = "" + attempts;
        }
    }
    private void IncreaseScore()
    {
        if (allowScoreChanges)
        {
            score++;
            scoreText.text = "" + score;
        }
    }
}
