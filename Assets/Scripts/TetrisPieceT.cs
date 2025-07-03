using UnityEngine;

public class TetrisPieceT : MonoBehaviour
{
    void Start()
    {
        CreateTPiece();
    }
    
    void CreateTPiece()
    {
        // T piece: T shape
        Vector3[] positions = {
            new Vector3(0, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, 1, 0)
        };
        
        CreateCubes(positions, Color.magenta);
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