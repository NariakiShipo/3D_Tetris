using UnityEngine;

public class TetrisPieceL : MonoBehaviour
{
    void Start()
    {
        CreateLPiece();
    }
    
    void CreateLPiece()
    {
        // L piece: L shape
        Vector3[] positions = {
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 2, 0),
            new Vector3(1, 0, 0)
        };
        
        CreateCubes(positions, new Color(1f, 0.5f, 0f)); // Orange
    }
    
    void CreateCubes(Vector3[] positions, Color color)
    {
        foreach (Vector3 pos in positions)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = transform;
            cube.transform.localPosition = pos;
            cube.tag = "TetrisCube";
            
            Renderer renderer = cube.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = color;
            }
        }
    }
}