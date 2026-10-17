using UnityEngine;

public class Pipes : MonoBehaviour
{
    // 定義管道的上下部分
    public Transform top;
    public Transform bottom;
    // 設定管道間距
    public float gap = 3f;

    // 突然移動的幅度，使管道不完全固定
    public float suddenMoveAmount = 0.05f;

    // 當管道移動到螢幕外時，計算刪除的邊界
    private float leftEdge;

    private void Start()
    {
        // 計算螢幕左邊界，管道超過此範圍後會被刪除
        leftEdge = Camera.main.ScreenToWorldPoint(Vector3.zero).x - 1f;

        // 設定管道的間距，使上管道向上移動，並讓下管道向下移動
        top.position += Vector3.up * gap / 2;
        bottom.position += Vector3.down * gap / 2;
    }

    private void Update()
    {
        // 從 GameManager 獲取當前的速度，使管道根據遊戲進度加速
        float speed = GameManager.Instance.pipeSpeed;

        // 使管道向左移動
        transform.position += speed * Time.deltaTime * Vector3.left;

        // 隨機讓管道產生微小變動，使遊戲更具挑戰性
        if (transform.position.x > 1 && Random.value <= 0.3f)
        {
            SuddenMove();
        }

        // 如果管道移動到螢幕左側邊界，則刪除以節省記憶體
        if (transform.position.x < leftEdge)
        {
            Destroy(gameObject);
        }
    }

    // 讓管道進行隨機小幅度移動，增加難度
    private void SuddenMove()
    {
        float randomDirection = Random.Range(0, 2) == 0 ? -1f : 1f; // 隨機選擇方向
        transform.position += Vector3.up * randomDirection * suddenMoveAmount;
    }

}
