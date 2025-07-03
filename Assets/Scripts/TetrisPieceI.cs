using UnityEngine;

public class TetrisPieceI : MonoBehaviour
{
    void Start()
    {
        CreateIPiece();
    }
    
    void CreateIPiece()
    {
        // I piece: 4 cubes in a line
        Vector3[] positions = {
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 2, 0),
            new Vector3(0, 3, 0)
        };
        
        CreateCubes(positions, Color.cyan);
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