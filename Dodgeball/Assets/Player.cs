using System.Collections;
using UnityEngine;

/// <summary>
/// Control the player on screen
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    /// <summary>
    /// Prefab for the orbs we will shoot
    /// </summary>
    public GameObject OrbPrefab;

    /// <summary>
    /// How fast our engines can accelerate us
    /// </summary>
    public float EnginePower = 1;
    
    /// <summary>
    /// How fast we turn in place
    /// </summary>
    public float RotateSpeed = 1;

    /// <summary>
    /// How fast we should shoot our orbs
    /// </summary>
    public float OrbVelocity = 10;


    private Rigidbody2D rb;

    public SpriteRenderer spriteRenderer;
    public float colorSpeed = 0.1f;

    private Color[] rainbowColors = {
        Color.red,
        new Color(1, 0.5f, 0), // Orange
        Color.yellow,
        Color.green,
        Color.cyan,
        Color.blue,
        new Color(0.5f, 0, 1) // Purple
    };

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(LoopThroughColors());
    }

    /// <summary>
    /// Handle moving and firing.
    /// Called by Uniity every 1/50th of a second, regardless of the graphics card's frame rate
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void FixedUpdate()
    {
        Manoeuvre();
        MaybeFire();
    }

    /// <summary>
    /// Fire if the player is pushing the button for the Fire axis
    /// Unlike the Enemies, the player has no cooldown, so they shoot a whole blob of orbs
    /// </summary>
    void MaybeFire()
    {
        if (Input.GetButton("Fire"))
        {

            for (int i = 0; i < 10; i++)
            {
                FireOrb();
            }
        }

    }

    /// <summary>
    /// Fire one orb.  The orb should be placed one unit "in front" of the player.
    /// transform.right will give us a vector in the direction the player is facing.
    /// It should move in the same direction (transform.right), but at speed OrbVelocity.
    /// </summary>
    private void FireOrb()
    {
        GameObject newOrb = Instantiate(OrbPrefab, transform.position + (transform.right * 1.5f), Quaternion.identity);

        Rigidbody2D orbBody = newOrb.GetComponent<Rigidbody2D>();

        orbBody.velocity = transform.right * OrbVelocity;
    }

    /// <summary>
    /// Accelerate and rotate as directed by the player
    /// Apply a force in the direction (Horizontal, Vertical) with magnitude EnginePower
    /// Note that this is in *world* coordinates, so the direction of our thrust doesn't change as we rotate
    /// Set our angularVelocity to the Rotate axis time RotateSpeed
    /// </summary>
    void Manoeuvre()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        Vector2 thrust = new Vector2(xInput, yInput) * EnginePower;

        rb.AddForce(thrust);

        rb.angularVelocity = Input.GetAxis("Rotate") * RotateSpeed;
    }

    /// <summary>
    /// If this is called, we got knocked off screen.  Deduct a point!
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void OnBecameInvisible()
    {
        ScoreKeeper.ScorePoints(-1);
    }

    IEnumerator LoopThroughColors()
    {
        int currentColorIndex = 0;

        while (true)
        {
            Color currentColor = rainbowColors[currentColorIndex];
            Color nextColor = rainbowColors[(currentColorIndex + 1) % rainbowColors.Length]; // Loop back to the start when reaching the end

            float elapsedTime = 0f;

            while (elapsedTime < colorSpeed)
            {
                spriteRenderer.color = Color.Lerp(currentColor, nextColor, elapsedTime / colorSpeed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            currentColorIndex = (currentColorIndex + 1) % rainbowColors.Length; // Move to next color
        }
    }
}
