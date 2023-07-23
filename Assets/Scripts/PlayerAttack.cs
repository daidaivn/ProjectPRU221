using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Interface cho các trạng thái bắn tên cung
public interface IShootState
{
    void FireBow(PlayerAttack playerAttack);
}

// Trạng thái bình thường của việc bắn tên cung
public class NormalShootState : IShootState
{
    public void FireBow(PlayerAttack playerAttack)
    {
        playerAttack.FireBow(0.5f, 0.5f);
    }
}

// Trạng thái bắn nhanh của việc bắn tên cung
public class RapidShootState : IShootState
{
    public void FireBow(PlayerAttack playerAttack)
    {
        playerAttack.FireBow(0.5f, 0.2f);
    }
}

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject ArrowPrefab;
    [SerializeField] SpriteRenderer ArrowGFX;
    [SerializeField] Transform Bow;
    public GameObject player;

    [SerializeField] float BowPower;

    [SerializeField] public AudioSource arrow;
    public Transform randomMap;

    float BowCharge;
    bool CanFire = true;
    float nextFire = 0;
    float fireRate;
    bool isRapid = false;

    private IShootState currentShootState;

    private void Awake()
    {
        SoundManager.Instance.AddToAudio(arrow);
    }

    private void Start()
    {
        // Khởi tạo trạng thái ban đầu là bình thường
        currentShootState = new NormalShootState();
    }

    private void Update()
    {
        if (!isRapid)
        {
            this.fireRate = Time.time + 0.5f;
        }
        else
        {
            this.fireRate = Time.time + 0.2f;
        }

        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        if (allEnemies != null && Time.time > nextFire)
        {
            Debug.Log("FireRate = " + (this.fireRate - Time.time));

            // Thực hiện bắn tên cung dựa vào trạng thái hiện tại
            currentShootState.FireBow(this);

            nextFire = fireRate;
        }
    }

    public void RapidFire()
    {
        if (!isRapid)
        {
            isRapid = true;
            StartCoroutine(RapidCoroutine());
        }
    }

    private IEnumerator RapidCoroutine()
    {
        yield return new WaitForSeconds(5f);
        isRapid = false;
    }

    // Hành vi bắn tên cung, dựa vào trạng thái hiện tại
    public void FireBow(float arrowSpeed, float arrowDamage)
    {
        ArrowGFX.enabled = true;
        BowCharge += Time.deltaTime;

        float ArrowSpeed = arrowSpeed + BowPower;
        float ArrowDamage = arrowDamage * BowPower;

        Enemy closestEnemy = FindClosestEnemy();
        if (closestEnemy != null)
        {
            Vector3 vectorToTarget = player.transform.position - closestEnemy.transform.position;

            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90f;
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

            Arrow Arrow = Instantiate(ArrowPrefab, Bow.position, rot).GetComponent<Arrow>();
            Arrow.transform.SetParent(randomMap);

            Arrow.ArrowVelocity = ArrowSpeed;
            Arrow.ArrowDamage = ArrowDamage;

            SoundManager.Instance.Play(arrow);
            CanFire = false;
            ArrowGFX.enabled = false;
        }
    }

    public void LevelUp()
    {
        this.BowPower *= 1.1f;
    }

    Enemy FindClosestEnemy()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        Enemy closestEnemy = null;
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (allEnemies != null)
        {
            foreach (Enemy currentEnemy in allEnemies)
            {
                float distanceToEnemy = (currentEnemy.transform.position - player.transform.position).sqrMagnitude;
                if (distanceToEnemy < distanceToClosestEnemy)
                {
                    distanceToClosestEnemy = distanceToEnemy;
                    closestEnemy = currentEnemy;
                }
            }
        }
        return closestEnemy;
    }
}
