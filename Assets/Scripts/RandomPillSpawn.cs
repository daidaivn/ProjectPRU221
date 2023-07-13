using System.Collections;
using UnityEngine;

public class RandomPillSpawn : MonoBehaviour
{
    public GameObject objectPrefab;
    public Transform playerTransform;

    private void Start()
    {
        StartCoroutine(GenerateObjects());
    }

    private IEnumerator GenerateObjects()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            GenerateObject();
        }
    }

    private void GenerateObject()
    {
        Vector2 playerPosition = playerTransform.position;

        Vector2 randomPosition = new Vector2(
            Random.Range(playerPosition.x - 10f, playerPosition.x + 10f),
            Random.Range(playerPosition.y - 5f, playerPosition.y + 5f)
        );

        GameObject newObject = Instantiate(objectPrefab, randomPosition, Quaternion.identity);
        Debug.Log("Object generated at position: " + randomPosition);
    }
}
