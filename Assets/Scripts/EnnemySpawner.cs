using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class EnnemySpawner : MonoBehaviour
{
    public GameObject ennemyPrefab;
    
    private List<Vector3> spawns;
    private float waveTime = 30f;
    private float nextSpawn;
    private float waveNumber = 1f;
    private float waveMax = 10f;
    public bool hasWon = false;
    private InterfaceHandle inter;
    public int ennemiesAlive = 0;

    void Start()
    {
        inter = GameObject.Find("Interface").GetComponent<InterfaceHandle>();
        inter.SetMaxWave((int)waveMax);
        spawns = new List<Vector3>();
        foreach (Transform spawn in transform.Find("Spawns"))
        {
            if(spawn.name.Contains("spawn"))
            {
                spawns.Add(spawn.position);
            }
        }

        nextSpawn = Time.time + waveTime/2;
    }

    void Update()
    {

        ennemiesAlive = EnnemiesAlive();

        if(ennemiesAlive <= 0)
        {
            if (Time.time > nextSpawn && waveNumber < waveMax)
            {
                SpawnEnnemies((int)waveNumber);

                inter.IncrementWave();
                waveNumber++;
            }

            else if(waveNumber > waveMax)
            {
                hasWon = true;
                SceneManager.LoadScene("victoryScene");
            }
        } else {
            nextSpawn = Time.time + waveTime;
        }
    }

    private int EnnemiesAlive()
    {
        int number = 0;

        foreach (Transform child in transform)
        {
            if(child.name.Contains("Ennemy"))
            {
                number++;
            }
        }

        return number;
    }

    private List<int> GetRandomSpawns(int quantity)
    {
        List<int> indices = new List<int>();

        if(spawns.Count <= 0)
        {
            Debug.Log("No ennemy spawns found");
            return null;
        }

        for (int i = 0; i < quantity; i++)
        {
            indices.Add(Random.Range(0, spawns.Count-1));
        }

        return indices;
    }

    private void SpawnEnnemies(int number)
    {
        foreach (int index in GetRandomSpawns(number))
        {
            float posx = Random.Range(-30, 30);
            float posz = Random.Range(-30, 30);
            Vector3 spawnPos = spawns.ElementAt(index) + new Vector3(posx, 2f, posz);
            GameObject newEnnemy = Instantiate(ennemyPrefab, spawnPos, Quaternion.identity);

            newEnnemy.transform.SetParent(transform);
        }
    }
}
