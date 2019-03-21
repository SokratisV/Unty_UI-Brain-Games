using UnityEngine;

public class SizeAdjuster : MonoBehaviour
{
    private float width;
    private float height;

    public float Width
    {
        get
        {
            return width;
        }

        set
        {
            width = value;
        }
    }
    public float Height
    {
        get
        {
            return height;
        }

        set
        {
            height = value;
        }
    }
}
