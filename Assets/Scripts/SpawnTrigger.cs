using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{

    [SerializeField] private GameObject worldSectionPrefab;
    [SerializeField] private GameObject currentTile;
    
    private string tileFolderPath = "Tiles";
    private List<GameObject> tilePrefabs;
    
    
    void Awake()
    {
        LoadTilePrefabs();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    void LoadTilePrefabs()
    {
        tilePrefabs = new List<GameObject>();
        foreach (var tile in Resources.LoadAll<GameObject>(tileFolderPath))
        {
            tilePrefabs.Add(tile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //TODO: STILL A GAP SOMETIMES WTF HOW TO FIX
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("SpawnTrigger"))
        {
            if (tilePrefabs.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, tilePrefabs.Count);
                var spawnPosition = currentTile.transform.position + new Vector3(0, 0, 30);
                currentTile = Instantiate(tilePrefabs[randomIndex], spawnPosition, Quaternion.identity);
            }

        }
    }
}

