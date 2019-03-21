using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GenerateQuestions : MonoBehaviour {

    public TextMeshProUGUI[] buttons;
    public AudioClip correctSound;
    public AudioClip incorrectSound;
    public TextMeshProUGUI equationText;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI attemptsText;
    private int score;
    private int attempts;
    private AudioSource audioSource;
    public string[] expressions;
    public int[] answers;
    private int equationIndex;
    private int correctButtonIndex;

	private void Start () {
        audioSource = GetComponent<AudioSource>();
        GetRandomEquationIndex();
        SetEquationText();
        SetButtonAnswers();
        scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        attemptsText = GameObject.FindGameObjectWithTag("AttemptsText").GetComponent<TextMeshProUGUI>();
        CalculatePanelSize();
    }
    public void CheckAnswer(int buttonNumber)
    {
        IncreaseAttempts();
        if (buttonNumber == correctButtonIndex)
        {
            audioSource.clip = correctSound;
            IncreaseScore();
        }
        else
        {
            audioSource.clip = incorrectSound;
        }
        audioSource.Play();
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == correctButtonIndex)
            {
                buttons[i].GetComponentInParent<Image>().color = Color.green;
            }
            else
            {
                buttons[i].GetComponentInParent<Image>().color = Color.red;
            }
            buttons[i].GetComponentInParent<Button>().interactable = false;
        }
        StartCoroutine(GetNewEquation());
    }
    private IEnumerator GetNewEquation()
    {
        yield return new WaitForSeconds(2);
        ResetButtons();
        GetRandomEquationIndex();
        SetButtonAnswers();
        SetEquationText();
    }
    private void IncreaseAttempts()
    {
        attempts++;
        attemptsText.text = "" + attempts;
    }
    private void IncreaseScore()
    {
        score++;
        scoreText.text = "" + score;
    }
    private void ResetScore()
    {
        score = 0;
        scoreText.text = "" + score;
    }
    private void ResetAttempts()
    {
        attempts = 0;
        attemptsText.text = "" + attempts;
    }
    public void ResetScoreAndAttempts()
    {
        ResetScore();
        ResetAttempts();
    }
    private void ResetButtons()
    {
        foreach (TextMeshProUGUI item in buttons)
        {
            item.GetComponentInParent<Button>().interactable = true;
            item.GetComponentInParent<Image>().color = ColorPalette.GetAccent();
        }
    }
    private void SetButtonAnswers()
    {
        int answer = answers[equationIndex]; //the answer to the equation
        correctButtonIndex = Random.Range(0, 3); //choose random button to assign correct answer
        int[] answerDeviations = GetDeviations();
        int deviationCounter = 0;
        answerDeviations[0] += answer;
        answerDeviations[1] += answer;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == correctButtonIndex)
            {
                buttons[i].text = "" + answer;
            }
            else
            {
                buttons[i].text = "" + answerDeviations[deviationCounter++];
            }
        }
        //switch (correctButtonIndex)
        //{
        //    default:
        //        break;
        //    case 0:
        //        answer1.text = "" + answer;
        //        answer2.text = "" + answerDeviations[0];
        //        answer3.text = "" + answerDeviations[1];
        //        break;
        //    case 1:
        //        answer2.text = "" + answer;
        //        answer1.text = "" + answerDeviations[0];
        //        answer3.text = "" + answerDeviations[1];
        //        break;
        //    case 2:
        //        answer3.text = "" + answer;
        //        answer1.text = "" + answerDeviations[0];
        //        answer2.text = "" + answerDeviations[1];
        //        break;
        //}
    }
    private int[] GetDeviations()
    {
        int[] deviations = new int[2];
        int[] numbersToChooseFrom = { -3, -2, -1, 1, 2, 3 };
        deviations[0] = numbersToChooseFrom[Random.Range(0, numbersToChooseFrom.Length)];
        while (true)
        {
            deviations[1] = numbersToChooseFrom[Random.Range(0, numbersToChooseFrom.Length)];
            if (deviations[1] != deviations[0])
            {
                break;
            }
        }
        return deviations;
    }
    private void GetRandomEquationIndex()
    {
        equationIndex = Random.Range(0, expressions.Length);
    }
    private void SetEquationText()
    {
        equationText.text = expressions[equationIndex] + "=?";
    }
    private void CalculatePanelSize()
    {
        GetComponent<SizeAdjuster>().Width = 550;
        GetComponent<SizeAdjuster>().Height = 500;
    }
    ////NOT IN USE
    //private string GetRandomExpression()
    //{
    //    string s = "";
    //    if (expressions.Length > 2)
    //    {
    //        int rand = Random.Range(0, expressions.Length - 1);
    //        foreach (char item in expressions[rand])
    //        {
    //            if (item.Equals('N'))
    //            {
    //                s += Random.Range(0, 20);
    //            }
    //            else if (item.Equals('O'))
    //            {
    //                s += GetRandomOperator();
    //            }
    //        }
    //    }
    //    return s;
    //}
    ////NOT IN USE
    //private string GetRandomOperator()
    //{
    //    int ran = Random.Range(0, 4);
    //    switch (ran)
    //    {
    //        case 0:
    //            return "+";

    //        case 1:
    //            return "-";

    //        case 2:
    //            return "*";
    //        case 3:
    //            return "/";
    //    }
    //    return "ERROR";
    //}
}
