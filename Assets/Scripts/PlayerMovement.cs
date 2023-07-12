using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Transform hand;
    private float activeMoveSpeed = 5f;
    public GameObject grenade;
    public GameObject player;

    [SerializeField] private GameObject projectile;

    private Vector2 movement;

    public void onClickDash()
    {
        Skill1.Instance.isCooldown1 = !Skill1.Instance.isCooldown1;
        GameObject.Find("Dash").GetComponent<Button>().interactable = false;
        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        float dashSpeed = 10f;
        float dashDuration = 0.5f;
        float dashCooldown = 1f;

        float startTime = Time.time;
        float endTime = startTime + dashDuration;

        activeMoveSpeed = dashSpeed;

        while (Time.time < endTime)
        {
            Movement();
            yield return null;
        }

        activeMoveSpeed = moveSpeed;

        float cooldownStartTime = Time.time;
        float cooldownEndTime = cooldownStartTime + dashCooldown;

        while (Time.time < cooldownEndTime)
        {
            yield return null;
        }

        GameObject.Find("Dash").GetComponent<Button>().interactable = true;
    }

    private void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.H))
        {
            onClickDash();
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            onClickGen();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            onClickMutiple();
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * activeMoveSpeed * Time.fixedDeltaTime);
    }

    private void Movement()
    {
        float mx = Input.GetAxisRaw("Horizontal");
        float my = Input.GetAxisRaw("Vertical");

        movement = new Vector2(mx, my).normalized;
    }

    private void rotateToTarget(Enemy enemy)
    {
        if (player != null && enemy != null)
        {
            Vector3 vectorToTarget = player.transform.position - enemy.transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 180f;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    public void closet()
    {
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player");

        float distanceToClosestEnemy = Mathf.Infinity;
        Enemy closestEnemy = null;

        foreach (Enemy currentEnemy in allEnemies)
        {
            SpriteRenderer enemySpriteRenderer = currentEnemy.transform.GetChild(0).GetComponent<SpriteRenderer>();
            enemySpriteRenderer.color = new Color(1f, 0f, 0.12f, 0f);

            float distanceToEnemy = (currentEnemy.transform.position - player.transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy && distanceToEnemy != 0)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
            }
        }

        if (closestEnemy != null)
        {
            SpriteRenderer closestEnemySpriteRenderer = closestEnemy.transform.GetChild(0).GetComponent<SpriteRenderer>();
            closestEnemySpriteRenderer.color = new Color(1f, 0f, 0.12f, 0.8f);
            rotateToTarget(closestEnemy);
        }
    }

    public void onClickGen()
    {
        Instantiate(grenade, transform.position, Quaternion.identity);
        Skill3.Instance.isCooldown3 = !Skill3.Instance.isCooldown3;
        GameObject.Find("explosion").GetComponent<Button>().interactable = false;
    }

    public void onClickMutiple()
    {
        Skill2.Instance.isCooldown2 = !Skill2.Instance.isCooldown2;
        GameObject.Find("3arrows").GetComponent<Button>().interactable = false;
    }

    public void AddSpeed()
    {
        moveSpeed *= 1.1f;
    }
}
