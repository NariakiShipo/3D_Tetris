using UnityEngine;
using UnityEngine.UI;

public class SimpleGameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public int gridWidth = 10;
    public int gridDepth = 10;
    public int gridHeight = 20;
    public float fallSpeed = 1.0f;
    
    [Header("UI References")]
    public Text scoreText;
    public Text levelText;
    public Text gameOverText;
    
    [Header("Game Objects")]
    public GameObject[] tetrisPieces;
    public Camera gameCamera;
    
    public bool[,,] grid;
    public GameObject currentPiece;
    public int currentScore = 0;
    public int currentLevel = 1;
    public bool gameOver = false;
    
    private float fallTimer = 0f;
    private Vector3 spawnPosition;
    
    void Start()
    {
        grid = new bool[gridWidth, gridHeight, gridDepth];
        spawnPosition = new Vector3(0, gridHeight - 1, 0);
        
        if (gameCamera != null)
        {
            gameCamera.transform.position = new Vector3(0, 15, -15);
            gameCamera.transform.LookAt(new Vector3(0, gridHeight/2, 0));
        }
        
        SpawnNewPiece();
        UpdateUI();
    }
    
    void Update()
    {
        if (gameOver) return;
        
        HandleInput();
        HandleFalling();
    }
    
    void HandleInput()
    {
        if (currentPiece == null) return;
        
        TetrisPiece pieceScript = currentPiece.GetComponent<TetrisPiece>();
        if (pieceScript == null) return;
        
        if (Input.GetKeyDown(KeyCode.A)) pieceScript.Move(Vector3.left);
        if (Input.GetKeyDown(KeyCode.D)) pieceScript.Move(Vector3.right);
        if (Input.GetKeyDown(KeyCode.W)) pieceScript.Move(Vector3.forward);
        if (Input.GetKeyDown(KeyCode.S)) pieceScript.Move(Vector3.back);
        
        if (Input.GetKeyDown(KeyCode.Q)) pieceScript.Rotate(Vector3.up);
        if (Input.GetKeyDown(KeyCode.E)) pieceScript.Rotate(Vector3.down);
        
        if (Input.GetKey(KeyCode.Space))
            fallTimer = fallSpeed;
    }
    
    void HandleFalling()
    {
        if (currentPiece == null) return;
        
        fallTimer += Time.deltaTime;
        if (fallTimer >= fallSpeed)
        {
            TetrisPiece pieceScript = currentPiece.GetComponent<TetrisPiece>();
            if (pieceScript != null)
            {
                bool canMove = pieceScript.Move(Vector3.down);
                if (!canMove)
                {
                    LockPiece();
                    SpawnNewPiece();
                }
            }
            fallTimer = 0f;
        }
    }
    
    void SpawnNewPiece()
    {
        if (tetrisPieces.Length == 0) return;
        
        int randomIndex = Random.Range(0, tetrisPieces.Length);
        currentPiece = Instantiate(tetrisPieces[randomIndex], spawnPosition, Quaternion.identity);
        
        TetrisPiece pieceScript = currentPiece.GetComponent<TetrisPiece>();
        if (pieceScript != null && !pieceScript.IsValidPosition())
        {
            GameOver();
        }
    }
    
    void LockPiece()
    {
        if (currentPiece == null) return;
        
        TetrisPiece pieceScript = currentPiece.GetComponent<TetrisPiece>();
        if (pieceScript != null)
        {
            pieceScript.LockInPlace();
        }
    }
    
    void GameOver()
    {
        gameOver = true;
        if (gameOverText != null)
        {
            gameOverText.text = "Game Over!\nScore: " + currentScore;
            gameOverText.gameObject.SetActive(true);
        }
    }
    
    public bool IsValidGridPosition(int x, int y, int z)
    {
        return x >= 0 && x < gridWidth && 
               y >= 0 && y < gridHeight && 
               z >= 0 && z < gridDepth;
    }
    
    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + currentScore;
        if (levelText != null) levelText.text = "Level: " + currentLevel;
    }
}