using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    private MapGenerator map;

    // Start is called before the first frame update
    void Start()
    {
        map = FindObjectOfType<MapGenerator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Remove previus Chunk and all of its items.
        if (other.CompareTag("Player"))
        {
            map.DestroyPreviusChunck();
            this.enabled = false; // Prevent collision twice.
        }
    }
}
