using UnityEngine;
using Tetris3D;

public class TetrisPiece : MonoBehaviour
{
    public bool Move(Vector3 direction)
    {
        transform.position += direction;
        
        if (IsValidPosition())
        {
            return true;
        }
        else
        {
            transform.position -= direction;
            return false;
        }
    }
    
    public void Rotate(Vector3 axis)
    {
        transform.Rotate(axis * 90f);
        
        if (!IsValidPosition())
        {
            transform.Rotate(axis * -90f);
        }
    }
    
    public bool IsValidPosition()
    {
        Transform[] cubes = GetComponentsInChildren<Transform>();
        var gridManager = GameManager.Instance.gridManager;

        foreach (Transform cube in cubes)
        {
            if (cube == transform) continue;

            Vector3 pos = new Vector3(
                Mathf.RoundToInt(cube.position.x),
                Mathf.RoundToInt(cube.position.y),
                Mathf.RoundToInt(cube.position.z)
            );

            if (!gridManager.IsInsideGrid(pos) || gridManager.IsOccupied(pos))
                return false;
        }
        return true;
    }
    
    public void LockInPlace()
    {
        this.enabled = false;
    }
}