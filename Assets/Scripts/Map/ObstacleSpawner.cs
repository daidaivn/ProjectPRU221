using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject[] objectPrefabs; // Mảng Prefab của các vật cản
    public Transform playerTransform;
    public float maxDistance = 8f; // Khoảng cách tối đa từ người chơi đến đối tượng mới
    public float minDistance = 3f; // Khoảng cách tối thiểu từ người chơi đến đối tượng mới
    public float maxDistanceFromPlayer = 30f; // Khoảng cách tối đa từ vật thể đến nhân vật trước khi bị xóa
    public int numObjectsToSpawn = 1; // Số lượng vật cản được sinh ra trong mỗi lần cập nhật
    public int maxNumObjects = 10; // Số lượng vật cản tối đa được phép có trong cùng một thời điểm
    private List<GameObject> objectPool = new List<GameObject>(); // Object Pool cho vật cản
    private int numSpawnedObjects = 0; // Biến đếm số lượng vật cản đã được sinh ra

    private void Start()
    {
        InitializeObjectPool();
    }

    private void Update()
    {
        // Xóa các vật thể xa quá người chơi
        DestroyObjectsFarFromPlayer();

        // Tạo mới số lượng vật thể được chỉ định
        int numObjectsToSpawnThisUpdate = Mathf.Min(numObjectsToSpawn, maxNumObjects - numSpawnedObjects);
        for (int i = 0; i < numObjectsToSpawnThisUpdate; i++)
        {
            GenerateObject();
        }
    }

    private void InitializeObjectPool()
    {
        // Khởi tạo Object Pool cho vật cản
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            GameObject newObject = Instantiate(objectPrefabs[i]);
            newObject.SetActive(false);
            objectPool.Add(newObject);
        }
    }

    private void GenerateObject()
    {
        // Tạo một hướng ngẫu nhiên từ người chơi
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        // Nhân với khoảng cách ngẫu nhiên từ minDistance đến maxDistance
        float randomDistance = Random.Range(minDistance, maxDistance);
        // Tính toán vị trí mới dựa vào hướng và khoảng cách ngẫu nhiên
        Vector2 randomPosition = (Vector2)playerTransform.position + randomDirection * randomDistance;

        // Lấy một vật cản từ Object Pool hoặc tạo mới nếu không có sẵn
        GameObject newObject = GetPooledObject();
        if (newObject == null)
        {
            // Tạo một vật cản mới nếu không có sẵn trong Object Pool
            int randomIndex = Random.Range(0, objectPrefabs.Length);
            newObject = Instantiate(objectPrefabs[randomIndex]);
            objectPool.Add(newObject);
        }

        // Đặt lại vị trí và kích hoạt vật cản
        newObject.transform.position = randomPosition;
        newObject.SetActive(true);

        // Tăng biến đếm số lượng vật cản đã sinh ra
        numSpawnedObjects++;
    }

    private void DestroyObjectsFarFromPlayer()
    {
        // Kiểm tra và xóa các vật cản xa quá khoảng cách hiển thị
        for (int i = 0; i < objectPool.Count; i++)
        {
            GameObject obj = objectPool[i];
            if (obj.activeInHierarchy)
            {
                try
                {
                    if (Vector2.Distance(playerTransform.position, obj.transform.position) > maxDistanceFromPlayer)
                    {
                        obj.SetActive(false);
                        // Giảm biến đếm số lượng vật cản đã sinh ra
                        numSpawnedObjects--;
                    }
                }
                catch (System.Exception)
                {
                    Debug.Log("Game Over");
                }
                
            }
        }
    }

    // Hàm để lấy vật cản từ Object Pool
    private GameObject GetPooledObject()
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            {
                return objectPool[i];
            }
        }
        return null;
    }
}
