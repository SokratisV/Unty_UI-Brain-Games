using UnityEngine;
using UnityEngine.UI;

public class PopulateGridv1 : MonoBehaviour {

    public GameObject prefab;
    public int numberOfGridCells;
    public Sprite[] crystalSprites;
    public Sprite hiddenCardSprite;
    public Sprite[] spriteArray;
    private int nextSpriteCounter = 0;

    void Start () {
        spriteArray = new Sprite[crystalSprites.Length * 2];
        ArrangeArray();
        Populate();
        CalculatePanelSize();
    }
    private Sprite NextSprite()
    {
        return spriteArray[nextSpriteCounter++];
    }
    private void Populate()
    {
        GameObject tempObj;

        for (int i = 0; i < numberOfGridCells; i++)
        {
            tempObj = Instantiate(prefab, transform);
            tempObj.name = "Nr" + i;
            tempObj.GetComponentInChildren<MemoryClassv1>().ImageSprite = NextSprite();
            tempObj.GetComponentInChildren<MemoryClassv1>().HiddenSprite = hiddenCardSprite;
        }
    }
    public void RestartGame()
    {
        GetComponentInChildren<MemoryClassv1>().ResetScoreAndAttempts();
        nextSpriteCounter = 0;
        ArrangeArray();
        foreach (MemoryClassv1 item in GetComponentsInChildren<MemoryClassv1>())
        {
            item.ImageSprite = NextSprite();
            item.ResetThisCard();
        }
    }
    private void ArrangeArray()
    {
        //Add every sprite twice
        for (int i = 0; i < crystalSprites.Length; i++)
        {
            spriteArray[i] = crystalSprites[i];
            spriteArray[spriteArray.Length - i - 1] = crystalSprites[i];
        }
        //Randomize array
        for (int t = 0; t < spriteArray.Length; t++)
        {
            Sprite tmp = spriteArray[t];
            int r = Random.Range(t, spriteArray.Length);
            spriteArray[t] = spriteArray[r];
            spriteArray[r] = tmp;
        }
    }
    public void QuitGame()
    {
        Application.Quit();
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
