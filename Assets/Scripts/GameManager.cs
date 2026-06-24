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
    [SerializeField] private GameObject pausePanel;
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
    private bool isPaused = false;
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
    // Màn hình Start: bấm P hoặc click chuột trái để chơi
    if (isStartMenu)
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetMouseButtonDown(0))
        {
            PlayGame();
        }

        return;
    }

    // Màn hình Game Over: bấm R hoặc click chuột trái để chơi lại
    if (isGameOver)
    {
        if (Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(0))
        {
            RestartGame();
        }

        return;
    }

    // Màn hình Pause
    if (isPaused)
    {
        Time.timeScale = 0f;

        // ESC: tiếp tục chơi
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResumeGame();
        }

        // R: chơi lại từ đầu
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        // Q: thoát về StartPanel
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuitToStartMenu();
        }

        return;
    }

    // Đang chơi: bấm ESC để tạm dừng
    if (isPlaying && Input.GetKeyDown(KeyCode.Escape))
    {
        PauseGame();
    }
}

    private void ShowStartMenu()
    {
        Debug.Log("HIEN START MENU");

        StopAllCoroutines();

        flashCoroutine = null;

        isStartMenu = true;
        isPlaying = false;
        isPaused = false;
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

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
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
        isPaused = false;
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

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
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
        if (!isPlaying || isPaused || isGameOver)
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

    public void AddLife(int amount)
    {
        if (!isPlaying || isPaused || isGameOver)
        {
            return;
        }

        lives += amount;

        if (lives > startingLives)
        {
            lives = startingLives;
        }

        UpdateLivesUI();

        Debug.Log("HOI MANG. SO MANG HIEN TAI: " + lives);
    }

    public void HitBigBomb()
    {
        if (!isPlaying || isPaused || isGameOver)
        {
            return;
        }

        if (lives <= 1)
        {
            lives = 0;
            UpdateLivesUI();

            Debug.Log("CHEM BOM TO O MANG CUOI -> GAME OVER");

            Explode();
            return;
        }

        lives--;

        IncreaseScore(-bigBombPenalty);
        UpdateLivesUI();

        Debug.Log("CHEM BOM TO -> MAT 1 MANG, TRU " + bigBombPenalty + " DIEM. CON LAI: " + lives);

        StartFlash(0.45f);
    }

    public void HitSmallBomb(int minusPoints)
    {
        if (!isPlaying || isPaused || isGameOver)
        {
            return;
        }

        IncreaseScore(-minusPoints);

        Debug.Log("CHEM SMALL BOMB -> TRU " + minusPoints + " DIEM");

        StartFlash(0.18f);
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

    public void PauseGame()
    {
        if (!isPlaying || isGameOver || isPaused)
        {
            return;
        }

        Debug.Log("TAM DUNG GAME");

        isPaused = true;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
            AudioManager.Instance.PlayPauseMusic();
        }

        if (blade != null)
        {
            blade.enabled = false;
        }

        if (spawner != null)
        {
            spawner.enabled = false;
        }

        if (fadeImage != null)
        {
            fadeImage.raycastTarget = false;
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
            pausePanel.transform.SetAsLastSibling();
        }

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (!isPaused)
        {
            return;
        }

        Debug.Log("TIEP TUC GAME");

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
            AudioManager.Instance.StopMusic();
        }

        isPaused = false;

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
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
    }

    public void QuitToStartMenu()
    {
        Debug.Log("QUIT VE START MENU");

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }

        ShowStartMenu();
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
        isPaused = false;
        isGameOver = true;

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
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
        Debug.Log("RESTART GAME");

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
            AudioManager.Instance.StopMusic();
        }

        StopAllCoroutines();

        flashCoroutine = null;

        isStartMenu = false;
        isPlaying = true;
        isPaused = false;
        isGameOver = false;

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        ClearScene();

        score = 0;
        lives = startingLives;

        if (startPanel != null)
        {
            startPanel.SetActive(false);
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
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
}