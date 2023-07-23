using System.Collections;
using UnityEngine;

public class RandomPillSpawn : MonoBehaviour
{
    public GameObject objectPrefab;
    public Transform playerTransform;
    public float maxDistance = 8f; // Khoảng cách tối đa từ người chơi đến đối tượng mới
    public float minDistance = 3f; // Khoảng cách tối thiểu từ người chơi đến đối tượng mới
    public float objectInterval = 20f; // Khoảng thời gian giữa việc sinh ra các đối tượng mới
    public float maxDistanceFromPlayer = 30f; // Khoảng cách tối đa từ vật thể đến nhân vật trước khi bị xóa
    private bool isGeneratingObject = false; // Biến đánh dấu xem có thể sinh ra đối tượng mới hay không
    private float timeSinceLastSpawn = 0f; // Biến đếm thời gian kể từ lần sinh ra đối tượng mới cuối cùng

    private void Update()
    {
        // Kiểm tra nếu đã đủ thời gian để sinh ra vật thể mới
        if (timeSinceLastSpawn >= objectInterval && !isGeneratingObject)
        {
            GenerateObject();
        }

        // Cập nhật thời gian kể từ lần sinh ra đối tượng mới cuối cùng
        timeSinceLastSpawn += Time.deltaTime;

        // Xóa các vật thể xa quá người chơi
        DestroyObjectsFarFromPlayer();
    }

    private void GenerateObject()
    {
        if (isGeneratingObject)
        {
            return;
        }

        isGeneratingObject = true;
        timeSinceLastSpawn = 0f;

        // Tạo một hướng ngẫu nhiên từ người chơi
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        // Nhân với khoảng cách ngẫu nhiên từ minDistance đến maxDistance
        float randomDistance = Random.Range(minDistance, maxDistance);
        // Tính toán vị trí mới dựa vào hướng và khoảng cách ngẫu nhiên
        Vector2 randomPosition = (Vector2)playerTransform.position + randomDirection * randomDistance;

        // Giới hạn vị trí ngẫu nhiên trong khoảng vùng Camera
        float screenXMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        float screenXMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        float screenYMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        float screenYMax = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
        randomPosition.x = Mathf.Clamp(randomPosition.x, screenXMin, screenXMax);
        randomPosition.y = Mathf.Clamp(randomPosition.y, screenYMin, screenYMax);

        GameObject newObject = Instantiate(objectPrefab, randomPosition, Quaternion.identity);
        newObject.tag = "Pill"; // Gán tag "SpawnedObject" cho vật thể mới sinh ra
        Debug.Log("Object generated at position: " + randomPosition);

        isGeneratingObject = false;
    }

    private void DestroyObjectsFarFromPlayer()
    {
        GameObject[] spawnedObjects = GameObject.FindGameObjectsWithTag("SpawnedObject");

        foreach (GameObject spawnedObject in spawnedObjects)
        {
            if (Vector2.Distance(playerTransform.position, spawnedObject.transform.position) > maxDistanceFromPlayer)
            {
                Destroy(spawnedObject);
                Debug.Log("Object destroyed: " + spawnedObject.transform.position);
            }
        }
    }
}
