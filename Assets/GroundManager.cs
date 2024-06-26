using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public GameObject groundPrefab;
    public GameObject obstaclePrefab;
    public Transform player;
    public float spawnOffset = 10f;
    public float despawnOffset = 10f;
    public int numGroundPieces = 5;
    public float groundLength = 30f;
    public float obstacleSpawnChance = 0.5f;

    private List<GameObject> activeGrounds;
    private List<GameObject> activeObstacles;
    private float lastSpawnZ;

    void Start()
    {
        activeGrounds = new List<GameObject>();
        activeObstacles = new List<GameObject>();
        lastSpawnZ = 0f;

        for (int i = 0; i < numGroundPieces; i++)
        {
            SpawnGround();
        }
    }

    void Update()
    {
        if (player.position.z - spawnOffset > lastSpawnZ - numGroundPieces * groundLength)
        {
            SpawnGround();
        }

        if (activeGrounds.Count > 0 && player.position.z - despawnOffset > activeGrounds[0].transform.position.z + groundLength)
        {
            DespawnGround();
        }
    }

    void SpawnGround()
    {
        GameObject newGround = Instantiate(groundPrefab, new Vector3(0, 0, lastSpawnZ), Quaternion.identity);
        lastSpawnZ += groundLength;
        activeGrounds.Add(newGround);

        SpawnObstacles(newGround);
    }

    void DespawnGround()
    {
        GameObject oldGround = activeGrounds[0];
        activeGrounds.RemoveAt(0);
        Destroy(oldGround);
    }

    void SpawnObstacles(GameObject ground)
    {
        float rand = Random.value;
        if (rand < obstacleSpawnChance)
        {
            GameObject newObstacle = Instantiate(obstaclePrefab, Vector3.zero, Quaternion.identity);
            PlaceObstacle(newObstacle, ground);
            activeObstacles.Add(newObstacle);
        }
    }
    void PlaceObstacle(GameObject obstacle, GameObject ground)
    {
        float groundWidth = ground.GetComponent<Renderer>().bounds.size.x;
        float groundLength = ground.GetComponent<Renderer>().bounds.size.z;
        float obstacleHeight = obstacle.transform.localScale.y / 2f;

        float[] xPositions = { -groundWidth / 4f, 0f, groundWidth / 4f };
        float[] zPositions = { groundLength / 4f, groundLength / 2f, 3 * groundLength / 4f };

        List<GameObject> existingObstaclesOnGround = activeObstacles.FindAll(o => o.transform.position.z >= ground.transform.position.z && o.transform.position.z <= ground.transform.position.z + groundLength);

        for (int i = 0; i < xPositions.Length; i++)
        {
            for (int j = 0; j < zPositions.Length; j++)
            {
                Vector3 proposedPosition = new Vector3(
                    ground.transform.position.x + xPositions[i],
                    ground.transform.position.y + obstacleHeight,
                    ground.transform.position.z + zPositions[j]
                );

                bool positionIsValid = true;
                foreach (GameObject existingObstacle in existingObstaclesOnGround)
                {
                    if (Vector3.Distance(existingObstacle.transform.position, proposedPosition) < obstacle.transform.localScale.x)
                    {
                        positionIsValid = false;
                        break;
                    }
                }

                if (positionIsValid)
                {
                    obstacle.transform.position = proposedPosition;
                    activeObstacles.Add(obstacle);
                    Debug.Log($"Spawned obstacle at: {obstacle.transform.position}");
                    return;
                }
            }
        }

        Destroy(obstacle);
    }
}