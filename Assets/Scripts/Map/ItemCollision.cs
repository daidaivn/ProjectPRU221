using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollision : MonoBehaviour
{
    public string playerTag = "Player"; // Tag của nhân vật người chơi
    public string boomTag = "Boom"; // Tag của vật phẩm "Boom"
    public string enemyTag = "Enemy"; // Tag của các đối tượng Enemy

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu vật phẩm va chạm với đối tượng có tag là "Player"
        if (collision.CompareTag(playerTag))
        {
            // Xóa tất cả các đối tượng có tag "Enemy" trên màn hình
            DeleteAllEnemies();
        }
        // Kiểm tra nếu vật phẩm có tag là "Boom" va chạm với đối tượng có tag là "Player"
        else if (collision.CompareTag(boomTag))
        {
            // Xóa tất cả các đối tượng có tag "Enemy" trên màn hình
            DeleteAllEnemies();
            // Xóa chính vật phẩm có tag "Boom"
            Destroy(gameObject);
        }
    }

    private void DeleteAllEnemies()
    {
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemyObject in enemyObjects)
        {
            if (enemyObject != gameObject)
            {
                Destroy(enemyObject);
            }
        }
    }
}
