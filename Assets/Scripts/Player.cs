using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public int level;
    public float health;
    public int score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SavePlayer()
    {
        SaveData data = new SaveData();
        data.level = level;
        data.health = health;
        data.score = score;
        data.position = new float[2] { transform.position.x, transform.position.y };

        DataManager.instance.SaveData(data);
        DataManager.instance.isSaveGame = true;
    }

    public void LoadPlayer()
    {
        SaveData data = DataManager.instance.LoadData();
        if (data != null)
        {
            level = data.level;
            health = data.health;
            score = data.score;

            Vector3 position = new Vector3(data.position[0], data.position[1], 0f);
            transform.position = position;

            DataManager.instance.isLoadGame = true;
        }
    }
}
