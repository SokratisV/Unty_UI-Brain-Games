using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ColorPalette : MonoBehaviour {

    public Color[] colors;
    private int colorIndex = 0;
    private static Color[] colorPalette = new Color[3];
    public List<GameObject> itemsForChange;

    private void Start()
    {
        StartCoroutine(AdjustNextColors());
    }
    public static Color[] GetColorPalette()
    {
        return colorPalette;
    }
    public static Color GetPrimary()
    {
        return colorPalette[0];
    }
    public static Color GetSecondary()
    {
        return colorPalette[1];
    }
    public static Color GetAccent()
    {
        return colorPalette[2];
    }
    private void GetNextColorPalette()
    {
        for (int i = 0; i < 3; i++)
        {
            colorPalette[i] = colors[colorIndex++];
        }
        if (colorIndex >= colors.Length)
        {
            colorIndex = 0;
        }
    }
    public void AdjustColors()
    {
        foreach (GameObject item in itemsForChange)
        {
            item.GetComponent<ColorChange>().ChangeColor();
        }
    }
    public void AdjustNextColorsButton()
    {
        GetNextColorPalette();
        foreach (GameObject item in itemsForChange)
        {
            item.GetComponent<ColorChange>().ChangeColor();
        }
    }
    private IEnumerator AdjustNextColors()
    {
        yield return new WaitForEndOfFrame();
        GetNextColorPalette();
        foreach (GameObject item in itemsForChange)
        {
            item.GetComponent<ColorChange>().ChangeColor();
        }
    }
}
