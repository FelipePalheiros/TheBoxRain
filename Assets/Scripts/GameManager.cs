using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject boxPrefab;
    [SerializeField]
    private int maxboxtoSpawn = 3;
    [SerializeField]
    private TMPro.TextMeshProUGUI scoreText;
    [SerializeField]
    private Image backgroudMenu;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject mainVcam;
    [SerializeField]
    private GameObject zoomVcam;
    [SerializeField]
    private GameObject gameOverMenu;

    private int highScore;
    private int score;
    private float timer;
    private Coroutine boxCoroutine;
    private  bool gameOver;

    private static GameManager instance;
    private const string HighScorePreferenceKey = "HighScore";

    public static GameManager Instance => instance;
    public int HighScore => highScore;

    void Start()
    {
        instance = this;
        highScore = PlayerPrefs.GetInt(HighScorePreferenceKey);
    }

    private void OnEnable()
    {
        player.SetActive(value: true);

        zoomVcam.SetActive(value: false);
        mainVcam.SetActive(value: true);

        gameOver = false;
        scoreText.text = "0";
        score = 0;
        timer = 0;
        boxCoroutine = StartCoroutine(SpawnBox());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Time.timeScale == 0)
            {
                Resume();
            }
            if (Time.timeScale == 1)
            {
                Pause();
            }
        }
        
            if (gameOver)
            return;

        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            score++;
            scoreText.text = score.ToString();

            timer = 0;
        }
    }

    private void Pause()
    {
        LeanTween.value(from: 1, to: 0, time: 0.75f)
            .setOnUpdate(SetTimeScale)
            .setIgnoreTimeScale(useUnScaledTime: true);
        backgroudMenu.gameObject.SetActive(value: true);
    }

    private void Resume()
    {
        LeanTween.value(from: 0, to: 1, time: 0.75f)
                            .setOnUpdate(SetTimeScale)
                            .setIgnoreTimeScale(useUnScaledTime: true);
        backgroudMenu.gameObject.SetActive(value: false);
    }

    private void SetTimeScale(float value)
    {
        Time.timeScale = value;
        Time.fixedDeltaTime = 0.02f * value;
    }

    private IEnumerator SpawnBox()
    {
        var boxtoSpawn = Random.Range(1, maxboxtoSpawn);
        for (int i = 0; i < boxtoSpawn; i++)
        {
            var x = Random.Range(-7, 7);
            var drag = Random.Range(0f, 2f);
            var box = Instantiate(boxPrefab, new Vector3(x, 11, 0), Quaternion.identity);
            box.GetComponent<Rigidbody>().drag = drag;
        }

        var timeToWait = Random.Range(0.5f, 1.5f);
        yield return new WaitForSeconds(timeToWait);
        yield return SpawnBox();
    }

    public void GameOver()
    {
        StopCoroutine(boxCoroutine);
        gameOver = true;

        if(Time.timeScale < 1) 
        {
            LeanTween.value(Time.timeScale, to: 1, time: 0.75f)
                    .setOnUpdate(SetTimeScale)
                    .setIgnoreTimeScale(useUnScaledTime: true);
            backgroudMenu.gameObject.SetActive(value: false);
        }

        if(score >highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(HighScorePreferenceKey, highScore);
            Debug.Log(highScore);
        }

        mainVcam.SetActive(value: false);
        zoomVcam.SetActive(value: true);

        gameObject.SetActive(value: false);
        gameOverMenu.SetActive(value: true);
    }

    public void Enabled()
    {
        gameObject.SetActive(value: true);
    }
}