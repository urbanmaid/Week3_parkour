using UnityEngine;
using System.Collections.Generic;

public class ChunkManager : MonoBehaviour
{
    public GameObject[] stage1Chunks;
    public GameObject[] stage2Chunks;
    public GameObject[] stage3Chunks;
    public GameObject stage1LastChunk;
    public GameObject stage2LastChunk;
    public GameObject stage3LastChunk;

    [SerializeField] private Transform player;
    [SerializeField] private float chunkLength = 50f;
    [SerializeField] private float spawnDistance = 30f;

    // 스테이지별 청크 개수
    [SerializeField] private int stage1ChunkCount = 10;
    [SerializeField] private int stage2ChunkCount = 12;
    [SerializeField] private int stage3ChunkCount = 15;

    [SerializeField] private float stage1Height = 0f;
    [SerializeField] private float stage2Height = 12f;
    [SerializeField] private float stage3Height = 24f;

    private List<GameObject> activeChunks = new List<GameObject>();
    private float lastChunkEndPosition = 0f;
    public int currentStage = 1;
    private int chunksSpawnedInStage = 0;
    private bool isGameFinished = false;
    private float currentHeight = 0f;

    void Start()
    {
        currentHeight = stage1Height;
        player.position = new Vector3(0, currentHeight + 1f, 2f);
        SpawnInitialChunks();
    }

    void Update()
    {
        if (player == null || isGameFinished) return;

        if (player.position.z > lastChunkEndPosition - spawnDistance)
        {
            SpawnChunk();
            RemoveOldChunks();
        }

        if (currentStage > 1 && chunksSpawnedInStage == spawnDistance / chunkLength)
        {
            Debug.Log("왜 호출 안하자?");
            GameManager.instance.SetCurStage(currentStage);
        }
    }

    void SpawnInitialChunks()
    {
        for (int i = 0; i < 5; i++)
        {
            SpawnChunk();
        }
    }

    void SpawnChunk()
    {
        int chunksPerStage = GetChunksPerStage(); // 현재 스테이지의 청크 개수 가져오기
        if (chunksSpawnedInStage >= chunksPerStage)
        {
            MoveToNextStage();
            if (isGameFinished) return;
        }

        GameObject chunk;
        if (chunksSpawnedInStage == chunksPerStage - 1)
        {
            chunk = Instantiate(GetLastChunkForStage(), Vector3.zero, Quaternion.identity);
            Debug.Log($"스테이지 {currentStage} - 마지막 청크 스폰: {chunk.name} at Y={currentHeight}");
        }
        else
        {
            GameObject[] currentChunks = GetCurrentStageChunks();
            int randomIndex = Random.Range(0, currentChunks.Length);
            chunk = Instantiate(currentChunks[randomIndex], Vector3.zero, currentChunks[randomIndex].transform.rotation);
            Debug.Log($"스테이지 {currentStage} - 청크 {chunksSpawnedInStage + 1}/{chunksPerStage}: {currentChunks[randomIndex].name}");
        }

        chunk.transform.position = new Vector3(0, currentHeight, lastChunkEndPosition);
        lastChunkEndPosition += chunkLength;
        activeChunks.Add(chunk);

        chunksSpawnedInStage++;
    }

    void RemoveOldChunks()
    {
        for (int i = activeChunks.Count - 1; i >= 0; i--)
        {
            if (activeChunks[i].transform.position.z < player.position.z - chunkLength * 2)
            {
                GameObject chunkToRemove = activeChunks[i];
                activeChunks.RemoveAt(i);
                Destroy(chunkToRemove);
            }
        }
    }

    void MoveToNextStage()
    {
        currentStage++;
        chunksSpawnedInStage = 0;

        switch (currentStage)
        {
            case 2:
                currentHeight = stage2Height;
                break;
            case 3:
                currentHeight = stage3Height;
                break;
            case 4:
                FinishGame();
                return;
        }

        Debug.Log($"스테이지 {currentStage} 시작! 높이: {currentHeight}, 청크 개수: {GetChunksPerStage()}");
    }

    GameObject[] GetCurrentStageChunks()
    {
        switch (currentStage)
        {
            case 1: return stage1Chunks;
            case 2: return stage2Chunks;
            case 3: return stage3Chunks;
            default: return stage1Chunks;
        }
    }

    GameObject GetLastChunkForStage()
    {
        switch (currentStage)
        {
            case 1: return stage1LastChunk;
            case 2: return stage2LastChunk;
            case 3: return stage3LastChunk;
            default: return stage1LastChunk;
        }
    }

    int GetChunksPerStage()
    {
        switch (currentStage)
        {
            case 1: return stage1ChunkCount;
            case 2: return stage2ChunkCount;
            case 3: return stage3ChunkCount;
            default: return stage1ChunkCount; // 안전장치
        }
    }

    void FinishGame()
    {
        isGameFinished = true;
        Debug.Log("모든 스테이지 클리어! 게임 종료.");
    }

    public float GetStageHeight()
    {
        if (currentStage == 1)
        {
            return stage1Height;
        }
        else if (currentStage == 2)
        {
            return stage2Height;
        }
        else if (currentStage == 3)
        {
            return stage3Height;
        }
        return 0;
    }
}