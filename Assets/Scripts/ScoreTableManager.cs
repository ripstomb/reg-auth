using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScoreTableManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayerScore
    {
        public string playerName;
        public int score;
    }

    public List<PlayerScore> scoresList = new List<PlayerScore>();
    public Text scoreboardText;
    public GameObject scoreboardPanel;

    void Start()
    {
        scoreboardPanel.SetActive(false);
    }

    public void ShowScoreboard()
    {
        // Ordenar la lista de puntajes de mayor a menor
        scoresList.Sort((a, b) => b.score.CompareTo(a.score));

        // Mostrar la tabla de puntajes en el texto
        string scoreboardString = "Scoreboard:\n";
        foreach (PlayerScore playerScore in scoresList)
        {
            scoreboardString += $"{playerScore.playerName}: {playerScore.score}\n";
        }
        scoreboardText.text = scoreboardString;

        // Activar el panel de la tabla de puntajes
        scoreboardPanel.SetActive(true);
    }

    public void CloseScoreboard()
    {
        // Desactivar el panel de la tabla de puntajes
        scoreboardPanel.SetActive(false);
    }
}
