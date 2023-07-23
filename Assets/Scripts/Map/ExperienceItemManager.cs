using UnityEngine;

public class ExperienceItemManager : MonoBehaviour
{
    public GameObject experienceItemPrefab; // Prefab của vật phẩm kinh nghiệm
    public int maxExperienceItems = 5; // Số lượng tối đa của vật phẩm kinh nghiệm có thể tồn tại cùng một lúc

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        CheckExperienceItems();
    }

    private void CheckExperienceItems()
    {
        GameObject[] experienceItems = GameObject.FindGameObjectsWithTag("ExperienceItem");

        // Kiểm tra số lượng vật phẩm kinh nghiệm hiện có
        if (experienceItems.Length < maxExperienceItems)
        {
            // Sinh ra một vật phẩm kinh nghiệm mới
            GenerateExperienceItem();
        }
    }

    private void GenerateExperienceItem()
    {
        // Tạo một thể hiện mới của Prefab vật phẩm kinh nghiệm
        GameObject newExperienceItem = Instantiate(experienceItemPrefab);

        // Đặt vị trí ngẫu nhiên trong phạm vi camera
        float cameraHeight = mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        Vector2 randomPosition = new Vector2(Random.Range(-cameraWidth, cameraWidth), Random.Range(-cameraHeight, cameraHeight));
        newExperienceItem.transform.position = randomPosition;
    }

    private void OnBecameInvisible()
    {
        // Xóa vật phẩm kinh nghiệm khi nó ra khỏi khung nhìn của camera
        Destroy(gameObject);

        // Sinh ra vật phẩm kinh nghiệm mới ngay sau khi xóa
        GenerateExperienceItem();
    }
}
