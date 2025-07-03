using UnityEngine;

public class TetrisPieceO : MonoBehaviour
{
    void Start()
    {
        CreateOPiece();
    }
    
    void CreateOPiece()
    {
        // O piece: 2x2 square
        Vector3[] positions = {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0)
        };
        
        CreateCubes(positions, Color.yellow);
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