using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy enemyPrefab; // ������ �� ������
    public Transform[] spawnPoints; // ���� ��ȯ�� ��ġ

    private List<Enemy> enemies = new List<Enemy>(); // ������ ���� ��� ����Ʈ

    private int wave =1;

    void Start()
    {
        
    }

    void Update()
    {
        
        if(enemies.Count <=0)
        {
            SpawnWave();
            SpawnWave();
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
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            enemies.Add(enemy);
        }
    }

}
