using System.Collections;
using TMPro;
using UnityEngine;

public class SwitchGame : MonoBehaviour {
    public TextMeshProUGUI[] texts;
    public GameObject[] prefabGames;
    private int whichPrefabIsActive = 0;
    private GameObject activeMiddlePanel;
    public TextMeshProUGUI gameNameText;

    private void Awake()
    {
        activeMiddlePanel = Instantiate(prefabGames[whichPrefabIsActive], transform);
        activeMiddlePanel.name = prefabGames[whichPrefabIsActive].name;
        gameNameText.text = "Game: " + activeMiddlePanel.name;
        StartCoroutine(EndOfFrame());
    }
    public void ChangeGame()
    {
        Destroy(activeMiddlePanel);
        whichPrefabIsActive++;
        whichPrefabIsActive = whichPrefabIsActive % prefabGames.Length;
        activeMiddlePanel = Instantiate(prefabGames[whichPrefabIsActive], transform);
        activeMiddlePanel.name = prefabGames[whichPrefabIsActive].name;
        ResetUIText();
        gameNameText.text = "Game: " + activeMiddlePanel.name;
        StartCoroutine(EndOfFrame());
    }
    public void RestartCurrent()
    {
        Destroy(activeMiddlePanel);
        activeMiddlePanel = Instantiate(prefabGames[whichPrefabIsActive], transform);
        activeMiddlePanel.name = prefabGames[whichPrefabIsActive].name;
        gameNameText.text = "Game: " + activeMiddlePanel.name;
        StartCoroutine(EndOfFrame());
    }
    private void ResetUIText()
    {
        foreach (TextMeshProUGUI item in texts)
        {
            item.GetComponent<ResetValue>().ResetToDefault();
        }
    }
    private IEnumerator EndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        GetComponent<AdjustSize>().ChangeSize(activeMiddlePanel);
        yield return new WaitForEndOfFrame();
        GetComponent<ColorPalette>().AdjustColors();
    }
}
