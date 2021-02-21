using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // Elements needed to create the map
    public GameObject ground;
    public GameObject obstacle;
    public GameObject wall;
    public GameObject startPoint;
    public GameObject checkPoint;
    public GameObject endPoint;
    public int difficulty; // Quantity of block groups
    public float colorPercentage = 30; // Percentage of color blocks
    public float randonessPercentage = 35; // Percentaje of random generated blocks

    private GameManager game;
    private Material[] colors;
    private List<GameObject> nextChunck;
    private GameObject[] prevChunck;
    private Vector3 nextPosition;
    private float chunckDeep;

    private Vector3[][] customBlocks = new Vector3[][] // Prebuild blocks
    {
        new Vector3[] // {-------}
        {
            new Vector3(-6, 1, 0),
            new Vector3(-4, 1, 0),
            new Vector3(-2, 1, 0),
            new Vector3(0, 1, 0),
            new Vector3(2, 1, 0),
            new Vector3(4, 1, 0),
            new Vector3(6, 1, 0)
        },
        /* {  -    }
         * {   -   }
         * {    -  }
         * {     - }
         * {      -}
         */
        new Vector3[] 
        {
            new Vector3(-1.5f, 1, 8),
            new Vector3(0.5f, 1, 6),
            new Vector3(2.5f, 1, 4),
            new Vector3(4.5f, 1, 2),
            new Vector3(6.5f, 1, 0)
        },
        /* {      -}
         * {     - }
         * {    -  }
         * {   -   }
         * {  -    }
         */
        new Vector3[]
        {
            new Vector3(-1.5f, 1, 0),
            new Vector3(0.5f, 1, 2),
            new Vector3(2.5f, 1, 4),
            new Vector3(4.5f, 1, 6),
            new Vector3(6.5f, 1, 8)
        },
        /* {    -  }
         * {   -   }
         * {  -    }
         * { -     }
         * {-      }
         */
        new Vector3[]
        {
            new Vector3(1.5f, 1, 8),
            new Vector3(-0.5f, 1, 6),
            new Vector3(-2.5f, 1, 4),
            new Vector3(-4.5f, 1, 2),
            new Vector3(-6.5f, 1, 0)
        },
        /* {-      }
         * { -     }
         * {  -    }
         * {   -   }
         * {    -  }
         */
        new Vector3[]
        {
            new Vector3(1.5f, 1, 0),
            new Vector3(-0.5f, 1, 2),
            new Vector3(-2.5f, 1, 4),
            new Vector3(-4.5f, 1, 6),
            new Vector3(-6.5f, 1, 8)
        },
        /* { -   - }
         * {       }
         * {  ---  }
         * {       }
         * { -   - }
         */
        new Vector3[]
        {
            new Vector3(-4.5f, 1, 0),
            new Vector3(4.5f, 1, 0),
            new Vector3(-2f, 1, 10),
            new Vector3(0f, 1, 10),
            new Vector3(2f, 1,10),
            new Vector3(-4.5f, 1, 20),
            new Vector3(4.5f, 1, 20)
        },
        new Vector3[] // {- - - -}
        {
            new Vector3(-6.5f, 1, 0),
            new Vector3(-2.5f, 1, 0),
            new Vector3(2.5f, 1, 0),
            new Vector3(6.5f, 1, 0)
        },
        new Vector3[] // { - - - }
        {
            new Vector3(-4, 1, 0),
            new Vector3(0, 1, 0),
            new Vector3(4, 1, 0)
        },
        new Vector3[] // {--   --}
        {
            new Vector3(-6.5f, 1, 0),
            new Vector3(-4.5f, 1, 0),
            new Vector3(4.5f, 1, 0),
            new Vector3(6.5f, 1, 0)
        },
        new Vector3[] // {  ---  }
        {
            new Vector3(-2, 1, 0),
            new Vector3(0, 1, 0),
            new Vector3(2, 1, 0)
        },
        new Vector3[] // {  - -  }
        {
            new Vector3(-2, 1, 0),
            new Vector3(2, 1, 0)
        },
        new Vector3[] // {---    }
        {
            new Vector3(-6.5f, 1, 0),
            new Vector3(-4.5f, 1, 0),
            new Vector3(-2.5f, 1, 0)
        },
        new Vector3[] // {-----  }
        {
            new Vector3(-6.5f, 1, 0),
            new Vector3(-4.5f, 1, 0),
            new Vector3(-2.5f, 1, 0),
            new Vector3(-0.5f, 1, 0),
            new Vector3(1.5f, 1, 0)
        },
        new Vector3[] // {    ---}
        {
            new Vector3(6.5f, 1, 0),
            new Vector3(4.5f, 1, 0),
            new Vector3(2.5f, 1, 0)
        },
        new Vector3[] // {  -----}
        {
            new Vector3(6.5f, 1, 0),
            new Vector3(4.5f, 1, 0),
            new Vector3(2.5f, 1, 0),
            new Vector3(0.5f, 1, 0),
            new Vector3(-1.5f, 1, 0)
        },
        new Vector3[] // {----  -}
        {
            new Vector3(-6.5f, 1, 0),
            new Vector3(-4.5f, 1, 0),
            new Vector3(-2.5f, 1, 0),
            new Vector3(-0.5f, 1, 0),
            new Vector3(6.5f, 1, 0)
        },
        new Vector3[] // {-  ----}
        {
            new Vector3(6.5f, 1, 0),
            new Vector3(4.5f, 1, 0),
            new Vector3(2.5f, 1, 0),
            new Vector3(0.5f, 1, 0),
            new Vector3(-6.5f, 1, 0)
        },
    };

    // Start is called before the first frame update
    void Start()
    {        
        this.difficulty = PlayerPrefs.GetInt("difficulty", 12); // Load Settings
        this.game = FindObjectOfType<GameManager>();
        if (game.colorMode)
        {
            // Load Materials for colors.
            this.colors = Resources.LoadAll<Material>("PlayerMaterials");
        }
        // Activate MeshCollider when endless mode is activate.
        this.ground.GetComponent<MeshCollider>().enabled = game.endlessMode;
        this.ground.GetComponent<BoxCollider>().enabled = !game.endlessMode;
        this.nextChunck = new List<GameObject>();
        this.nextPosition.z = 0; // init the position.
        this.chunckDeep = ground.GetComponent<Renderer>().bounds.size.z;
        NewChunck();
    }

    // Genberate a new Chunk
    public void NewChunck()
    {
        this.prevChunck = new GameObject[nextChunck.Count];
        nextChunck.CopyTo(prevChunck); // Pass the currentChunck references to prevChunk Array
        this.nextChunck.Clear(); // Clean the next chunck item list
        GenerateChunk();
    }  

    // Generate one chuck of the map.
    private void GenerateChunk() 
    {
        nextChunck.Add(Instantiate(ground, ground.transform.position + nextPosition, ground.transform.rotation)); 
        if ((game.walls) || (game.endlessMode)) // Create the walls if endless mode is activated.
        {
            nextChunck.Add(Instantiate(wall, wall.transform.position + nextPosition, wall.transform.rotation)); // Generate invisible left wall
            nextChunck.Add(Instantiate(wall, wall.transform.position + nextPosition + new Vector3(-15, 0, 0), Quaternion.Euler(0, 0, -90))); // Generate invisible right wall
        }
        nextChunck.Add(Instantiate(startPoint, startPoint.transform.position + nextPosition, startPoint.transform.rotation)); // Generate start point trigger
        nextChunck.Add(Instantiate(checkPoint, checkPoint.transform.position + nextPosition, checkPoint.transform.rotation)); // Generate check point trigger
        if (!game.endlessMode) {
            Instantiate(endPoint); // Generate end point trigger;
        }
        float step = chunckDeep / difficulty; // Determine the distance between  block groups
        for(float i = ((nextPosition.z == 0) ? (step * 2) : nextPosition.z); i < nextPosition.z + chunckDeep; i += step) // Create each block group
        {
            Vector3[] BlockGroup = customBlocks[Random.Range(0, customBlocks.Length)]; // Set random prebuilded block
            if (Random.value <= (randonessPercentage / 100)) // Randomly build a Random block
            {
                // https://stackoverflow.com/questions/19191058/fastest-way-to-generate-a-random-boolean
                BlockGroup = RandomBlockGroup(new System.Random().Next() > (System.Int32.MaxValue / 2));
            }
            GenerateBlockGroup(new Vector3(0, 0, i), BlockGroup); // Instantiate the block group            
        }
        this.nextPosition.z += chunckDeep;
    }

    // Instantiate in the scene all blocks in the selected positions
        private void GenerateBlockGroup(Vector3 offset, Vector3[] obstacleGroup)
        {
            foreach (Vector3 position in obstacleGroup) // for each position instantiate a clock
            {
                GameObject current = Instantiate(obstacle, position + offset, obstacle.transform.rotation);
                nextChunck.Add(current); // Store  the object in a list;
                if (game.colorMode) // If game color mode, randomly change materias of blocks
                {
                    if (Random.value <= (colorPercentage / 100))
                    {
                        current.GetComponent<Renderer>().sharedMaterial = colors[Random.Range(0, colors.Length)];
                    }
                }
            }
        }

    // Create random positions of blocks
    private Vector3[] RandomBlockGroup(bool multiline)
    {
        List<Vector3> blocks = new List<Vector3>();
        float[] positions = new float[] { -6, -4, -2, 0, 2, 4, 6 }; // Allowed positions of each line
        int lines = 1;
        if (multiline) { lines = 2; } // if multiline, the group will be two lines
        for(int i = 0; i < lines; i++)
        {
            foreach (float x in positions) // Create each line positions
            {
                if (Random.value <= 0.45f) // 45% of change to spawn a block
                {
                    blocks.Add(new Vector3(x, 1, i * 15)); // distance between lines is 15
                }
            }
        }
        return blocks.ToArray();
    }

    // Remove las lucnk and all of its objects
    public void DestroyPreviusChunck() 
    {
        foreach(GameObject item in prevChunck) {
            GameObject.Destroy(item, 2);
        }
    }
}