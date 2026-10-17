using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Player player;
    [SerializeField] private Spawner spawner;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject getReady;
    [SerializeField] private GameObject scoreBoard;
    [SerializeField] private Text bestScore;
    [SerializeField] private Text nowScore;
    [SerializeField] private GameObject scoreMedal;

    // 遊戲變數
    public float pipeSpeed = 5f; // 初始管道速度
    public Sprite goldMedal, sliverMedal, bronzeMedal, trashMedal; // 獎牌圖片
    private float speedIncreaseTimer = 0f; // 計時器，用來控制速度增加
    private float speedIncreaseInterval = 5f; // 每 5 秒增加一次速度

    public int score { get; private set; } = 0; // 玩家得分
    public int bestscore = 0; // 最高得分紀錄

    private void Awake()
    {
        // 設置單例模式，確保只有一個 GameManager
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        // 清除單例，確保不會有多個 GameManager
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        // 隱藏 UI，確保遊戲開始前畫面整潔
        gameOver.SetActive(false);
        scoreBoard.SetActive(false);
        bestScore.gameObject.SetActive(false);
        nowScore.gameObject.SetActive(false);
        scoreMedal.gameObject.SetActive(false);

        Pause(); // 啟動時暫停遊戲，等待玩家開始
    }

    // 暫停遊戲
    public void Pause()
    {
        Time.timeScale = 0f; // 停止時間運行
        player.enabled = false; // 禁用玩家控制
    }

    // 開始遊戲
    public void Play()
    {
        // 初始化遊戲變數
        score = 0;
        speedIncreaseTimer = 0f;
        pipeSpeed = 5f;
        scoreText.text = score.ToString();
        nowScore.text = score.ToString();

        // 隱藏不必要的 UI
        playButton.SetActive(false);
        gameOver.SetActive(false);
        getReady.SetActive(false);
        scoreBoard.SetActive(false);
        scoreText.gameObject.SetActive(true);
        bestScore.gameObject.SetActive(false);
        nowScore.gameObject.SetActive(false);
        scoreMedal.gameObject.SetActive(false);

        Time.timeScale = 1f; // 恢復時間運行
        player.enabled = true; // 啟用玩家控制

        spawner.ResetSpawner(); // 重新設置管道生成頻率

        // 刪除場上所有管道，確保新的開始
        Pipes[] pipes = FindObjectsByType<Pipes>(FindObjectsSortMode.None);
        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }
    }

    // 遊戲結束
    public void GameOver()
    {
        // 更新最高分
        if (score > bestscore)
        {
            bestScore.text = score.ToString();
            bestscore = score;
        }
        else
        {
            bestScore.text = bestscore.ToString();
        }

        // 根據玩家得分分配獎牌
        if (score >= 20)
        {
            scoreMedal.GetComponent<Image>().sprite = goldMedal; // 設定金牌
        }
        else if (score >= 10)
        {
            scoreMedal.GetComponent<Image>().sprite = sliverMedal; // 設定銀牌
        }
        else if (score >= 5)
        {
            scoreMedal.GetComponent<Image>().sprite = bronzeMedal; // 設定銅牌
        }
        else
        {
            scoreMedal.GetComponent<Image>().sprite = trashMedal; // 設定低分獎牌
        }

        // 顯示遊戲結束 UI
        playButton.SetActive(true);
        gameOver.SetActive(true);
        getReady.SetActive(false);
        scoreMedal.gameObject.SetActive(true);
        scoreBoard.SetActive(true);
        scoreText.gameObject.SetActive(false);
        bestScore.gameObject.SetActive(true);
        nowScore.gameObject.SetActive(true);
        scoreMedal.gameObject.SetActive(true);

        GetComponent<AudioSource>().Play(); // 播放結束音效
        Pause(); // 暫停遊戲
    }

    // 增加玩家得分
    public void IncreaseScore()
    {
        score++; // 增加分數
        scoreText.text = score.ToString();
        nowScore.text = score.ToString();
    }

    private void Update()
    {
        // 計時，控制管道速度增加
        speedIncreaseTimer += Time.deltaTime;
        if (speedIncreaseTimer >= speedIncreaseInterval)
        {
            pipeSpeed += 1f; // 每 5 秒增加管道速度
            speedIncreaseTimer = 0f; // 重置計時器
        }
    }

}
