using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGridWord : MonoBehaviour {

    public string[] words;
    public GameObject wordPanel;
    public GameObject alphabetPanel;
    public GameObject letterButtonPrefab;
    public GameObject letterLabelPrefab;
    public AudioClip correctSound;
    public AudioClip incorrectSound;
    public AudioClip victory;
    public string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private new AudioSource audio;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI attemptsText;
    private int score;
    private int numberOfAttempts;
    private int indexOfWord;
    private int howManyLetters;
    private float letterWidth;
    private float minWidth = 475;
    private float maxWidth = 600;
    private float minHeight = 550;
    private float maxHeight = 800;
    private int howManylettersFound;
    private bool stopInput;

    private void Update()
    {
        if (!stopInput)
        {
            foreach (char c in Input.inputString)
            {
                if (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z')
                {
                    //CheckForLetterInWord(c);
                    foreach (TextMeshProUGUI item in alphabetPanel.GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        if (item.text.Equals(c.ToString().ToUpper()))
                        {
                            item.GetComponentInParent<Button>().onClick.Invoke();
                        }
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Restart();
        }
    }
    private void Start()
    {
        audio = GetComponent<AudioSource>();
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        attemptsText = GameObject.FindGameObjectWithTag("AttemptsText").GetComponent<TextMeshProUGUI>();
        GetRandomWord();
        PopulateWordPanel();
        PopulateAlphabetPanel();
        CalculatePanelSize();
        howManylettersFound = 0;
        stopInput = false;
    }
    private void Restart()
    {
        GetComponentInParent<SwitchGame>().RestartCurrent(); 
    }
    private void RemovePrevious()
    {
        for (int i = 0; i < wordPanel.transform.childCount; i++)
        {
            Destroy(wordPanel.transform.GetChild(i).gameObject);
        }
    }
    private void GetRandomWord()
    {
        indexOfWord = Random.Range(0, words.Length);
        howManyLetters = 0;
    }
    private void PopulateWordPanel()
    {
        GameObject temp = null;
        foreach (char character in words[indexOfWord])
        {
            temp = Instantiate(letterLabelPrefab, wordPanel.transform);
            temp.GetComponentInChildren<TextMeshProUGUI>().text = "_";
            howManyLetters++;
        }
        if (temp != null)
        {
            letterWidth = temp.GetComponent<RectTransform>().rect.width;
        }
        else
        {
            Debug.Log("Instantiation problem @ PopulateWordPanel");
        }
    }
    private void PopulateAlphabetPanel()
    {
        GameObject temp;
        foreach (char character in alphabet)
        {
            temp = Instantiate(letterButtonPrefab, alphabetPanel.transform);
            temp.GetComponentInChildren<TextMeshProUGUI>().text = "" + character;
            temp.GetComponent<Button>().onClick.AddListener(() => CheckForLetterInWord(character));
        }
    }
    private void CheckForLetterInWord(char letter)
    {
        char tempCharacter;
        letter = char.ToUpper(letter);
        bool correct = false;

        for (int i = 0; i < words[indexOfWord].Length; i++)
        {
            tempCharacter = char.ToUpper(words[indexOfWord][i]);
            if (letter.Equals(tempCharacter))
            {
                string text = wordPanel.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text;
                if (text == "_")
                {
                    wordPanel.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = "" + letter;
                    howManylettersFound++;
                }
                correct = true;
            }
        }
        if (correct)
        {
            audio.clip = correctSound;
            IncreaseScore();
            IncreaseAttemptsCount();
        }
        else
        {
            audio.clip = incorrectSound;
            IncreaseAttemptsCount();
        }
        audio.Play();
        DeactivateLetter(letter.ToString(), correct);
        CheckForVictory();
    }
    private void DeactivateLetter(string letter, bool correct)
    {
        foreach (TextMeshProUGUI item in alphabetPanel.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (item.text.Equals(letter))
            {
                item.GetComponentInParent<Button>().interactable = false;
                item.GetComponentInParent<Button>().onClick.RemoveAllListeners();
                if (correct)
                {
                    item.GetComponentInParent<Image>().color = Color.green;
                }
                else
                {
                    item.GetComponentInParent<Image>().color = Color.red;
                }
            }
        }
    }
    private void CheckForVictory()
    {
        if (howManylettersFound == howManyLetters)
        {
            StartCoroutine(Win());
        }
    }
    private void CalculatePanelSize()
    {
        float tempWidth = letterWidth * howManyLetters + 100;
        float tempHeight = tempWidth;
        if (tempWidth < minWidth)
        {
            tempWidth = minWidth;
        }
        else if (tempWidth > maxWidth)
        {
            tempWidth = maxWidth;
            AdjustLabelSize();
        }
        if (tempHeight < minHeight)
        {
            tempHeight = minHeight;
        }
        else if (tempHeight > maxHeight)
        {
            tempHeight = maxHeight;
        }
        GetComponent<SizeAdjuster>().Width = tempWidth;
        GetComponent<SizeAdjuster>().Height = tempHeight;
    }
    private void AdjustLabelSize()
    {
        Transform temp = transform.GetChild(0); //child 0 must be the "Word" panel
        Vector2 newSize = new Vector2
        {
            x = maxWidth / howManyLetters
        };
        newSize.y = newSize.x;
        for (int i = 0; i < temp.childCount; i++)
        {
            temp.GetChild(i).GetComponent<RectTransform>().sizeDelta = newSize;
        }
    }
    private IEnumerator Win()
    {
        stopInput = true;
        audio.clip = victory;
        audio.Play();
        yield return new WaitForSeconds(2);
        Restart();
        stopInput = false;
    }
    public void IncreaseScore()
    {
        score++;
        scoreText.text = "" + score;
    }
    public void IncreaseAttemptsCount()
    {
        numberOfAttempts++;
        attemptsText.text = "" + numberOfAttempts;
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
    }

}
