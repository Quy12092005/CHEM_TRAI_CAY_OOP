using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Objects")]
    [SerializeField] private Blade blade;
    [SerializeField] private Spawner spawner;
    [SerializeField] private DifficultyManager difficultyManager;

    [Header("Main UI")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    [SerializeField] private Image fadeImage;

    [Header("Panels")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Game Over UI")]
    [SerializeField] private Text finalScoreText;
    [SerializeField] private Text bestScoreText;

    [Header("Game Settings")]
    [SerializeField] private int startingLives = 3;
    [SerializeField] private int bigBombPenalty = 15;

    public int score { get; private set; }
    public int lives { get; private set; }

    private bool isStartMenu = true;
    private bool isPlaying = false;
    private bool isGameOver = false;

    private Coroutine flashCoroutine;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        ShowStartMenu();
    }

    private void Update()
    {
        // Ở màn hình Start: bấm P hoặc click chuột trái để chơi
        if (isStartMenu)
        {
            if (Input.GetKeyDown(KeyCode.P) || Input.GetMouseButtonDown(0))
            {
                PlayGame();
            }
        }

        // Ở màn hình Game Over: bấm R hoặc click chuột trái để chơi lại
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(0))
            {
                RestartGame();
            }
        }
    }

    private void ShowStartMenu()
    {
        Debug.Log("HIEN START MENU");

        StopAllCoroutines();

        flashCoroutine = null;

        isStartMenu = true;
        isPlaying = false;
        isGameOver = false;

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        ClearScene();

        score = 0;
        lives = startingLives;

        if (blade != null)
        {
            blade.enabled = false;
        }

        if (spawner != null)
        {
            spawner.enabled = false;
        }

        if (startPanel != null)
        {
            startPanel.SetActive(true);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (fadeImage != null)
        {
            fadeImage.color = Color.clear;
            fadeImage.raycastTarget = false;
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMenuMusic();
        }
        if (difficultyManager != null)
        {
            difficultyManager.ResetDifficulty();
        }
        UpdateScoreText();
        UpdateLivesUI();
    }

    private void NewGame()
    {
        Debug.Log("BAT DAU GAME MOI");

        StopAllCoroutines();

        flashCoroutine = null;

        isStartMenu = false;
        isPlaying = true;
        isGameOver = false;

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        ClearScene();

        score = 0;
        lives = startingLives;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopMusic();
        }

        if (startPanel != null)
        {
            startPanel.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (fadeImage != null)
        {
            fadeImage.color = Color.clear;
            fadeImage.raycastTarget = false;
        }

        if (blade != null)
        {
            blade.enabled = true;
        }

        if (spawner != null)
        {
            spawner.enabled = false;
            spawner.enabled = true;
        }
        if (difficultyManager != null)
        {
            difficultyManager.ResetDifficulty();
        }
        UpdateScoreText();
        UpdateLivesUI();
    }

    private void ClearScene()
    {
        Fruit[] fruits = FindObjectsOfType<Fruit>();

        foreach (Fruit fruit in fruits)
        {
            Destroy(fruit.gameObject);
        }

        Bomb[] bombs = FindObjectsOfType<Bomb>();

        foreach (Bomb bomb in bombs)
        {
            Destroy(bomb.gameObject);
        }
    }

    public void IncreaseScore(int points)
    {
        if (!isPlaying || isGameOver)
        {
            return;
        }

        score += points;

        if (score < 0)
        {
            score = 0;
        }

        UpdateScoreText();
        if (difficultyManager != null)
        {
            difficultyManager.UpdateDifficultyByScore(score);
        }
        int hiscore = PlayerPrefs.GetInt("hiscore", 0);

        if (score > hiscore)
        {
            PlayerPrefs.SetInt("hiscore", score);
        }
    }

    public void HitBigBomb()
    {
        if (!isPlaying || isGameOver)
        {
            return;
        }

        // Mạng cuối cùng: Game Over
        if (lives <= 1)
        {
            lives = 0;
            UpdateLivesUI();

            Debug.Log("CHEM BOM TO O MANG CUOI -> GAME OVER");

            Explode();
            return;
        }

        // 2 mạng đầu: mất 1 tim + trừ 15 điểm + lóe màn hình lâu hơn
        lives--;

        IncreaseScore(-bigBombPenalty);
        UpdateLivesUI();

        Debug.Log("CHEM BOM TO -> MAT 1 MANG, TRU " + bigBombPenalty + " DIEM. CON LAI: " + lives);

        StartFlash(0.45f);
    }

    public void HitSmallBomb(int minusPoints)
    {
        if (!isPlaying || isGameOver)
        {
            return;
        }

        IncreaseScore(-minusPoints);

        Debug.Log("CHEM SMALL BOMB -> TRU " + minusPoints + " DIEM");

        StartFlash(0.18f);
    }
    public void AddLife(int amount)
    {
        if (!isPlaying || isGameOver)
        {
            return;
        }

        lives += amount;

        if (lives > startingLives)
        {
            lives = startingLives;
        }

        UpdateLivesUI();

        Debug.Log("Hoi mang! So mang hien tai: " + lives);
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    private void UpdateLivesUI()
    {
        if (livesText == null)
        {
            return;
        }

        string hearts = "";

        for (int i = 0; i < lives; i++)
        {
            hearts += "❤ ";
        }

        livesText.text = "Lives: " + hearts;
    }

    private void UpdateGameOverUI()
    {
        int bestScore = PlayerPrefs.GetInt("hiscore", 0);

        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("hiscore", bestScore);
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = "Score: " + score.ToString();
        }

        if (bestScoreText != null)
        {
            bestScoreText.text = "Best Score: " + bestScore.ToString();
        }
    }

    private void StartFlash(float duration)
    {
        if (fadeImage == null)
        {
            return;
        }

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashSequence(duration));
    }

    private IEnumerator FlashSequence(float duration)
    {
        if (fadeImage == null)
        {
            yield break;
        }

        float halfDuration = duration / 2f;
        float elapsed = 0f;

        while (elapsed < halfDuration)
        {
            float t = Mathf.Clamp01(elapsed / halfDuration);
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        elapsed = 0f;

        while (elapsed < halfDuration)
        {
            float t = Mathf.Clamp01(elapsed / halfDuration);
            fadeImage.color = Color.Lerp(Color.white, Color.clear, t);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        fadeImage.color = Color.clear;
        flashCoroutine = null;
    }

    public void Explode()
    {
        if (isGameOver)
        {
            return;
        }

        Debug.Log("GOI HAM EXPLODE -> GAME OVER");

        isStartMenu = false;
        isPlaying = false;
        isGameOver = true;

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }

        if (blade != null)
        {
            blade.enabled = false;
        }

        if (spawner != null)
        {
            spawner.enabled = false;
        }

        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        if (fadeImage != null)
        {
            fadeImage.raycastTarget = false;

            float elapsed = 0f;
            float duration = 0.5f;

            while (elapsed < duration)
            {
                float t = Mathf.Clamp01(elapsed / duration);

                fadeImage.color = Color.Lerp(Color.clear, Color.white, t);
                Time.timeScale = 1f - t;

                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            fadeImage.color = Color.clear;
        }

        Time.timeScale = 0f;

        UpdateGameOverUI();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameOverMusic();
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Debug.Log("DA HIEN GAME OVER");
    }

    public void PlayGame()
    {
        // Tránh gọi Play nhiều lần cùng lúc
        if (!isStartMenu && isPlaying)
        {
            return;
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        Debug.Log("DA BAM PLAY / BAT DAU CHOI");

        NewGame();
    }

    public void RestartGame()
    {
        // Chỉ restart khi đang Game Over
        if (!isGameOver)
        {
            return;
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        Debug.Log("DA BAM RESTART");

        NewGame();
    }
}