using UnityEngine;


public class Laser : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Collider2D coll2D;
    private bool isLaserOn;
    private float timeUntilNextToggle;
    public AudioSource audioSource;
    
    public Sprite laserOnSprite;
    public Sprite laserOffSprite;
    [Range(-200.0f, 200.0f)]
    public float rotationSpeed = 0.0f;
    public bool toggleMechanism = false;
    [HideInInspector]
    public float toggleInterval = 0.5f;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll2D = GetComponent<Collider2D>();
        isLaserOn = false;
        timeUntilNextToggle = toggleInterval;
    }


    void FixedUpdate()
    {
        timeUntilNextToggle -= Time.fixedDeltaTime;

        if (timeUntilNextToggle <= 0)
        {
            ToggleLaserState();
            timeUntilNextToggle = toggleInterval;
        }

        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.fixedDeltaTime);
    }


    void ToggleLaserState()
    {
        if (toggleMechanism)
        {
            isLaserOn = !isLaserOn;
            coll2D.enabled = isLaserOn;
            spriteRenderer.sprite = isLaserOn ? laserOnSprite : laserOffSprite;
        }
    }

}
