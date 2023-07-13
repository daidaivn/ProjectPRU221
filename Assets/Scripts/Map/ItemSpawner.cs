using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<GameObject> itemPrefabs; // Danh sách các Prefab của vật phẩm
    public Transform character; // Transform của nhân vật
    public float spawnIntervalMin = 3f; // Thời gian tối thiểu giữa các lần sinh vật phẩm
    public float spawnIntervalMax = 7f; // Thời gian tối đa giữa các lần sinh vật phẩm
    public float minDistance = 2f; // Khoảng cách tối thiểu giữa vật phẩm và nhân vật
    public float maxDistance = 10f; // Khoảng cách tối đa giữa vật phẩm và nhân vật
    public float despawnDistance = 15f; // Khoảng cách thay đổi vị trí vật phẩm

    private float timer; // Đếm thời gian giữa các lần sinh vật phẩm
    private float spawnInterval; // Thời gian giữa các lần sinh vật phẩm hiện tại

    private List<GameObject> spawnedItems; // Danh sách chứa các vật phẩm đã sinh ra

    private void Start()
    {
        spawnedItems = new List<GameObject>();
        GenerateSpawnInterval(); // Tạo thời gian giữa các lần sinh vật phẩm ban đầu
    }

    private void Update()
    {
        // Đếm thời gian
        timer += Time.deltaTime;

        // Kiểm tra nếu đến thời điểm sinh vật phẩm mới
        if (timer >= spawnInterval)
        {
            SpawnItem();
            GenerateSpawnInterval(); // Tạo thời gian giữa các lần sinh vật phẩm mới
            timer = 0f; // Đặt lại đếm thời gian
        }

        // Kiểm tra và xóa các vật phẩm cách xa camera
        for (int i = spawnedItems.Count - 1; i >= 0; i--)
        {
            GameObject item = spawnedItems[i];
            if (Vector3.Distance(item.transform.position, character.position) > despawnDistance)
            {
                // Không xóa vật phẩm mà chỉ đặt lại vị trí
                item.transform.position = GetRandomPosition();
            }
        }
    }

    private void SpawnItem()
    {
        // Chọn ngẫu nhiên một Prefab từ danh sách
        GameObject randomItemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];

        // Tạo vị trí ngẫu nhiên trong khoảng giới hạn
        Vector3 randomPosition = GetRandomPosition();

        // Kiểm tra khoảng cách với nhân vật
        float distance = Vector3.Distance(randomPosition, character.position);
        if (distance < minDistance)
        {
            Vector3 safePosition = character.position + (randomPosition - character.position).normalized * minDistance;
            randomPosition = safePosition;
        }

        // Sinh ra vật phẩm tại vị trí đã chọn
        GameObject newItem = Instantiate(randomItemPrefab, randomPosition, Quaternion.identity);
        spawnedItems.Add(newItem);
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 randomPosition = character.position + randomDirection * Random.Range(minDistance, maxDistance);
        randomPosition.z = 0f;

        return randomPosition;
    }

    private void GenerateSpawnInterval()
    {
        spawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax);
    }
}
