using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int lives;
    private int score;
    private int time;
    private float levelSpeedUp = 1.1f;
    private Home[] homes;
    private Frogger frogger;

    public static GameManager instance;
    public Text livesText;
    public Text scoreText;
    public Text timeText;
    public Image gameOver;

    private void Awake() {
        if(instance == null)
            instance = this;
        else
            DestroyImmediate(gameObject);
        frogger = FindObjectOfType<Frogger>();
        homes = FindObjectsOfType<Home>();    
    }
    
    private void Start() {
        NewGame();
    }

    private void NewGame()
    {
        gameOver.gameObject.SetActive(false);
        SetScore(0);
        SetLives(3);
        NewLevel();
    }

    private void NewLevel()
    {
        for(int i=0; i<homes.Length; i++)
        {
            homes[i].enabled = false;
        }

        Respawn();
    }

    private void Respawn()
    {
        frogger.Respawn();

        StopAllCoroutines();
        StartCoroutine(Timer(30));
    }

    IEnumerator Timer(int duration)
    {
        time = duration;
        while(time >0 )
        {
            yield return new WaitForSeconds(1);
            SetTime(time-1);
        }
        frogger.Death();
    } 

    public void Died()
    {
        SetLives(lives - 1);
        if(lives > 0)
        {
            Invoke(nameof(Respawn),1f);
        }
        else
        {
            Invoke(nameof(GameOver),1f);
        }
    }

    private void GameOver()
    {
        gameOver.gameObject.SetActive(true);
    }
    
    public void OnClickReplay()
    {
        SceneManager.LoadScene(0);
    }

    public void HomeOccupied()
    {
        int bonusPoint = time * 20;
        SetScore(score + bonusPoint + 50);
        if(Cleared())
        {
            SetScore(score+1000);
            SetLives(lives + 1);
            Invoke(nameof(NewLevel),1f);
        }
        else
            Invoke(nameof(Respawn),1f);
    }

    private bool Cleared()
    {
        for(int i=0; i<homes.Length; i++)
        {
            if(!homes[i].enabled)
                return false;
        }
        return true;
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = "Score: " + this.score;
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "Lives: " + this.lives;
    }

    private void SetTime(int time)
    {
        this.time = time;
        timeText.text = "Time: " + this.time;
    }
}