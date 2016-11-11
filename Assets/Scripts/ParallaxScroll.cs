using UnityEngine;


public class ParallaxScroll : MonoBehaviour
{

    private Vector2 backgroundOffset;
    private Vector2 foregroundOffset;

    public Renderer background;
    public Renderer foreground;
    public float backgroundSpeed = 0.02f;
    public float foregroundSpeed = 0.06f;

    public float Offset { get; set; }


    void Start()
    {
        backgroundOffset = new Vector2(0, 0);
        foregroundOffset = new Vector2(0, 0);
    }


    void Update()
    {
        backgroundOffset.x = Offset * backgroundSpeed;
        foregroundOffset.x = Offset * foregroundSpeed;

        background.material.mainTextureOffset = backgroundOffset;
        foreground.material.mainTextureOffset = foregroundOffset;
    }

}
