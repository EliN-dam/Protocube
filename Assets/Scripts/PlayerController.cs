using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerController : MonoBehaviour
{
    public float forwardForce = 1000f; // Movement speed
    public float jumpForce = 5f; // Jump impulse

    private GameManager game;
    private PlayerControls controls;
    private PauseMenu menuUI;
    private Vector2 move;
    private Vignette effect;
    private Score score;
    private Rigidbody rb;
    private bool Grounded;
    private bool LeftLimit; 
    private bool RightLimit;
    private Material[] colors; // Player materials
    private int colorIndex;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        game = FindObjectOfType<GameManager>();
        score = FindObjectOfType<Score>();
        menuUI = FindObjectOfType<PauseMenu>();
        effect = FindObjectOfType<Vignette>();
        if (game.colorMode) // If the color mode is activated
        {
            this.colors = Resources.LoadAll<Material>("PlayerMaterials"); // Load the materials
            this.colorIndex = Random.Range(0, colors.Length); // Choose one randomly
            this.GetComponent<Renderer>().sharedMaterial = colors[colorIndex]; // Assign material to player
            ChangeAmbientColor(colors[colorIndex].color); // Modified vignette color
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(0, 0, forwardForce * Time.fixedDeltaTime, ForceMode.Force); // Constant forward movement
        float horizontalMove = move.x; // Contains horizontal control values
        if (LeftLimit) // Prevent player continue when reach left wall
        {
            if (horizontalMove < 0) { horizontalMove = 0; }
        }
        else if (RightLimit) // Prevent player continue when reach left wall
        {
            if (horizontalMove > 0) { horizontalMove = 0; }
        }
        rb.AddForce(horizontalMove, 0, 0 * Time.fixedDeltaTime, ForceMode.VelocityChange); // Horizontal player movement
        if (rb.position.y < -2f) // If the player fall
        {
            if (game.noDeathMode) // If no death mode activated
            {
                transform.position = new Vector3(0, 1, transform.position.z); // Position player back to the platform
                score.decreaseMultiplier(); // Decrease the score
            }
            else { game.GameOver(); } // Restart the level
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground")) { Grounded = true; } // determines whether the player is in the ground

        if (collision.collider.CompareTag("Wall")) // Determines whether the player is in contact with the walls
        { 
            if (rb.position.x > 0 )  // Right wall
            { 
                RightLimit = true; 
            }
            else // Left wall
            { 
                LeftLimit = true; 
            }
        }

        if (collision.collider.CompareTag("Obstacle")) // Determines whether the player crash with a block
        {
            // If the materials of the player and the block are the same...
            if (GetComponent<Renderer>().sharedMaterial == collision.gameObject.GetComponent<Renderer>().sharedMaterial)
            {
                score.increaseMultiplier(); // Increase Score
            }
            else
            {
                // Is no death mode is activated decrease the score
                if (game.noDeathMode) { score.decreaseMultiplier(); } 
                else { game.GameOver(); } // Otherwise restart the level
            }
        }

        if (collision.collider.CompareTag("Pieces")) // Determines whether the player is in contact with pieces of the destroyed block
        {
            Rigidbody collisionRb = collision.collider.GetComponent<Rigidbody>();
            collisionRb.AddForce(new Vector3(1, 1, 0) * 0.0000005f, ForceMode.Impulse); // Add impulse to get more spectacular physics
        }
    }

    private void OnCollisionExit(Collision collision) 
    {
        if (collision.collider.CompareTag("Ground")) { Grounded = false; } // Determines where the player is not in the ground.
        if (collision.collider.CompareTag("Wall")) // Determines whether the player is not in contact with the walls
        {
            if (rb.position.x > 0) { RightLimit = false; }
            else { LeftLimit = false; }
        }
    }

    private void Awake()
    {
        // Settings actions from the new Input Sytem
        controls = new PlayerControls();
        controls.Player.Jump.performed += input => Jump();
        controls.Player.Move.performed += input => move = input.ReadValue<Vector2>(); // Get the values of the stick
        controls.Player.Move.canceled += input => move = Vector2.zero;
        controls.Player.NextColor.started += input => NextColor();
        controls.Player.PrevColor.started += input => PrevColor();
        controls.UI.Pause.performed += input => menuUI.SelectOption();
    }

    private void OnEnable() // Enable Action Maps.
    {
        controls.Player.Enable();
        controls.UI.Enable();
    }

    private void OnDisable() // Disable Action Maps.
    {
        controls.Player.Disable();
        controls.UI.Disable();
    }

    private void Jump() // Player Jump action
    {
        if (!PauseMenu.gamePause) // If the game is not paused
        {
            if (Grounded) // If the player is on the ground
            {
                // Add impulse UP!
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    private void NextColor()
    {
        if (game.colorMode) // If color mode is activated
        {
            if (!PauseMenu.gamePause) // And the game is not paused
            {
                // Change the player material to the next one
                if (colorIndex == colors.Length - 1) { this.colorIndex = 0; }
                else { this.colorIndex++; }
                this.GetComponent<Renderer>().sharedMaterial = colors[colorIndex];
                ChangeAmbientColor(colors[colorIndex].color); // Modified vignette color
            }
        }
    }

    private void PrevColor()
    {
        if (game.colorMode) // If color mode is activated
        {
            if (!PauseMenu.gamePause) // And the game is not paused
            {
                // Change the player material to the previus one
                if (colorIndex == 0) { this.colorIndex = colors.Length - 1; }
                else { this.colorIndex--; }
                this.GetComponent<Renderer>().sharedMaterial = colors[colorIndex];
                ChangeAmbientColor(colors[colorIndex].color); // Modified vignette color
            }
        }
    }

    private void ChangeAmbientColor(Color color)
    {
        // Change post procesing vignette color
        CustomColorFix();
        ColorParameter currentColor = new ColorParameter();
        currentColor.value = color;
        effect.color = currentColor;
    }

    private void CustomColorFix()
    {
        // adjust the values of each color
        if (colors[colorIndex].name.Equals("Red"))
        {
            effect.smoothness = new FloatParameter() { value = 0.2f };
            effect.intensity = new FloatParameter() { value = 0.2f };
        } 
        else if (colors[colorIndex].name.Equals("Lime"))
        {
            effect.smoothness = new FloatParameter() { value = 0.5f };
            effect.intensity = new FloatParameter() { value = 0.2f };
        }
    }
}