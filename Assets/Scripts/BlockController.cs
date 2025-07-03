using UnityEngine;
using System.Collections;
using Tetris3D;

namespace Tetris3D
{
public class BlockController : MonoBehaviour
{
    private BlockSpawner spawner;
    private float fallTimer = 0f;
    private float fallInterval;
    private bool isActive = true;

    public void Init(BlockSpawner spawner)
    {
        this.spawner = spawner;
        fallInterval = GameManager.Instance.currentDropSpeed;
        isActive = true;
    }

    void Update()
    {
        if (!isActive || GameManager.Instance.isPaused || GameManager.Instance.isGameOver) return;
        HandleInput();
        fallTimer += Time.deltaTime;
        if (fallTimer >= fallInterval)
        {
            Move(Vector3.down);
            fallTimer = 0f;
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) Move(Vector3.forward);
        if (Input.GetKeyDown(KeyCode.S)) Move(Vector3.back);
        if (Input.GetKeyDown(KeyCode.A)) Move(Vector3.left);
        if (Input.GetKeyDown(KeyCode.D)) Move(Vector3.right);
        if (Input.GetKeyDown(KeyCode.Space)) while (Move(Vector3.down)) ;
        if (Input.GetKeyDown(KeyCode.Q)) Rotate(Vector3.up);
        if (Input.GetKeyDown(KeyCode.E)) Rotate(Vector3.down);
        if (Input.GetKeyDown(KeyCode.R)) Rotate(Vector3.left);
        if (Input.GetKeyDown(KeyCode.F)) Rotate(Vector3.right);
        // if (Input.GetKeyDown(KeyCode.C)) spawner.HoldBlock(); // Hold 功能未實作
    }

    bool Move(Vector3 dir)
    {
        transform.position += dir;
        if (!IsValidPosition())
        {
            transform.position -= dir;
            return false;
        }
        // 檢查是否到底（y=0）
        foreach (Transform cube in transform)
        {
            if (Mathf.RoundToInt(cube.position.y) <= 0)
            {
                LandBlock();
                return false;
            }
        }
        return true;
    }

    void Rotate(Vector3 axis)
    {
        transform.Rotate(axis * 90, Space.World);
        if (!IsValidPosition())
            transform.Rotate(-axis * 90, Space.World);
    }

    bool IsValidPosition()
    {
        foreach (Transform cube in transform)
        {
            Vector3 pos = RoundVec3(cube.position);
            if (!GameManager.Instance.gridManager.IsInsideGrid(pos) || GameManager.Instance.gridManager.IsOccupied(pos))
                return false;
        }
        return true;
    }

    void LandBlock()
    {
        isActive = false;
        GameManager.Instance.gridManager.AddToGrid(transform);
        int cleared = GameManager.Instance.gridManager.CheckAndClearFullLayers();
        if (cleared > 0)
        {
            GameManager.Instance.AddScore(100 * cleared);
            GameManager.Instance.LinesCleared(cleared);
        }
        if (IsGameOver())
        {
            GameManager.Instance.GameOver();
            Destroy(this);
        }
        else
        {
            if (spawner != null) spawner.SpawnNewBlock();
            Destroy(this);
        }
    }

    bool IsGameOver()
    {
        foreach (Transform cube in transform)
        {
            Vector3 pos = RoundVec3(cube.position);
            if (pos.y >= GridManager.height - 1 && GameManager.Instance.gridManager.IsOccupied(pos))
                return true;
        }
        return false;
    }

    Vector3 RoundVec3(Vector3 v)
    {
        return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
    }
}
}
