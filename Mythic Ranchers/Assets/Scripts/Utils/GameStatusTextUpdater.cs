using System;
using TMPro;
using UnityEngine;

public class GameStatusTextUpdater : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI dungeonTitleText, playersCountText, enemiesCountText, deathsText, timerText;

    private void Update()
    {
        // Make sure the instance of the game manager is initialized
        if (MythicGameManagerMultiplayer.Instance == null)
        {
            return;
        }

        int dungeonKeyLevel = MythicGameManagerMultiplayer.Instance.DungeonKeyLevel.Value;
        int playerCount = MythicGameManagerMultiplayer.Instance.PlayerCount.Value;
        int enemiesCount = MythicGameManagerMultiplayer.Instance.EnemiesCount.Value;
        int deathsCount = MythicGameManagerMultiplayer.Instance.DeathsCount.Value;
        float timerCount = MythicGameManagerMultiplayer.Instance.TimerCount.Value;

        // Convert seconds to minutes:seconds format
        TimeSpan timeSpan = TimeSpan.FromSeconds(timerCount);
        string timerTextFormatted = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

        // Update TextMeshPro texts
        dungeonTitleText.text = $"Dungeon Level: {dungeonKeyLevel}";
        playersCountText.text = $"Players: {playerCount}";
        enemiesCountText.text = $"Enemies: {enemiesCount}";
        deathsText.text = $"Deaths: {deathsCount}";
        timerText.text = $"Time: {timerTextFormatted}";
    }
}
