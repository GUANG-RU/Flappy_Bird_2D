using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public Pipes prefab;
    public float initialSpawnRate = 1f; // 初始生成頻率（秒）
    //public float minSpawnRate = 0.5f;   // 最低生成頻率，確保不會太快
    public float spawnDecreaseRate = 0.05f; // 每 10 秒降低的生成間隔
    public float minHeight = -1f; // 管道生成的最低高度
    public float maxHeight = 2f;  // 管道生成的最高高度
    public float verticalGap = 3f; // 管道上下之間的間距

    private float currentSpawnRate; // 當前的管道生成間隔時間

    private void Start()
    {
        currentSpawnRate = initialSpawnRate; // 初始化生成間隔
        StartCoroutine(SpawnPipes()); // 啟動生成管道的協同程序
        StartCoroutine(AdjustSpawnRate()); // 啟動每 10 秒調整頻率的協同程序
    }

    // 協同程序：負責根據 spawnRate 間隔生成管道
    private IEnumerator SpawnPipes()
    {
        while (true)
        {
            Spawn(); // 生成管道
            yield return new WaitForSeconds(currentSpawnRate); // 等待當前的生成間隔後再執行下一次
        }
    }

    // 協同程序：每 10 秒降低生成頻率，使遊戲變得更具挑戰性
    private IEnumerator AdjustSpawnRate()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f); // 等待 10 秒
            currentSpawnRate -= spawnDecreaseRate; // 降低生成間隔
        }
    }

    // 生成管道的方法
    private void Spawn()
    {
        Pipes pipes = Instantiate(prefab, transform.position, Quaternion.identity); // 建立新的管道
        pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight); // 隨機調整管道高度
        pipes.gap = verticalGap; // 設定管道間距
    }

    // 重置生成頻率（在遊戲重新開始時調用）
    public void ResetSpawner()
    {
        currentSpawnRate = initialSpawnRate; 
    }
}
