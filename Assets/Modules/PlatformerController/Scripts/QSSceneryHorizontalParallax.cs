using UnityEngine;

public class QSSceneryParallax : MonoBehaviour
{
    public float parallaxIndex = 1;

    private Vector2 initialOffset;
    private void Start()
    {
        initialOffset = transform.position - Camera.main.transform.position * parallaxIndex;
    }


    private void LateUpdate()
    {
        //now = beginning + (Camera now - camera beginning) * parallax
        //now = beginning + Camera now * parallax - camera beginning * parallax
        //now = beginning - (camera beginning * parallax) + camera now * parallax


        transform.position = initialOffset + (Vector2)Camera.main.transform.position * parallaxIndex;
    }
}
