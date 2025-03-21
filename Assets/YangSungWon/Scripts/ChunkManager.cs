using UnityEngine;
using System.Collections.Generic;

public class ChunkManager : MonoBehaviour
{
    // 난이도별 청크 프리팹 배열
    public GameObject[] stage1Chunks;
    public GameObject[] stage2Chunks;
    public GameObject[] stage3Chunks;

    // 각 스테이지의 마지막 청크
    public GameObject stage1LastChunk;
    public GameObject stage2LastChunk;
    public GameObject stage3LastChunk;

    [SerializeField] private Transform player;
    [SerializeField] private float chunkLength = 50f; // 네가 원하는 50으로 변경
    [SerializeField] private float spawnDistance = 30f;
    [SerializeField] private int chunksPerStage = 10;

    // 스테이지별 높이 설정
    [SerializeField] private float stage1Height = 0f;   // 스테이지 1 시작 높이
    [SerializeField] private float stage2Height = 10f;  // 스테이지 2 시작 높이
    [SerializeField] private float stage3Height = 20f;  // 스테이지 3 시작 높이

    private List<GameObject> activeChunks = new List<GameObject>();
    private float lastChunkEndPosition = 0f;
    private int currentStage = 1;
    private int chunksSpawnedInStage = 0;
    private bool isGameFinished = false;
    private float currentHeight = 0f; // 현재 스테이지의 Y축 높이

    void Start()
    {
        // 초기 높이 설정
        currentHeight = stage1Height;
        player.position = new Vector3(0, currentHeight + 1f, -20f); // 청크 시작점 근처
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

        // Y축 높이를 현재 스테이지 높이에 맞춰 설정
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

        // 스테이지 전환 시 높이 업데이트
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

        // 플레이어 높이 조정
        player.position = new Vector3(player.position.x, currentHeight + 1f, player.position.z);
        Debug.Log($"스테이지 {currentStage} 시작! 높이: {currentHeight}");
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

    void FinishGame()
    {
        isGameFinished = true;
        Debug.Log("모든 스테이지 클리어! 게임 종료.");
    }
}