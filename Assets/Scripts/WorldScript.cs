using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldScript : MonoBehaviour
{

    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject monstersParent;
    [SerializeField] GameObject monsterTemplate;
    [SerializeField] GameObject bossMonsterTemplate;
    [SerializeField] GameObject[] monsterHeart;
    [SerializeField] TextScript roundText;
    [SerializeField] CameraScript cameraScript;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] PlayerScript player;
    [SerializeField] TextScript maxRoundsText;
    [SerializeField] TextScript maxCoinsText;
    [SerializeField] GameObject[] UIObjects;

    public int round = 0;
    public int coins = 0;
    public int respawnMonstersCount = 6;
    public int respawnBossMonstersCount = 0;
    public int monstersHealth = 100;
    public int bossMonstersHealth = 500;

    private int roundChangeDuration = 5;
    private float roundChangeTimer = 0;
    private bool canChange = false;
    private bool shaking = false;
    private bool paused = false;
    Animator roundAnimator;

    public int maxRounds = 0;
    public int maxCoins = 0;


    // Start is called before the first frame update
    void Start()
    {
        LoadPrefs();
        modifyMaxes();
        PauseGame();
        roundText.SetText("#1");
         roundAnimator = roundText.GetComponent<Animator>();
        coins = player.coins;
    }

    // Update is called once per frame
    void Update()
    {

        modifyMaxes();
        checkInputs();
        CheckRoundFinished();
    }

    public void SavePrefs()
    {
        PlayerPrefs.SetInt("maxRounds", maxRounds);
        PlayerPrefs.SetInt("maxCoins", maxCoins);
        PlayerPrefs.Save();
    }

    public void LoadPrefs()
    {
        maxRounds = PlayerPrefs.GetInt("maxRounds", 0);
        maxCoins = PlayerPrefs.GetInt("maxCoins", 0);
    }

    public void modifyMaxes() {
        coins = player.coins;
        if (round > maxRounds)
        {
            maxRounds = round;

        }
        if (coins > maxCoins)
        {
            maxCoins = coins;

        }
        SavePrefs();
        modifyMaxesTexts();
    }

    void modifyMaxesTexts()
    {
        maxRoundsText.SetText(maxRounds.ToString());
        maxCoinsText.SetText(maxCoins.ToString());
    }

    void checkInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(paused)
            {
                ResumeGame();
            } else
            {
                PauseGame();
            }
        }
    }

    public void ExitGame()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    public void PauseGame()
    {

        ToggleUI();

        pauseMenu.GetComponent<CanvasGroup>().alpha = pauseMenu.GetComponent<CanvasGroup>().alpha == 1 ? 0 : 1;
        pauseMenu.GetComponent<CanvasGroup>().interactable = pauseMenu.GetComponent<CanvasGroup>().interactable ? false : true;
        pauseMenu.GetComponent<CanvasGroup>().blocksRaycasts = pauseMenu.GetComponent<CanvasGroup>().blocksRaycasts ? false : true;
        paused = true;
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {

        ToggleUI();

        paused = false;
        pauseMenu.GetComponent<CanvasGroup>().alpha = pauseMenu.GetComponent<CanvasGroup>().alpha == 1 ? 0 : 1;
        pauseMenu.GetComponent<CanvasGroup>().interactable = pauseMenu.GetComponent<CanvasGroup>().interactable ? false : true;
        pauseMenu.GetComponent<CanvasGroup>().blocksRaycasts = pauseMenu.GetComponent<CanvasGroup>().blocksRaycasts ? false : true;
        Time.timeScale = 1;
    }

    public void ToggleUI()
    {
        for (int i = 0; i < UIObjects.Length; i++)
        {

            UIObjects[i].GetComponent<CanvasGroup>().alpha = UIObjects[i].GetComponent<CanvasGroup>().alpha == 1 ? 0 : 1;
            UIObjects[i].GetComponent<CanvasGroup>().interactable = UIObjects[i].GetComponent<CanvasGroup>().interactable ? false : true;
            UIObjects[i].GetComponent<CanvasGroup>().blocksRaycasts = UIObjects[i].GetComponent<CanvasGroup>().blocksRaycasts ? false : true;
        }
    }

    void RespawnMonsters()
    {
        for(int i = 0; i< respawnMonstersCount; i++)
        {
            int randomSpawner = new System.Random().Next(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomSpawner];
            BoxCollider2D boxCollider2D = spawnPoint.GetComponent<BoxCollider2D>();

            float randomX = new System.Random().Next(Convert.ToInt32(spawnPoint.position.x - (boxCollider2D.size.x)/4), Convert.ToInt32(spawnPoint.position.x + (boxCollider2D.size.x) / 4));
            float randomY = new System.Random().Next(Convert.ToInt32(spawnPoint.position.y - (boxCollider2D.size.y) / 4), Convert.ToInt32(spawnPoint.position.y + (boxCollider2D.size.y) / 4));

            Vector3 spawnPosition = new Vector3(randomX, randomY, 0);
            GameObject newMonster = Instantiate(monsterTemplate, spawnPosition,new Quaternion() ,monstersParent.transform);
            newMonster.SetActive(true);
        }
        for (int i = 0; i < respawnBossMonstersCount; i++)
        {
            int randomSpawner = new System.Random().Next(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomSpawner];
            BoxCollider2D boxCollider2D = spawnPoint.GetComponent<BoxCollider2D>();

            float randomX = new System.Random().Next(Convert.ToInt32(spawnPoint.position.x - (boxCollider2D.size.x) / 4), Convert.ToInt32(spawnPoint.position.x + (boxCollider2D.size.x) / 4));
            float randomY = new System.Random().Next(Convert.ToInt32(spawnPoint.position.y - (boxCollider2D.size.y) / 4), Convert.ToInt32(spawnPoint.position.y + (boxCollider2D.size.y) / 4));

            Vector3 spawnPosition = new Vector3(randomX, randomY, 0);
            GameObject newBossMonster = Instantiate(bossMonsterTemplate, spawnPosition, new Quaternion(), monstersParent.transform);
            newBossMonster.SetActive(true);
        }
    }

    void CheckRoundFinished()
    {

        if (monstersParent.transform.childCount == 2 && canChange)
        {
            round += 1;
            roundText.SetText("#" + round.ToString());

            roundText.SetColor(new Color(221f / 255f, 29f / 255f, 62f / 255f, 1));

            if(round >= 5)
            {
                respawnBossMonstersCount += 1;
            }
            RespawnMonsters();

            monstersHealth = Convert.ToInt32(monstersHealth * 1.2f);
            bossMonstersHealth = Convert.ToInt32(bossMonstersHealth * 1.2f);
            for (int i = 0; i<monsterHeart.Length; i++)
            {
                monsterHeart[i].transform.localScale = new Vector3(monsterHeart[i].transform.localScale.x * 1.05f, monsterHeart[i].transform.localScale.y * 1.05f);
            }
            
            //monsterHeart.transform.GetChild(0).GetComponent<Text>().fontSize -= 10 ;
            monsterTemplate.GetComponent<MonsterScript>().SetMaxHealth(monstersHealth);
            bossMonsterTemplate.GetComponent<MonsterScript>().SetMaxHealth(bossMonstersHealth);



            respawnMonstersCount += 2;
            canChange = false;
            roundChangeTimer = 0;
        } else if (monstersParent.transform.childCount == 2 && !canChange)
        {
            StartTimer();
            if (!shaking)
            {
                cameraScript.Shake(roundChangeDuration);
                shaking = true;
            }
            if (!roundAnimator.GetCurrentAnimatorStateInfo(0).IsName("RoundStart"))
            {
                roundAnimator.SetBool("Start", true);
            } 
            roundText.SetText("#" + (round+1).ToString());
            roundText.SetColor(new Color(1, 1, 1, 1));
            

        }
    }
    void StartTimer()
    {
        if (roundChangeTimer >= roundChangeDuration)
        {
            canChange = true;
            roundChangeTimer = 0;
            roundAnimator.SetBool("Start", false);
        } else
        {
            roundChangeTimer += Time.deltaTime;
            canChange = false;
            shaking = false;
        }
    }
}
