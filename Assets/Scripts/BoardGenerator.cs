using UnityEngine;

public class BoardGenerator : MonoBehaviour
{
    public GameObject tilePrefab; 
    public int gridSize = 8;
    public float spacing = 1.0f;

    void Start()
    {
        GenerateBoard();
    }

    void GenerateBoard()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 position = new Vector3(x * spacing, 0, z * spacing);
                Instantiate(tilePrefab, position, Quaternion.identity);
            }
        }
    }
}
