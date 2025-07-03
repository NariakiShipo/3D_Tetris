using UnityEngine;

namespace Tetris3D
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Settings")]
        public int gridWidth = 10;
        public int gridDepth = 10;
        public int gridHeight = 20;
        public float initialDropSpeed = 1.0f;
        public float speedIncrement = 0.1f;
        
        [Header("References")]
        public GridManager gridManager;
        public BlockSpawner blockSpawner;
        public UIManager uiManager;
        public InputManager inputManager;
        
        [Header("Game State")]
        public int score = 0;
        public int level = 1;
        public int linesCleared = 0;
        public float currentDropSpeed;
        public bool isGameOver = false;
        public bool isPaused = false;
        
        // Singleton pattern
        public static GameManager Instance { get; private set; }
        
        // Game state enum
        public enum GameState
        {
            Menu,
            Playing,
            Paused,
            GameOver
        }
        
        public GameState currentState = GameState.Menu;
        
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        void Start()
        {
            InitializeGame();
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
            
            if (Input.GetKeyDown(KeyCode.R) && isGameOver)
            {
                RestartGame();
            }
        }
        
        public void InitializeGame()
        {
            currentDropSpeed = initialDropSpeed;
            score = 0;
            level = 1;
            linesCleared = 0;
            isGameOver = false;
            isPaused = false;
            currentState = GameState.Playing;
            
            // Initialize managers
            if (gridManager == null)
                gridManager = FindFirstObjectByType<GridManager>();
            if (blockSpawner == null)
                blockSpawner = FindFirstObjectByType<BlockSpawner>();
            if (uiManager == null)
                uiManager = FindFirstObjectByType<UIManager>();
            if (inputManager == null)
                inputManager = FindFirstObjectByType<InputManager>();
            
            // Setup grid
            gridManager.InitializeGrid(gridWidth, gridDepth, gridHeight);
            
            // Start spawning blocks
            blockSpawner.SpawnFirstBlock();
            
            // Update UI
            uiManager.UpdateScore(score);
            uiManager.UpdateLevel(level);
            uiManager.SetGameState(currentState);
        }
        
        public void StartGame()
        {
            currentState = GameState.Playing;
            isPaused = false;
            isGameOver = false;
            uiManager.SetGameState(currentState);
        }
        
        public void PauseGame()
        {
            if (currentState == GameState.Playing)
            {
                currentState = GameState.Paused;
                isPaused = true;
                Time.timeScale = 0f;
                uiManager.SetGameState(currentState);
            }
        }
        
        public void ResumeGame()
        {
            if (currentState == GameState.Paused)
            {
                currentState = GameState.Playing;
                isPaused = false;
                Time.timeScale = 1f;
                uiManager.SetGameState(currentState);
            }
        }
        
        public void TogglePause()
        {
            if (isGameOver) return;
            
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
        
        public void GameOver()
        {
            isGameOver = true;
            currentState = GameState.GameOver;
            uiManager.SetGameState(currentState);
            Debug.Log("Game Over! Final Score: " + score);
        }
        
        public void RestartGame()
        {
            Time.timeScale = 1f;
            
            // Clear the grid
            gridManager.ClearGrid();
            
            // Reset game state
            InitializeGame();
        }
        
        public void AddScore(int points)
        {
            score += points;
            uiManager.UpdateScore(score);
        }
        
        public void LinesCleared(int numberOfLines)
        {
            linesCleared += numberOfLines;
            
            // Calculate score based on lines cleared
            int points = 0;
            switch (numberOfLines)
            {
                case 1: points = 100 * level; break;
                case 2: points = 300 * level; break;
                case 3: points = 500 * level; break;
                case 4: points = 800 * level; break;
                default: points = numberOfLines * 100 * level; break;
            }
            
            AddScore(points);
            
            // Level up every 10 lines
            int newLevel = (linesCleared / 10) + 1;
            if (newLevel > level)
            {
                level = newLevel;
                currentDropSpeed = Mathf.Max(0.1f, initialDropSpeed - (level - 1) * speedIncrement);
                uiManager.UpdateLevel(level);
            }
        }
        
        public bool IsGameActive()
        {
            return currentState == GameState.Playing && !isPaused && !isGameOver;
        }
    }
}