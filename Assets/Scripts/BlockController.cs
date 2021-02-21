using UnityEngine;

public class BlockController : MonoBehaviour
{
    public GameObject destroyedObstacle;

    private GameManager game;
    private GameObject player;
    private Rigidbody rb;
    private bool sameMaterial;

    // Start is called before the first frame update
    void Start()
    {
        this.rb = this.GetComponent<Rigidbody>();
        this.player = GameObject.FindWithTag("Player");
        this.game = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Change the block mass if the material of the block is the same of the player has selected or the god mode is activated.
        if ((game.destroyMode) || (player.GetComponent<Renderer>().sharedMaterial == this.GetComponent<Renderer>().sharedMaterial))
        {
            rb.mass = 0f;
            sameMaterial = true;
        }
        else 
        {
            rb.mass = 30f;
            sameMaterial = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if (collision.collider.CompareTag("Player")) 
        {
            Debug.Log("Player material = " + player.GetComponent<Renderer>().sharedMaterial.name, player.gameObject);
            Debug.Log("Obtacle material = " + this.GetComponent<Renderer>().sharedMaterial.name);
        }*/
        
        if ((sameMaterial) && (collision.collider.CompareTag("Player"))) 
        {
            // Switching the block for a destructible one
            GameObject destroyed = Instantiate(destroyedObstacle, transform.position, transform.rotation);
            Destroy(destroyed, 2); // Remove pieces after two seconds.
            // Change all pieces material to the same of the original block
            Renderer[] pieces = destroyed.GetComponentsInChildren<Renderer>();
            foreach(Renderer piece in pieces)
            {
                piece.sharedMaterial = this.GetComponent<Renderer>().sharedMaterial;
            }
            gameObject.SetActive(false); // Remove the original block.
        }
    }
}
