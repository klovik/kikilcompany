using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public GameObject[] testPrefabs;
    public int chunkOffsetX, chunkOffsetZ;
    public float mapSizeX, mapSizeZ;
    private GameObject player;
    private Transform generatorCursor;
    private GameObject mapParent;

    private void Awake()
    {
        mapParent = GameObject.Find("Map");
        generatorCursor = transform.GetChild(0);
        player = GameObject.Find("Player");
        player.SetActive(false);
    }

    private void Start()
    {
        //StartCoroutine(SlowGenerateChunks(testPrefabs, 1));
        GenerateChunks(testPrefabs);
        OnGenerationEnd();
    }

    private void GenerateChunks(GameObject[] chunkPrefabs)
    {
        int i = 0;
        for (int x = 0; x < mapSizeX * chunkOffsetX; x += chunkOffsetX)
        {
            for (int z = 0; z < mapSizeZ * chunkOffsetZ; z += chunkOffsetZ)
            {
                GameObject randomChunkPrefab = chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];
                generatorCursor.transform.position = new Vector3(x, 0, z);
                GameObject chunk = Instantiate(randomChunkPrefab);
                print($"Instantiated {randomChunkPrefab.name}");
                chunk.name = $"#{i} {randomChunkPrefab.name}";
                chunk.transform.SetParent(mapParent.transform, true);
                chunk.transform.localPosition = generatorCursor.transform.position;
                i++;
            }
        }

        mapParent.transform.position = new Vector3(-(mapSizeX/2)*chunkOffsetX, 0, -(mapSizeZ/2)*chunkOffsetZ);
        print("Generation done!");
    }

    private void OnGenerationEnd()
    {
        player.SetActive(true);
        //generatorCursor.gameObject.SetActive(false);
    }
}
