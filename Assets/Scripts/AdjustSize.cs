using UnityEngine;

public class AdjustSize : MonoBehaviour {

    public void ChangeSize(GameObject containerPanel)
    {
        RectTransform tempTransform = GetComponent<RectTransform>();
        tempTransform.sizeDelta = new Vector2(containerPanel.GetComponent<SizeAdjuster>().Width, containerPanel.GetComponent<SizeAdjuster>().Height);
    }
}
