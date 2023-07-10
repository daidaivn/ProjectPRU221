using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour
{
    public GameObject obstaclePrefab; // Prefab của vật cản
    public Transform character; // Transform của nhân vật
    public float cameraFollowSpeed = 2f; // Tốc độ di chuyển camera theo nhân vật
    public float minDistance = 2f; // Khoảng cách tối thiểu giữa vật cản và nhân vật
    public float limitDistance = 10f; // Khoảng cách giới hạn để sinh ra vật cản
    public int maxObstacles = 5; // Số lượng vật cản tối đa

    private List<GameObject> obstacles; // Danh sách chứa các vật cản đã sinh ra

    private void Start()
    {
        obstacles = new List<GameObject>();
    }

    private void Update()
    {
        MoveCamera();
        SpawnObstacle();
    }

    private void MoveCamera()
    {
        Vector3 targetPosition = new Vector3(character.position.x, character.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, cameraFollowSpeed * Time.deltaTime);
    }

    private void SpawnObstacle()
    {
        // Kiểm tra và xóa các vật cản hiện tại nếu limitDistance thay đổi hoặc vượt quá số lượng tối đa
        for (int i = obstacles.Count - 1; i >= 0; i--)
        {
            GameObject obstacle = obstacles[i];
            if (Vector3.Distance(obstacle.transform.position, character.position) > limitDistance || obstacles.Count > maxObstacles)
            {
                obstacles.RemoveAt(i);
                Destroy(obstacle);
            }
        }

        // Kiểm tra khoảng cách giữa camera và nhân vật
        if (Vector3.Distance(transform.position, character.position) > limitDistance || obstacles.Count > maxObstacles)
        {
            return; // Vượt quá khoảng cách giới hạn hoặc số lượng vật cản tối đa, không sinh ra vật cản
        }

        Vector3 spawnPosition = GetRandomPosition();
        GameObject newObstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);

        float distance = Vector3.Distance(newObstacle.transform.position, character.position);
        if (distance < minDistance)
        {
            Vector3 safePosition = character.position + (newObstacle.transform.position - character.position).normalized * minDistance;
            newObstacle.transform.position = safePosition;
        }

        obstacles.Add(newObstacle);
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomPosition = Random.insideUnitCircle * limitDistance;
        randomPosition += character.position;
        randomPosition.z = 0f;

        return randomPosition;
    }
}
