using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MemoryClassv1 : MonoBehaviour{

    private Sprite imageSprite;
    private Sprite hiddenSprite;
    private bool isItHidden = true;
    private static int counter;
    private static bool blockedFromClicking;
    private static GameObject[] openCards;
    private static int openCardsIndex;
    public AudioClip correctSound;
    public AudioClip incorrectSound;
    public AudioClip victorySound;
    private new AudioSource audio;
    private static int score;
    private static int numberOfAttempts;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI attemptsText;
    private static bool allowScoreChanges;

    private void Start()
    {
        scoreText = GameObject.FindWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        attemptsText = GameObject.FindWithTag("AttemptsText").GetComponent<TextMeshProUGUI>();
        openCards = new GameObject[2];
        GetComponent<Image>().sprite = hiddenSprite;
        audio = GetComponent<AudioSource>();
        ResetScore();
        ResetAttempts();
        blockedFromClicking = false;
        openCardsIndex = 0;
        allowScoreChanges = true;
        counter = 0;
    }
    public void ToggleBetweenSprites()
    {
        if (!blockedFromClicking)
        {
            if (isItHidden)
            {
                GetComponentInChildren<Image>().sprite = imageSprite;
                isItHidden = false;
                counter++;
                openCards[GetNextOpenCardIndex()] = this.gameObject;
            }
        }
        if (counter >= 2 && !blockedFromClicking)
        {
            blockedFromClicking = true;
            StartCoroutine(CloseOpenCards());
        }

    }
    private int GetNextOpenCardIndex()
    {
        if (openCardsIndex == 2)
        {
            openCardsIndex = 0;
        }
        return openCardsIndex++;
    }
    private void SwitchToHidden()
    {
        GetComponentInChildren<Image>().sprite = hiddenSprite;
        isItHidden = true;
    }
    public Sprite ImageSprite
    {
        get
        {
            return imageSprite;
        }

        set
        {
            imageSprite = value;
        }
    }
    public Sprite HiddenSprite
    {
        get
        {
            return hiddenSprite;
        }

        set
        {
            hiddenSprite = value;
        }
    }
    private IEnumerator CloseOpenCards()
    {
        yield return new WaitForSeconds(1);
        counter = 0;
        if (allowScoreChanges)
        {
            IncreaseAttemptsCount();
            CheckForSameCard();
        }
        blockedFromClicking = false;
    }
    private void CheckForSameCard()
    {

        if (openCards[0].GetComponent<MemoryClassv1>().imageSprite == openCards[1].GetComponent<MemoryClassv1>().imageSprite)
        {
            audio.clip = correctSound;
            openCards[0].transform.parent.GetComponent<Image>().color = Color.green;
            openCards[1].transform.parent.GetComponent<Image>().color = Color.green;
            openCards[0].GetComponent<Button>().interactable = false;
            openCards[1].GetComponent<Button>().interactable = false;
            IncreaseScore();
        }
        else
        {
            audio.clip = incorrectSound;
            openCards[0].GetComponent<MemoryClassv1>().SwitchToHidden();
            openCards[1].GetComponent<MemoryClassv1>().SwitchToHidden();
        }
        if (allowScoreChanges)
        {
            audio.Play();
            if (score == 8)
            {
                StartCoroutine(WonTheGame());
            }
        }

    }
    public void IncreaseScore()
    {
        if (allowScoreChanges)
        {
            score++;
            scoreText.text = "" + score;
        }
    }
    public void IncreaseAttemptsCount()
    {
        if (allowScoreChanges)
        {
            numberOfAttempts++;
            attemptsText.text = "" + numberOfAttempts;
        }
    }
    public void ResetScore()
    {
        score = 0;
        scoreText.GetComponent<ResetValue>().ResetToDefault();

    }
    public void ResetAttempts()
    {
        numberOfAttempts = 0;
        attemptsText.GetComponent<ResetValue>().ResetToDefault();

    }
    public void ResetScoreAndAttempts()
    {
        ResetScore();
        ResetAttempts();
        StartCoroutine(IdleTime());
    }
    private IEnumerator WonTheGame()
    {
        yield return new WaitForSeconds(1);
        audio.clip = victorySound;
        audio.Play();
    }
    public void ResetThisCard()
    {
        SwitchToHidden();
        transform.parent.GetComponent<Image>().color = ColorPalette.GetAccent();
        GetComponent<Button>().interactable = true;
    }
    private IEnumerator IdleTime()
    {
        allowScoreChanges = false;
        yield return new WaitForSeconds(1);
        allowScoreChanges = true;
    }
}
