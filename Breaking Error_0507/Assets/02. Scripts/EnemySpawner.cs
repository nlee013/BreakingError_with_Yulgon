using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy enemyPrefab; // ������ �� ������
    public Transform[] spawnPoints; // ���� ��ȯ�� ��ġ

    private List<Enemy> enemies = new List<Enemy>(); // ������ ���� ��� ����Ʈ

    private int enemyCount = 0;
    private int wave;

    void Start()
    {
        
    }

    void Update()
    {
        if(enemies.Count <= 0)
        {
            SpawnWave();
        }
    }

    void SpawnWave()
    {
        wave++;

        // ���� ���̺� * 2�� �ݿø��� ����ŭ �� ����
        int spawnCount = Mathf.RoundToInt(wave * 2f);

        for (int i = 0; i < spawnCount; i++) // spawnCount��ŭ �� ����
        {
            CreateEnemy();
        }
    }

    void CreateEnemy()
    {

    }
}
