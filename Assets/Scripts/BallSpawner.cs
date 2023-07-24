using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using Assets.Scripts;

public class BallSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject prefabBall;
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI level;
    public Transform randomMap;
    Timer spawnTimer;

    int minSpawnX;
    int minSpawnY;
    int maxSpawnX;
    int maxSpawnY;

    private BallFactory ballFactory; // Thêm biến ballFactory để sử dụng BallFactory

    void Start()
    {
        var width = Screen.width - 100;
        var height = Screen.height - 100;
        minSpawnX = 0;
        maxSpawnX = Screen.width;
        minSpawnY = 0;
        maxSpawnY = Screen.height;

        // Khởi tạo BallFactory với prefabBall được sử dụng trong BallSpawner
        ballFactory = new BallFactory(prefabBall);

        spawnTimer = gameObject.AddComponent<Timer>();
        spawnTimer.Duration = 5;
        spawnTimer.Run();
    }

    void Update()
    {
        if (spawnTimer.Finished)
        {
            SpawnBear();

            spawnTimer.Duration = 0.5f;
            spawnTimer.Run();
        }
    }

    bool check = false;
    public void SpawnBear()
    {
        int score = Convert.ToInt32(Regex.Replace("0" + scoreText.text, "\\D+", ""));
        Vector3 Location = new Vector3(UnityEngine.Random.Range(minSpawnX, maxSpawnX), UnityEngine.Random.Range(minSpawnY, maxSpawnY), -Camera.main.transform.position.z);
        Vector3 worldLocation = Camera.main.ScreenToWorldPoint(Location);

        GameObject ball = ballFactory.CreateBall(randomMap, worldLocation);
        ball.GetComponent<Enemy>().HealthLevel();

        // Kiểm tra điều kiện để sử dụng biến ilevel
        if (score >= 20 && score % 5 == 4)
        {
            check = true;
        }

        // Sử dụng biến ilevel sau khi kiểm tra điều kiện
        int ilevel = Convert.ToInt32(Regex.Replace("0" + level.text, "\\D+", ""));
        if (score / 20f > ilevel)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().AddHealth(100f);
            level.text = "Level: " + (ilevel + 1);
            var gobs = GameObject.FindGameObjectsWithTag("Enemy");
            var x = gobs[gobs.Length - 1];
            x.GetComponent<Enemy>().Upgrade(score);
            x.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().AddSpeed();
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().LevelUp();
        }
    }

}