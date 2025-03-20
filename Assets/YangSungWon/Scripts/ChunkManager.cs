using UnityEngine;
using System.Collections.Generic;

public class ChunkManager : MonoBehaviour
{
    // 난이도별 청크 프리팹 배열
    public GameObject[] stage1Chunks; // 스테이지 1: 기본 난이도 청크
    public GameObject[] stage2Chunks; // 스테이지 2: 중간 난이도 청크
    public GameObject[] stage3Chunks; // 스테이지 3: 높은 난이도 청크

    [SerializeField] private Transform player;
    [SerializeField] private float chunkLength = 10f;
    [SerializeField] private float spawnDistance = 30f;
    [SerializeField] private int chunksPerStage = 10; // 스테이지당 청크 수 (10으로 설정)

    private List<GameObject> activeChunks = new List<GameObject>();
    private float lastChunkEndPosition = 0f;
    private int currentStage = 1; // 현재 스테이지 (1, 2, 3)
    private int chunksSpawnedInStage = 0; // 현재 스테이지에서 스폰된 청크 수
    private bool isGameFinished = false; // 게임 종료 여부

    void Start()
    {
        // 초기 청크 생성 (스테이지 1 시작)
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
        // 처음에 몇 개의 청크를 미리 생성 (예: 5개)
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
            if (isGameFinished) return; // 게임 끝나면 더 이상 생성 안 함
        }

        GameObject chunk;
        GameObject[] currentChunks = GetCurrentStageChunks();
        int randomIndex = Random.Range(0, currentChunks.Length);
        chunk = Instantiate(currentChunks[randomIndex], Vector3.zero, Quaternion.identity);

        chunk.transform.position = new Vector3(0, 0, lastChunkEndPosition);
        lastChunkEndPosition += chunkLength;
        activeChunks.Add(chunk);

        chunksSpawnedInStage++;
        Debug.Log($"스테이지 {currentStage} - 청크 {chunksSpawnedInStage}/{chunksPerStage}: {currentChunks[randomIndex].name}");
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
        chunksSpawnedInStage = 0; // 새 스테이지 시작 시 카운트 리셋

        if (currentStage > 3)
        {
            FinishGame();
        }
        else
        {
            Debug.Log($"스테이지 {currentStage} 시작!");
        }
    }

    GameObject[] GetCurrentStageChunks()
    {
        switch (currentStage)
        {
            case 1: return stage1Chunks;
            case 2: return stage2Chunks;
            case 3: return stage3Chunks;
            default: return stage1Chunks; // 안전장치
        }
    }

    void FinishGame()
    {
        isGameFinished = true;
        Debug.Log("모든 스테이지 클리어! 게임 종료.");
        // 여기서 마무리 로직 추가 가능 (예: 엔딩 화면 표시)
    }
}