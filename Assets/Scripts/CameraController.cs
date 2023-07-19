using UnityEngine;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    public GameObject character; // GameObject để camera theo dõi
    public GameObject tilePrefab; // Prefab của tile để sinh ra
    public float tileSize = 1f; // Kích thước của mỗi tile
    public int viewDistance = 5; // Khoảng cách camera hiển thị map
    public GameObject parentContainer;
    public GameObject spawnedObjectsGroup; // GameObject để nhóm các mục sinh ra
    public GameObject[] obstaclePrefabs; // Mảng Prefab của các vật cản

    public float minDistance = 1f; // Khoảng cách tối thiểu giữa các vật cản
    public float maxDistance = 3f; // Khoảng cách tối đa giữa các vật cản

    private Vector2Int previousCameraPosition; // Lưu trữ vị trí trước đó của camera
    private List<Vector2Int> oldTilePositions = new List<Vector2Int>(); // Lưu trữ vị trí của các tile cũ
    private List<Vector2Int> oldObstaclePositions = new List<Vector2Int>(); // Lưu trữ vị trí của các vật cản cũ

    private Dictionary<Vector2Int, GameObject> tiles = new Dictionary<Vector2Int, GameObject>();
    private Dictionary<Vector2Int, GameObject> obstacles = new Dictionary<Vector2Int, GameObject>();

    private void Start()
    {
        GenerateInitialMap();
    }

    private void Update()
    {
        // Di chuyển camera theo vị trí của nhân vật
        transform.position = new Vector3(character.transform.position.x, character.transform.position.y, transform.position.z);
        UpdateMap();
    }

    private void GenerateInitialMap()
    {
        // Sinh ra các tile ban đầu trong khoảng viewDistance x viewDistance
        for (int x = -viewDistance; x <= viewDistance; x++)
        {
            for (int y = -viewDistance; y <= viewDistance; y++)
            {
                Vector2Int tilePosition = new Vector2Int(x, y);
                SpawnTile(tilePosition);
            }
        }
    }

    private void UpdateMap()
    {
        // Lấy vị trí hiện tại của camera
        Vector2Int currentCameraPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x / tileSize), Mathf.RoundToInt(transform.position.y / tileSize));

        // Kiểm tra xem camera đã di chuyển hay chưa
        if (currentCameraPosition != previousCameraPosition)
        {
            // Xóa các tile cũ xa hơn một khoảng viewDistance so với camera
            foreach (Vector2Int oldTilePosition in oldTilePositions)
            {
                if (!IsPositionWithinViewDistance(oldTilePosition, currentCameraPosition))
                {
                    DestroyTile(oldTilePosition);
                }
            }

            // Xóa các vật cản cũ xa hơn một khoảng viewDistance so với camera
            foreach (Vector2Int oldObstaclePosition in oldObstaclePositions)
            {
                if (!IsPositionWithinViewDistance(oldObstaclePosition, currentCameraPosition))
                {
                    DestroyObstacle(oldObstaclePosition);
                }
            }

            // Camera đã di chuyển, cập nhật map và vật cản trong vùng viewDistance
            for (int x = currentCameraPosition.x - viewDistance; x <= currentCameraPosition.x + viewDistance; x++)
            {
                for (int y = currentCameraPosition.y - viewDistance; y <= currentCameraPosition.y + viewDistance; y++)
                {
                    Vector2Int tilePosition = new Vector2Int(x, y);
                    if (!tiles.ContainsKey(tilePosition))
                    {
                        SpawnTile(tilePosition);
                    }
                    if (!obstacles.ContainsKey(tilePosition) && IsPositionWithinViewDistance(tilePosition, currentCameraPosition))
                    {
                        SpawnObstacle(tilePosition);
                    }
                }
            }

            // Cập nhật vị trí trước đó của camera và vị trí các tile và vật cản cũ
            previousCameraPosition = currentCameraPosition;
            UpdateOldTilePositions();
            UpdateOldObstaclePositions();
        }
    }

    private bool IsPositionWithinViewDistance(Vector2Int position, Vector2Int currentCameraPosition)
    {
        int distanceX = Mathf.Abs(position.x - currentCameraPosition.x);
        int distanceY = Mathf.Abs(position.y - currentCameraPosition.y);
        return distanceX <= viewDistance && distanceY <= viewDistance;
    }

    private void DestroyTile(Vector2Int position)
    {
        if (tiles.TryGetValue(position, out GameObject tile))
        {
            Destroy(tile);
            tiles.Remove(position);
        }
    }

    private void DestroyObstacle(Vector2Int position)
    {
        if (obstacles.TryGetValue(position, out GameObject obstacle))
        {
            Destroy(obstacle);
            obstacles.Remove(position);
        }
    }

    private void UpdateOldTilePositions()
    {
        oldTilePositions.Clear();
        foreach (Vector2Int tilePosition in tiles.Keys)
        {
            oldTilePositions.Add(tilePosition);
        }
    }

    private void UpdateOldObstaclePositions()
    {
        oldObstaclePositions.Clear();
        foreach (Vector2Int obstaclePosition in obstacles.Keys)
        {
            oldObstaclePositions.Add(obstaclePosition);
        }
    }

    private void SpawnTile(Vector2Int position)
    {
        Vector3 spawnPosition = new Vector3(position.x * tileSize, position.y * tileSize, -1f); // Đặt giá trị z là -1f
        GameObject newTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity);
        newTile.transform.parent = parentContainer.transform; // Đặt mục cha cho mục con
        tiles.Add(position, newTile);
    }

    private void SpawnObstacle(Vector2Int position)
    {
        float randomDistance = Random.Range(minDistance, maxDistance);
        float obstacleSpacing = randomDistance * tileSize; // Khoảng cách giữa các vật cản
        Vector3 spawnPosition = new Vector3(position.x * tileSize, position.y * tileSize, 0f);

        // Kiểm tra khoảng cách với các vật cản đã tồn tại
        bool isObstacleTooClose = IsObstacleTooClose(position, obstacleSpacing);
        if (isObstacleTooClose)
        {
            return; // Không sinh ra vật cản mới
        }

        // Xác suất sinh ra vật cản
        float obstacleProbability = Random.Range(0f, 1f);
        float obstacleSpawnChance = 0.8f; // Xác suất sinh ra vật cản (vd: 20%)

        if (obstacleProbability <= obstacleSpawnChance)
        {
            int randomIndex = Random.Range(0, obstaclePrefabs.Length);
            GameObject obstaclePrefab = obstaclePrefabs[randomIndex];
            GameObject newObstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
            newObstacle.transform.parent = spawnedObjectsGroup.transform; // Thêm vật cản vào trong GameObject Group
            obstacles.Add(position, newObstacle);
        }
    }

    private bool IsObstacleTooClose(Vector2Int position, float obstacleSpacing)
    {
        foreach (Vector2Int obstaclePosition in obstacles.Keys)
        {
            float distanceX = Mathf.Abs(position.x - obstaclePosition.x);
            float distanceY = Mathf.Abs(position.y - obstaclePosition.y);
            float obstacleDistance = Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY) * tileSize;

            if (obstacleDistance < obstacleSpacing)
            {
                return true; // Có vật cản quá gần
            }
        }

        return false; // Không có vật cản quá gần
    }
}
