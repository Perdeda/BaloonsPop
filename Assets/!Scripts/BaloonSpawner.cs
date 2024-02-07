using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BaloonSpawner : MonoBehaviour
{
    [SerializeField]
    Baloon baloonPref;
    [SerializeField]
    Transform spawnPointTransform;
    [SerializeField]
    float offset = 10f;
    [SerializeField]
    int maxBaloonHP = 4;
    [SerializeField]
    float lowestGravityScale = -.4f;
    [SerializeField]
    bool handleOffset = true;

    public ObjectPool<Baloon> baloons;
    Vector3 spawnPoint;
    bool spawning = false;
    float spawnRate = 0.4f;

    void Start()
    {
        spawnPoint = spawnPointTransform.position;
        Baloon.onBaloonPop += OnBaloonDeath;
        baloons = new ObjectPool<Baloon>(SpawnBaloon, OnTakeFromPool, OnReturnToPool, OnDestroyFromPool, true, 32, 64);

        if (handleOffset)
        {
            float height = Camera.main.orthographicSize * 2;
            float width = height * Camera.main.aspect;
            offset = width/2;
        }
        //StartSpawning();
    }
    public void StartSpawning()
    {
        spawning = true;
        StartCoroutine(StartSpawningCor());
    }
    public void StopSpawning()
    {
        spawning = false;
    }
    public int GetActiveAmount()
    {
        return baloons.CountActive;
    }
    IEnumerator StartSpawningCor()
    {
        while (spawning)
        {
            yield return new WaitForSeconds(spawnRate);
            if(spawning)
                baloons.Get();
            yield return null;
        }
    }
    void OnBaloonDeath(int reward,Baloon baloon,bool byPlayer)
    {
        baloons.Release(baloon);
    }
    Baloon SpawnBaloon()
    {
        Baloon baloon = Instantiate(baloonPref, GetRandomSpawnPoint(), Quaternion.identity);
        baloon.Init(GetRandomHP(),GetRandomGravityScale());
        return baloon;
    }
    void OnTakeFromPool(Baloon baloon)
    {
        baloon.transform.position = GetRandomSpawnPoint();
        baloon.transform.rotation = Quaternion.identity;
        baloon.transform.gameObject.SetActive(true);
        baloon.Init(GetRandomHP(), GetRandomGravityScale());
    }
    void OnReturnToPool(Baloon baloon)
    {
        baloon.gameObject.SetActive(false);
    }
    void OnDestroyFromPool(Baloon baloon)
    {
        Destroy(baloon.gameObject);
    }
    #region Getting random numbers
    Vector3 GetRandomSpawnPoint()
    {
        return new Vector3(spawnPoint.x + Random.Range(-offset, offset), spawnPoint.y, spawnPoint.z);
    }
    private void Update()
    {
        spawnPoint = spawnPointTransform.position;
    }
    int GetRandomHP(bool difficultyBased = true)
    {
        if (difficultyBased)
        {
            if(GameManager.currDifficulty == Difficulty.Medium)
                return Random.Range(1, maxBaloonHP);
            
            int firstParam = Random.Range(1, maxBaloonHP);
            int secondParam = Random.Range(1, maxBaloonHP);

            if (GameManager.currDifficulty == Difficulty.Easy)
                return Mathf.Min(firstParam, secondParam);
            else
                return Mathf.Max(firstParam, secondParam);

        }
        else
            return Random.Range(1, maxBaloonHP);
    }
    float GetRandomGravityScale(bool difficultyBased = true)
    {
        if (difficultyBased)
        {
            if (GameManager.currDifficulty == Difficulty.Medium)
                return Random.Range(-.1f, lowestGravityScale);

            float firstParam = Random.Range(-.1f, lowestGravityScale);
            float secondParam = Random.Range(-.1f, lowestGravityScale);

            if (GameManager.currDifficulty == Difficulty.Easy)
                return Mathf.Max(firstParam, secondParam);
            else
                return Mathf.Min(firstParam, secondParam);

        }
        else
            return Random.Range(-.1f, lowestGravityScale);
    }
    #endregion
    private void OnDestroy()
    {
        Baloon.onBaloonPop -= OnBaloonDeath;
    }
}
