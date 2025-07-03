using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static int width = 10;
    public static int depth = 10;
    public static int height = 20;
    private Transform[,,] grid = new Transform[width, height, depth];

    void Start()
    {
        CreateGridVisual();
    }

    public void ResetGrid()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                for (int z = 0; z < depth; z++)
                    if (grid[x, y, z] != null)
                    {
                        Destroy(grid[x, y, z].gameObject);
                        grid[x, y, z] = null;
                    }
    }

    public bool IsInsideGrid(Vector3 pos)
    {
        return (int)pos.x >= 0 && (int)pos.x < width && (int)pos.z >= 0 && (int)pos.z < depth && (int)pos.y >= 0 && (int)pos.y < height;
    }

    public bool IsOccupied(Vector3 pos)
    {
        if (!IsInsideGrid(pos)) return true;
        return grid[(int)pos.x, (int)pos.y, (int)pos.z] != null;
    }

    public void AddToGrid(Transform block)
    {
        foreach (Transform cube in block)
        {
            Vector3 pos = RoundVec3(cube.position);
            if (IsInsideGrid(pos))
            {
                grid[(int)pos.x, (int)pos.y, (int)pos.z] = cube;
            }
        }
    }

    public void RemoveFromGrid(Transform block)
    {
        foreach (Transform cube in block)
        {
            Vector3 pos = RoundVec3(cube.position);
            if (IsInsideGrid(pos))
                grid[(int)pos.x, (int)pos.y, (int)pos.z] = null;
        }
    }

    public int CheckAndClearFullLayers()
    {
        int cleared = 0;
        for (int y = 0; y < height; y++)
        {
            if (IsLayerFull(y))
            {
                ClearLayer(y);
                MoveAllAboveDown(y);
                cleared++;
                y--;
            }
        }
        return cleared;
    }

    private bool IsLayerFull(int y)
    {
        for (int x = 0; x < width; x++)
            for (int z = 0; z < depth; z++)
                if (grid[x, y, z] == null)
                    return false;
        return true;
    }

    private void ClearLayer(int y)
    {
        for (int x = 0; x < width; x++)
            for (int z = 0; z < depth; z++)
            {
                Destroy(grid[x, y, z].gameObject);
                grid[x, y, z] = null;
            }
    }

    private void MoveAllAboveDown(int y)
    {
        for (int i = y + 1; i < height; i++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    if (grid[x, i, z] != null)
                    {
                        grid[x, i - 1, z] = grid[x, i, z];
                        grid[x, i, z] = null;
                        grid[x, i - 1, z].position += Vector3.down;
                    }
                }
            }
        }
    }

    private Vector3 RoundVec3(Vector3 v)
    {
        return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
    }

    public bool IsTopLayerBlocked()
    {
        int y = height - 1;
        for (int x = 0; x < width; x++)
            for (int z = 0; z < depth; z++)
                if (grid[x, y, z] != null)
                    return true;
        return false;
    }

    public void InitializeGrid(int width, int depth, int height)
    {
        GridManager.width = width;
        GridManager.depth = depth;
        GridManager.height = height;
        grid = new Transform[width, height, depth];
        ResetGrid();
    }

    public void ClearGrid()
    {
        ResetGrid();
    }

    public void CreateGridVisual()
    {
        // 生成地板格線
        GameObject floorParent = new GameObject("GridFloorVisual");
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                GameObject tile = GameObject.CreatePrimitive(PrimitiveType.Cube);
                tile.transform.position = new Vector3(x, -0.5f, z);
                tile.transform.localScale = new Vector3(1, 0.05f, 1);
                tile.GetComponent<Renderer>().material.color = new Color(0.3f, 0.3f, 0.3f, 1f);
                tile.transform.parent = floorParent.transform;
            }
        }
        // 生成四周牆體
        GameObject wallParent = new GameObject("GridWallsVisual");
        Color wallColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);
        for (int x = -1; x <= width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // 左右牆
                if (x == -1 || x == width)
                {
                    for (int z = 0; z < depth; z++)
                    {
                        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        wall.transform.position = new Vector3(x, y, z);
                        wall.transform.localScale = new Vector3(1, 1, 1);
                        var renderer = wall.GetComponent<Renderer>();
                        renderer.enabled = false;
                        wall.transform.parent = wallParent.transform;
                    }
                }
            }
        }
        for (int z = -1; z <= depth; z++)
        {
            for (int y = 0; y < height; y++)
            {
                // 前後牆
                if (z == -1 || z == depth)
                {
                    for (int x = 0; x < width; x++)
                    {
                        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        wall.transform.position = new Vector3(x, y, z);
                        wall.transform.localScale = new Vector3(1, 1, 1);
                        var renderer = wall.GetComponent<Renderer>();
                        renderer.enabled = false;
                        wall.transform.parent = wallParent.transform;
                    }
                }
            }
        }
    }
}
