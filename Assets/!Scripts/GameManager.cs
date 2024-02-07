using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static Difficulty currDifficulty = Difficulty.Medium;

    [SerializeField]
    LayerMask baloonLayer = new LayerMask();
    [SerializeField]
    BaloonSpawner spawner;
    [SerializeField]
    LeaderboardManager leaderboardManager;
    [SerializeField]
    TMP_InputField inputField;
    [SerializeField]
    TextMeshProUGUI timerText;
    [SerializeField]
    GameObject playAgainScreen;
    [SerializeField]
    TextMeshProUGUI scoreText;
        
    int currDamage = 1;
    int score = 0;
    string userName;

    void Awake()
    {
        Application.targetFrameRate = 60;
        Baloon.onBaloonPop += OnBaloonPopped;
    }
    private void Start()
    {
        inputField.text = PlayerPrefs.GetString("userName", "Player");
    }
    private void OnDestroy()
    {
        Baloon.onBaloonPop -= OnBaloonPopped;
    }
    public void OnPlay()
    {
        userName = inputField.text;
        if (userName == "")
            userName = "Player";
        PlayerPrefs.SetString("userName", userName);
        score = 0;
        spawner.StartSpawning();
        TimerStart();
    }
    public void ChangeDifficulty(Difficulty difficulty)
    {
        currDifficulty = difficulty;
    }
    public void ChangeDifficulty(DifficultyButton btn)
    {
        ChangeDifficulty(btn.difficulty);
    }
    void TimerStart()
    {
        timerText.text = "";
        StartCoroutine(TimerCor());
    }
    IEnumerator TimerCor()
    {
        float t = 0;
        float dur = 15f;
        while (t < dur)
        {
            t += Time.deltaTime;
            timerText.text = Mathf.RoundToInt(dur - t).ToString();
            yield return null;
        }
        spawner.StopSpawning();
        timerText.text = "...";
        while (spawner.GetActiveAmount() > 0)
        {
            yield return null;
        }
        TimerStop();
    }
    void TimerStop()
    {
        timerText.text = "";
        scoreText.text = "Score : " + score;
        spawner.StopSpawning();
        if (leaderboardManager.HandleEntry(score, userName))
            scoreText.text += " (New record!)";
        playAgainScreen.SetActive(true);
    }
    private void OnBaloonPopped(int reward,Baloon baloon,bool byPlayer)
    {
        if(byPlayer)
            score += reward;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), r.direction, float.MaxValue);
            if (hit.collider != null)
            {
                if (Mathf.Pow(2, hit.transform.gameObject.layer) == baloonLayer.value)
                    hit.transform.GetComponent<Baloon>().TakeDamage(currDamage);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(Camera.main.transform.position, Input.mousePosition);
    }
}
