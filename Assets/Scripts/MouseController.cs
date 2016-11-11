using UnityEngine;
using UnityEngine.SceneManagement;


public class MouseController : MonoBehaviour
{

    private Rigidbody2D rb2D;
    private Vector2 upwardForce;
    private bool dead;
    private bool grounded;
    private bool groundedStateToggled;
    private Animator animator;
    private AudioManager audioManager;
    private ParticleSystem.EmissionModule emission;
    private ParticleSystem.MinMaxCurve emissionRate;
    private uint coins;
    
    public float jetpackForce = 30.0f;
    public float forwardSpeed = 8.0f;
    public ParticleSystem jetpack;
    public Transform groundCheckTransform;
    public LayerMask groundCheckLayerMask;
    public Texture2D coinIconTexture;
    public ParallaxScroll parallax;


    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        upwardForce = new Vector2(0, jetpackForce);
        dead = false;
        grounded = true;
        groundedStateToggled = false;
        animator = GetComponent<Animator>();
        audioManager = GetComponent<AudioManager>();
        emission = jetpack.emission;
        emissionRate = emission.rate;
        coins = 0;

        audioManager.StartAudio(AudioManager.AudioType.Background);
    }


    void FixedUpdate()
    {
        bool jetpackActive = Input.GetButton("Fire1");
        jetpackActive = jetpackActive && !dead;

        if (jetpackActive)
        {
            rb2D.AddForce(upwardForce);
        }

        if (!dead)
        {
            Vector2 newVelocity = rb2D.velocity;
            newVelocity.x = forwardSpeed;
            rb2D.velocity = newVelocity;
        }

        UpdateGroundedState();
        AdjustJetpack(jetpackActive);

        parallax.Offset = transform.position.x;
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        string hitObjectTag = collider.gameObject.tag;

        switch (hitObjectTag)
        {
            case "Coin":
                CollectCoin(collider);
                break;
            case "Laser":
                HitByLaser(collider);
                break;
            default:
                Debug.Log("Hit By Unrecognized Object");
                break;
        }
    }


    void OnGUI()
    {
        DisplayCoinsCount();
        DisplayRestartButton();
    }


    void UpdateGroundedState()
    {
        bool newGroundedState = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
        groundedStateToggled = (grounded != newGroundedState) ? true : false;
        grounded = newGroundedState;
        animator.SetBool("Grounded", grounded);
    }


    void AdjustJetpack(bool jetpackActive)
    {
        if (groundedStateToggled && !grounded)
        {
            jetpack.Play();
            audioManager.StartAudio(AudioManager.AudioType.Jetpack);
            audioManager.StopAudio(AudioManager.AudioType.Footsteps);
        }
        else if (groundedStateToggled && grounded)
        {
            jetpack.Stop();
            audioManager.StopAudio(AudioManager.AudioType.Jetpack);
            audioManager.StartAudio(AudioManager.AudioType.Footsteps);
        }

        if (dead)
        {
            jetpack.Stop();
            audioManager.StopAudio(AudioManager.AudioType.Jetpack);
            audioManager.StopAudio(AudioManager.AudioType.Footsteps);
        }

        emissionRate.constantMin = emissionRate.constantMax = jetpackActive ? 300 : 75;
        emission.rate = emissionRate;
    }


    void CollectCoin(Collider2D coinCollider)
    {
        coins++;
        Destroy(coinCollider.gameObject);
        audioManager.StartAudio(AudioManager.AudioType.CoinCollect);
    }


    void HitByLaser(Collider2D laserCollider)
    {
        dead = true;
        animator.SetBool("Dead", true);
        audioManager.StartAudio(AudioManager.AudioType.LaserZap);
    }


    void DisplayCoinsCount()
    {
        Rect coinIconRect = new Rect(10, 10, 32, 32);
        GUI.DrawTexture(coinIconRect, coinIconTexture);

        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.yellow;

        Rect labelRect = new Rect(coinIconRect.xMax, coinIconRect.y, 60, 32);
        GUI.Label(labelRect, coins.ToString(), style);
    }


    void DisplayRestartButton()
    {
        if (dead && grounded)
        {
            Rect buttonRect = new Rect(Screen.width * 0.35f, Screen.height * 0.45f, Screen.width * 0.30f, Screen.height * 0.1f);
            
            if (GUI.Button(buttonRect, "Tap To Restart"))
            {
                string sceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            };
            
            float buttonCenterX = (buttonRect.xMin + buttonRect.xMax) * 0.5f;
            string distance = ((int)transform.position.x).ToString();

            GUIStyle style = new GUIStyle();
            style.fontSize = 30;
            style.fontStyle = FontStyle.Bold;
            style.normal.textColor = Color.white;

            Rect labelRect = new Rect(buttonCenterX - 112, buttonRect.yMax + 10, 144, 32);
            GUI.Label(labelRect, "Distance", style);
            
            Rect distanceLabelRect = new Rect(labelRect.xMax, labelRect.y, 60, 32);
            GUI.Label(distanceLabelRect, distance, style);
        }
    }

}
