using System.Collections;
using UnityEngine;

public class RandomPillSpawn : MonoBehaviour
{
    public GameObject objectPrefab;
    public Transform playerTransform;
    public float maxDistance = 8f;
    public float minDistance = 3f;
    public float objectInterval = 20f;
    public float maxDistanceFromPlayer = 30f;
    private bool isGeneratingObject = false;
    private float timeSinceLastSpawn = 0f;
    // Biến để lưu trữ đối tượng "RandomMap"
    public GameObject randomMap;

    // Define the variables minSpawnX, maxSpawnX, minSpawnY, and maxSpawnY directly here
    private int minSpawnX;
    private int minSpawnY;
    private int maxSpawnX;
    private int maxSpawnY;

    // Define the spawnTimer variable
    private Timer spawnTimer;
    private bool isGameOver = false;
    private void Start()
    {
        var width = Screen.width - 100;
        var height = Screen.height - 100;
        minSpawnX = 0;
        maxSpawnX = Screen.width;
        minSpawnY = 0;
        maxSpawnY = Screen.height;

        // Tìm và lưu trữ đối tượng "RandomMap"
        randomMap = GameObject.Find("RandomMap");

        // Hoặc gán trực tiếp thông qua giao diện Inspector
        // randomMap = yourRandomMapGameObject;

        // Initialize the spawnTimer variable
        spawnTimer = gameObject.AddComponent<Timer>();
        spawnTimer.Duration = 5;
        spawnTimer.Run();
    }

    private void Update()
    {
        // Kiểm tra nếu Canvas "GameOver" không được hiển thị, thì mới thực hiện spawn đối tượng
        if (!IsGameOverCanvasActive())
        {
            if (timeSinceLastSpawn >= objectInterval && !isGeneratingObject)
            {
                GenerateObject();
            }

            timeSinceLastSpawn += Time.deltaTime;
            DestroyObjectsFarFromPlayer();
        }
    }

    private void GenerateObject()
    {
        if (isGeneratingObject)
        {
            return;
        }

        isGeneratingObject = true;
        timeSinceLastSpawn = 0f;

        if (playerTransform != null)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            float randomDistance = Random.Range(minDistance, maxDistance);
            Vector2 randomPosition = (Vector2)playerTransform.position + randomDirection * randomDistance;

            float screenXMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
            float screenXMax = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
            float screenYMin = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
            float screenYMax = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;
            randomPosition.x = Mathf.Clamp(randomPosition.x, screenXMin, screenXMax);
            randomPosition.y = Mathf.Clamp(randomPosition.y, screenYMin, screenYMax);

            // Sinh ra đối tượng mới và gán "RandomMap" làm cha
            GameObject newObject = Instantiate(objectPrefab, randomPosition, Quaternion.identity);
            newObject.transform.SetParent(randomMap.transform);

            Debug.Log("Object generated at position: " + randomPosition);
        }
        else
        {
            Debug.LogWarning("PlayerTransform is null. Cannot generate object.");
        }

        isGeneratingObject = false;
    }
    bool IsGameOverCanvasActive()
    {
        GameObject canvasGameOver = GameObject.Find("GameOver");
        if (canvasGameOver != null && canvasGameOver.activeInHierarchy)
        {
            isGameOver = true;
            return true;
        }
        else
        {
            isGameOver = false;
            return false;
        }
    }
    private void DestroyObjectsFarFromPlayer()
    {
        GameObject[] spawnedObjects = GameObject.FindGameObjectsWithTag("Pill");

        foreach (GameObject spawnedObject in spawnedObjects)
        {
            try
            {
                if (Vector2.Distance(playerTransform.position, spawnedObject.transform.position) > maxDistanceFromPlayer)
                {
                    Destroy(spawnedObject);
                    Debug.Log("Object destroyed: " + spawnedObject.transform.position);
                }
            }
            catch (System.Exception)
            {
                Debug.Log("Game Over");
            }

        }
    }
}
