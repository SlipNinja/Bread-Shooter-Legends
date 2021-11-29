using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Growth : MonoBehaviour
{
    
    public GameObject weatPrefab;
    private Vector3 boundingSize;
    private Vector3 boundingCenter;
    private float minGapX, minGapZ;
    private GameObject spawns;
    private Dictionary<GameObject, GameObject> spawnPoints; // <spawnPoint, weat>
    private float spawnRate = 2f;
    private float nextSpawn = 0.0f;

    void Start()
    {
        spawnPoints = new Dictionary<GameObject, GameObject>();
        spawns = new GameObject("Growth Spots");
        spawns.transform.position = Vector3.zero;
        spawns.transform.SetParent(transform);
        
        minGapX = 6f;
        minGapZ = 2f;

        float offset = 3f;
        Vector3 VecOffset = new Vector3(-offset*2, 0, -offset*2);
        boundingSize = GetComponent<Renderer>().bounds.size + VecOffset;

        int rows = (int)Mathf.Floor(boundingSize.x/minGapX);
        int cols = (int)Mathf.Floor(boundingSize.z/minGapZ);

        float offsetX = (boundingSize.x - rows*minGapX)/2;
        float offsetZ = (boundingSize.z - cols*minGapZ)/2;

        for (int row = 0; row <= rows; row++)
        {
            for (int col = 0; col <= cols; col++)
            {
                GameObject spawnPoint = new GameObject("Growth Spot");
                spawnPoint.transform.SetParent(spawns.transform);

                Vector3 newPos = new Vector3(row*minGapX + offsetX, 0, col*minGapZ + offsetZ);
                spawnPoint.transform.position = newPos + transform.position - boundingSize/2;
                spawnPoints.Add(spawnPoint, null);
            }
        }
    }
 
    void Update () {
    
    if (Time.time > nextSpawn ) {
        nextSpawn = Time.time + 1/spawnRate;// Range around spawnrate mayb

        GameObject spawn = GetRandomFreeSpawn();

        if(spawn is null)
        {
            return;
        }

        GameObject newWeat = Instantiate(weatPrefab, spawn.transform.position, Quaternion.identity);
        newWeat.transform.SetParent(transform);

        spawnPoints[spawn] = newWeat;
    }
 }

    private GameObject GetRandomFreeSpawn()
    {
        List<KeyValuePair<GameObject, GameObject>> freeSpawns = spawnPoints.Where(freeSpawn => freeSpawn.Value is null).ToList();
        
        if(freeSpawns.Count() <= 0)
        {
            return null;
        }
        
        int spawnIndex = Random.Range(0, freeSpawns.Count()-1);
        
        return freeSpawns.ElementAt(spawnIndex).Key;
    }

    public bool Gather(GameObject weat, float deleteOffset)
    {
        List<KeyValuePair<GameObject, GameObject>> newFreeSpawns = spawnPoints.Where(spawn => spawn.Value == weat).ToList();

        if(newFreeSpawns.Count <= 0)
        {
            Debug.Log("Already gathered");
            return false;
        }

        Destroy(weat);
        StartCoroutine(CleaningCoroutine(newFreeSpawns.ElementAt(0).Key, deleteOffset));

        return true;
    }

    IEnumerator CleaningCoroutine(GameObject spawn, float deleteOffset)
    {
        yield return new WaitForSeconds(deleteOffset);

        spawnPoints[spawn] = null;
        
    }
}
