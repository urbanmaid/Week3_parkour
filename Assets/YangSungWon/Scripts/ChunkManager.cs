using UnityEngine;
using System.Collections.Generic;

public class ChunkManager : MonoBehaviour
{
    // ���̵��� ûũ ������ �迭
    public GameObject[] stage1Chunks; // �������� 1: �⺻ ���̵� ûũ
    public GameObject[] stage2Chunks; // �������� 2: �߰� ���̵� ûũ
    public GameObject[] stage3Chunks; // �������� 3: ���� ���̵� ûũ

    [SerializeField] private Transform player;
    [SerializeField] private float chunkLength = 10f;
    [SerializeField] private float spawnDistance = 30f;
    [SerializeField] private int chunksPerStage = 10; // ���������� ûũ �� (10���� ����)

    private List<GameObject> activeChunks = new List<GameObject>();
    private float lastChunkEndPosition = 0f;
    private int currentStage = 1; // ���� �������� (1, 2, 3)
    private int chunksSpawnedInStage = 0; // ���� ������������ ������ ûũ ��
    private bool isGameFinished = false; // ���� ���� ����

    void Start()
    {
        // �ʱ� ûũ ���� (�������� 1 ����)
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
        // ó���� �� ���� ûũ�� �̸� ���� (��: 5��)
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
            if (isGameFinished) return; // ���� ������ �� �̻� ���� �� ��
        }

        GameObject chunk;
        GameObject[] currentChunks = GetCurrentStageChunks();
        int randomIndex = Random.Range(0, currentChunks.Length);
        chunk = Instantiate(currentChunks[randomIndex], Vector3.zero, Quaternion.identity);

        chunk.transform.position = new Vector3(0, 0, lastChunkEndPosition);
        lastChunkEndPosition += chunkLength;
        activeChunks.Add(chunk);

        chunksSpawnedInStage++;
        Debug.Log($"�������� {currentStage} - ûũ {chunksSpawnedInStage}/{chunksPerStage}: {currentChunks[randomIndex].name}");
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
        chunksSpawnedInStage = 0; // �� �������� ���� �� ī��Ʈ ����

        if (currentStage > 3)
        {
            FinishGame();
        }
        else
        {
            Debug.Log($"�������� {currentStage} ����!");
        }
    }

    GameObject[] GetCurrentStageChunks()
    {
        switch (currentStage)
        {
            case 1: return stage1Chunks;
            case 2: return stage2Chunks;
            case 3: return stage3Chunks;
            default: return stage1Chunks; // ������ġ
        }
    }

    void FinishGame()
    {
        isGameFinished = true;
        Debug.Log("��� �������� Ŭ����! ���� ����.");
        // ���⼭ ������ ���� �߰� ���� (��: ���� ȭ�� ǥ��)
    }
}