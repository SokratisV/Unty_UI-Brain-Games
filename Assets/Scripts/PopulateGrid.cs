using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopulateGrid : MonoBehaviour {

    public GameObject prefab;
    public Sprite revealeadCorrectImage;
    public Sprite revealeadIncorrectImage;
    public Sprite hiddenImage;
    public int[] gamePattern; //public to see correct cards in inspector
    private int length;
    public float revealSpeed = 0.5f;
    private float revealedTime = 0.3f;
    private float countDownTimer = 3f;
    public TextMeshProUGUI countDownText;
    private IEnumerator currentCoroutine;

    void Start () {
        MemoryClass.revealedCorrectSprite = revealeadCorrectImage;
        MemoryClass.revealedIncorrectSprite = revealeadIncorrectImage;
        MemoryClass.hiddenSprite = hiddenImage;
        length = GetComponent<GridLayoutGroup>().constraintCount;
        length *= length;
        gamePattern = new int[length];
        RandomizeArray();
        Populate();
        if (countDownText == null)
        {
            countDownText = GameObject.FindWithTag("CountDown").GetComponent<TextMeshProUGUI>();
        }
        GetComponentInChildren<MemoryClass>().ResetScoreAndAttempts();
        CalculatePanelSize();
    }
    private void Populate()
    {
        GameObject temp;

        for (int i = 0; i < length; i++)
        {
            temp = Instantiate(prefab, transform);
            temp.name = "Number" + i;
            temp.transform.GetChild(0).GetComponent<Image>().sprite = hiddenImage;
            temp.GetComponentInChildren<Button>().interactable = false;
            if (gamePattern[i] < MemoryClass.howManyCorrectCards)
            {
                temp.GetComponentInChildren<MemoryClass>().amIActive = true;
            }
        }
        currentCoroutine = ShowCorrect();
        StartCoroutine(currentCoroutine);
    }
    //public void Repopulate()
    //{
    //    Debug.Log("REPOP");
    //    RandomizeArray();
    //    countDownTimer = 3;
    //    Transform temp;

    //    for (int i = 0; i < length; i++)
    //    {
    //        temp = transform.GetChild(i);
    //        temp.GetChild(0).GetComponent<Image>().sprite = hiddenImage;
    //        if (gamePattern[i] < MemoryClass.howManyCorrectCards)
    //        {
    //            temp.GetComponentInChildren<MemoryClass>().amIActive = true;
    //        }
    //        else
    //        {
    //            temp.GetComponentInChildren<MemoryClass>().amIActive = false;
    //        }
    //        temp.GetComponentInChildren<Button>().interactable = false;
    //    }
    //    StartCoroutine(ShowCorrect());
    //}
    private void RandomizeArray()
    {
        int temp;
        for (int i = 0; i < length; i++)
        {
            gamePattern[i] = i;
        }
        for (int i = 0; i < length; i++)
        {
            int rnd = Random.Range(0, length);
            temp = gamePattern[rnd];
            gamePattern[rnd] = gamePattern[i];
            gamePattern[i] = temp;
        }
    }
    private IEnumerator ShowCorrect()
    {
        Transform temp;
        if (countDownText == null)
        {
            yield return new WaitForEndOfFrame();
        }

        countDownText.text = "" + countDownTimer--;
        yield return new WaitForSeconds(1);
        countDownText.text = "" + countDownTimer--;
        yield return new WaitForSeconds(1);
        countDownText.text = "" + countDownTimer--;
        yield return new WaitForSeconds(1);
        countDownText.text = "" + countDownTimer;
        
        for (int i = 0; i < length; i++)
        {
            if (gamePattern[i] < MemoryClass.howManyCorrectCards)
            {
                transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = revealeadCorrectImage;
                yield return new WaitForSeconds(revealSpeed);
            }
        }
        yield return new WaitForSeconds(revealedTime);
        countDownText.text = "Go!";

        for (int i = 0; i < length; i++)
        {
            temp = transform.GetChild(i);
            if (gamePattern[i] < MemoryClass.howManyCorrectCards)
            {
                temp.GetChild(0).GetComponent<Image>().sprite = hiddenImage;
            }
            temp.GetComponentInChildren<Button>().interactable = true;
        }
        yield return new WaitForSeconds(.5f);
        countDownText.text = "";
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void RestartGame()
    {
        StopTheCoroutine();
        RandomizeArray();
        Transform temp;

        for (int i = 0; i < length; i++)
        {
            temp = transform.GetChild(i);
            temp.GetChild(0).GetComponent<Image>().sprite = hiddenImage;
            if (gamePattern[i] < MemoryClass.howManyCorrectCards)
            {
                temp.GetComponentInChildren<MemoryClass>().amIActive = true;
            }
            else
            {
                temp.GetComponentInChildren<MemoryClass>().amIActive = false;
            }
            temp.GetComponentInChildren<Button>().interactable = false;
        }
        currentCoroutine = ShowCorrect();
        StartCoroutine(currentCoroutine); 
    }
    private void StopTheCoroutine()
    {
        StopCoroutine(currentCoroutine);
        countDownText.text = "";
        countDownTimer = 3;
    }
    private void CalculatePanelSize()
    {
        GridLayoutGroup gridScript = GetComponent<GridLayoutGroup>();
        float middlePanelHeight = gridScript.cellSize.y * gridScript.constraintCount + gridScript.spacing.y * gridScript.constraintCount;
        float middlePanelWidth = gridScript.cellSize.x * gridScript.constraintCount + gridScript.spacing.x * gridScript.constraintCount;
        float upperPanelHeight = middlePanelHeight / 4;
        float lowerPanelHeight = upperPanelHeight;
        float tempHeight = middlePanelHeight + upperPanelHeight + lowerPanelHeight;
        GetComponent<SizeAdjuster>().Height = tempHeight;
        GetComponent<SizeAdjuster>().Width = middlePanelWidth;
    }
}
