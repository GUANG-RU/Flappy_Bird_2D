using UnityEngine;
using UnityEngine.Audio;

public class Player : MonoBehaviour
{
    // 角色的動畫精靈陣列
    public Sprite[] sprites;

    // 角色的物理屬性
    public float strength = 5f; // 跳躍的力量
    public float gravity = -9.81f; // 重力影響
    public float tilt = 5f; // 角色旋轉角度的影響量

    // 音效
    public AudioClip FlyAudioClip, DeathAudioClip, ScoredAudioClip;

    // 私有變數
    private SpriteRenderer spriteRenderer; // 負責顯示精靈圖片
    private Vector3 direction; // 控制角色移動方向
    private int spriteIndex; // 控制動畫精靈索引

    private void Awake()
    {
        // 取得 SpriteRenderer，負責顯示角色圖片
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // 啟動角色飛行動畫，每 0.15 秒更換一次精靈
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    private void OnEnable()
    {
        // 當角色啟用時，重設位置到初始高度
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;

        // 重設方向，確保角色不受上一局影響
        direction = Vector3.zero;
    }

    private void Update()
    {
        // 檢測玩家是否按下空白鍵或滑鼠左鍵
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            direction = Vector3.up * strength; // 讓角色向上移動
            GetComponent<AudioSource>().PlayOneShot(FlyAudioClip); // 播放飛行音效
        }

        // 套用重力並更新角色位置
        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;

        // 根據角色方向調整旋轉角度，使其有自然的飛翔傾斜
        Vector3 rotation = transform.eulerAngles;
        rotation.z = direction.y * tilt;
        transform.eulerAngles = rotation;
    }

    private void AnimateSprite()
    {
        // 更換精靈圖片，模擬飛翔動畫
        spriteIndex++;

        // 防止超過索引範圍
        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = 0;
        }

        // 更新角色的 Sprite
        if (spriteIndex < sprites.Length && spriteIndex >= 0)
        {
            spriteRenderer.sprite = sprites[spriteIndex];
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 如果碰到障礙物，則結束遊戲並播放死亡音效
        if (other.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver();
            GetComponent<AudioSource>().PlayOneShot(DeathAudioClip);
        }
        // 如果碰到計分區，則增加分數並播放得分音效
        else if (other.gameObject.CompareTag("Scoring"))
        {
            GameManager.Instance.IncreaseScore();
            GetComponent<AudioSource>().PlayOneShot(ScoredAudioClip);
        }
    }

}
