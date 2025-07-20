using UnityEngine;
public class UIReveal : MonoBehaviour
{
    public bool startHidden = true;
    public bool revealOnAwake = false;

    [Header("Movement")]
    public bool movement = true;

    public enum RevealDirection
    {
        Bottom, Left, Right, Top
    }

    public RevealDirection direction;
    public float offset = Mathf.Infinity;

    public CoroutineAnimation movementAnimation;

    [Header("Zoom")]
    public bool zoom = false;

    public Vector2 initialZoomScale = Vector2.zero;
    public CoroutineAnimation initialZoomAnimation;

    [Header("Fade")]
    public bool fade = false;
    public float initialAlpha = 0.2f;
    public CoroutineAnimation alphaAnimation;

}
