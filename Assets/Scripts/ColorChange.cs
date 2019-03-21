using UnityEngine;
using UnityEngine.UI;

public class ColorChange : MonoBehaviour {
    public string colorHierarchy;
    private Color color;

    private void Start()
    {
        if (GetComponentInParent<ColorPalette>() != null)
        {
            GetComponentInParent<ColorPalette>().itemsForChange.Add(gameObject);
        }
        else
        {
            transform.parent.GetComponentInChildren<ColorPalette>().itemsForChange.Add(gameObject);
        }
    }
    private void OnDestroy()
    {
        if (GetComponentInParent<ColorPalette>() != null)
        {
            GetComponentInParent<ColorPalette>().itemsForChange.Remove(gameObject);
        }
        else
        {
            try
            {
                transform.parent.GetComponentInChildren<ColorPalette>().itemsForChange.Remove(gameObject);

            }
            catch (System.NullReferenceException)
            {

            }
        }
    }
    public void ChangeColor()
    {
        color = GetComponent<Image>().color;
        if (colorHierarchy.Equals("primary"))
        {
            if (color != Color.green)
            {
                GetComponent<Image>().color = ColorPalette.GetPrimary();
            }
        }
        else if (colorHierarchy.Equals("secondary"))
        {
            if (color != Color.green)
            {
                GetComponent<Image>().color = ColorPalette.GetSecondary();
            }
        }
        else if (colorHierarchy.Equals("accent"))
        {
            if (color != Color.green)
            {
                GetComponent<Image>().color = ColorPalette.GetAccent();
            }
        }
    }  
}
