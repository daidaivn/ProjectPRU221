using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CameraRunController : MonoBehaviour
{
    public GameObject character; // Biến để lưu trữ GameObject nhân vật
    public float cameraSpeed = 5f; // Tốc độ di chuyển của camera

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        // Di chuyển camera theo vị trí của nhân vật
        transform.position = new Vector3(character.transform.position.x, character.transform.position.y, transform.position.z);

        // Di chuyển camera khi nhấn nút di chuyển
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, verticalInput, 0f);
        transform.position += moveDirection * cameraSpeed * Time.deltaTime;
    }

}
