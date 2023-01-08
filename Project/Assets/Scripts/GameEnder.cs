using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnder : Singleton<GameEnder>
{
    public bool Lost { get; private set; }

    [SerializeField] SceneTransitioner sceneTransitioner;
    [SerializeField] TMP_Text beetsHarvestedText;
    [SerializeField] TMP_Text enemiesKilledText;
    [SerializeField] TMP_Text timeSurvivedText;
    [SerializeField] GameObject loseUI;
    int beetsHarvested;
    int enemiesKilled;
    float timeSurvived;

    public void HarvestedBeet()
    {
        if (Lost) return;
        beetsHarvested++;
    }
    public void KilledEnemy() => enemiesKilled++;

    private void OnEnable()
    {
        Farmland[] farms = FindObjectsOfType<Farmland>();

        for (int i = 0; i < farms.Length; i++)
            farms[i].BeetHarvested += HarvestedBeet;
    }

    private void Start()
    {
        EnemySpawner.I.EnemyKilled += KilledEnemy;
        PlayerPrefs.SetFloat("Survived", 0);
        PlayerPrefs.SetInt("Harvested", 0);
        PlayerPrefs.SetInt("Killed", 0);
    }

    public void Lose()
    {
        EnemySpawner.I.EnemyKilled -= KilledEnemy;
        Farmland[] farms = FindObjectsOfType<Farmland>();
        for (int i = 0; i < farms.Length; i++)
            farms[i].BeetHarvested -= HarvestedBeet;

        float timeSurvivedHighscore = PlayerPrefs.GetFloat("Survived");
        int harvestHighscore = PlayerPrefs.GetInt("Harvested");
        int killsHighscore = PlayerPrefs.GetInt("Killed");

        timeSurvivedText.text = $"Survived: {timeSurvived.ToString("N3")}";
        beetsHarvestedText.text = $"Harvested: {beetsHarvested}";
        enemiesKilledText.text = $"Killed: {enemiesKilled}";

        if (beetsHarvested > harvestHighscore)
        {
            PlayerPrefs.SetInt("Harvested", beetsHarvested);
            beetsHarvestedText.text += "<color=yellow>!</color>";
        }
        else if (beetsHarvested == harvestHighscore)
            beetsHarvestedText.text += "<color=lightblue>-</color>";

        if (enemiesKilled > killsHighscore)
        {
            PlayerPrefs.SetInt("Killed", enemiesKilled);
            enemiesKilledText.text += "<color=yellow>!</color>";
        }
        else if (enemiesKilled == killsHighscore)
            enemiesKilledText.text += "<color=lightblue>-</color>";

        if (timeSurvived > timeSurvivedHighscore)
        {
            PlayerPrefs.SetFloat("Survived", timeSurvived);
            timeSurvivedText.text += "<color=yellow>!</color>";
        }
        else if(timeSurvived == timeSurvivedHighscore)
            timeSurvivedText.text += "<color=lightblue>-</color>";

        beetsHarvestedText.text += $"\nHigh: <color=lightblue>{harvestHighscore}</color>";
        enemiesKilledText.text += $"\nHigh: <color=lightblue>{killsHighscore}</color>";
        timeSurvivedText.text += $"\nHigh: <color=lightblue>{timeSurvivedHighscore.ToString("N3")}</color>";

        Lost = true;
        EnemySpawner.I.KillAllEnemies();
        loseUI.SetActive(true);
    }

    void Update()
    {
        if (Lost)
        {
            if (Input.GetKeyDown(KeyCode.R))
                sceneTransitioner.TransitionToScene("Game");
            else if (Input.GetKeyDown(KeyCode.M))
                sceneTransitioner.TransitionToScene("Menu");
        }
        else
            timeSurvived += Time.deltaTime;
    }
}
