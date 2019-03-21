using UnityEngine;
using TMPro;

public class ResetValue : MonoBehaviour {

    public string resetValue;

    public void ResetToDefault()
    {
        GetComponent<TextMeshProUGUI>().text = resetValue;
    }
    public void ResetToDefault(string value)
    {
        GetComponent<TextMeshProUGUI>().text = value;
    }
}
