using UnityEngine;

namespace Tetris3D
{
    public class InputManager : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameManager.Instance.isPaused)
                    GameManager.Instance.ResumeGame();
                else
                    GameManager.Instance.PauseGame();
            }
            if (Input.GetKeyDown(KeyCode.Return) && GameManager.Instance.isGameOver)
            {
                GameManager.Instance.StartGame();
            }
        }
    }
}
