using UnityEngine;

public class MuteSounds : MonoBehaviour {

    public Camera gameCamera;
    private bool muted = false;

    public void Mute()
    {
        if (muted)
        {
            foreach (AudioSource item in GetComponentsInChildren<AudioSource>())
            {
                item.volume = 1f;
            }
        }
        else
        {
            foreach (AudioSource item in GetComponentsInChildren<AudioSource>())
            {
                item.volume = 0f;
            }
        }
        muted = !muted;
    }
}
