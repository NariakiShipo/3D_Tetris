using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text scoreText;
    public Text linesText;
    public Text levelText;
    public Text speedText;
    public GameObject gameStatePanel;
    public Text gameStateText;

    public void UpdateAll(int score, int lines, int level, float speed)
    {
        UpdateScore(score);
        UpdateLines(lines);
        UpdateLevel(level);
        UpdateSpeed(speed);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void UpdateLines(int lines)
    {
        linesText.text = $"Lines: {lines}";
    }

    public void UpdateLevel(int level)
    {
        levelText.text = $"Level: {level}";
    }

    public void UpdateSpeed(float speed)
    {
        speedText.text = $"Speed: {speed:F1}";
    }

    public void ShowGameState(string state)
    {
        gameStatePanel.SetActive(true);
        gameStateText.text = state;
    }

    public void HideGameState()
    {
        gameStatePanel.SetActive(false);
    }

    public void SetGameState(Tetris3D.GameManager.GameState state)
    {
        switch (state)
        {
            case Tetris3D.GameManager.GameState.Menu:
                ShowGameState("GAME START");
                break;
            case Tetris3D.GameManager.GameState.Playing:
                HideGameState();
                break;
            case Tetris3D.GameManager.GameState.Paused:
                ShowGameState("PAUSED");
                break;
            case Tetris3D.GameManager.GameState.GameOver:
                ShowGameState("GAME OVER");
                break;
        }
    }
}
