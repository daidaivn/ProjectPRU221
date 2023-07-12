using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] SpriteRenderer arrowGFX;
    [SerializeField] Transform bow;
    [SerializeField] float bowPower;

    [SerializeField] public AudioSource arrow;

    private bool canFire = true;
    private float nextFire = 0;

    private void Awake()
    {
        SoundManager.Instance.AddToAudio(arrow);
    }

    private void Update()
    {
        if (Time.time > nextFire && canFire)
        {
            FireBow();
        }
    }

    private void FireBow()
    {
        arrowGFX.enabled = true;

        float arrowSpeed = 0.5f + bowPower;
        float arrowDamage = 0.5f * bowPower;

        Enemy closestEnemy = FindClosestEnemy();
        if (closestEnemy != null)
        {
            Vector3 direction = closestEnemy.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

            GameObject newArrow = Instantiate(arrowPrefab, bow.position, rotation);
            Arrow arrowScript = newArrow.GetComponent<Arrow>();
            arrowScript.ArrowVelocity = arrowSpeed;
            arrowScript.ArrowDamage = arrowDamage;

            SoundManager.Instance.Play(arrow);

            canFire = false;
            arrowGFX.enabled = false;
            nextFire = Time.time + 0.5f;
        }
    }

    public void LevelUp()
    {
        bowPower *= 1.1f;
    }

    private Enemy FindClosestEnemy()
    {
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();

        float closestDistance = Mathf.Infinity;
        Enemy closestEnemy = null;
        Vector3 playerPosition = transform.position;

        foreach (Enemy enemy in allEnemies)
        {
            float distance = Vector3.Distance(playerPosition, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}
