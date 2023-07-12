
using UnityEngine;
using UnityEngine.UI;

public class SkillSurfing : MonoBehaviour
{
    public static SkillSurfing Instance { get; private set; }
    [SerializeField]
    int numberOfProjectiles;

    [SerializeField]
    GameObject player;

    public float dashSpeed;
    public float dashLength = .5f, dashCooldown = 1f;
    private float dashCounter;
    private float dashCoolCounter;

    [SerializeField]
    GameObject projectile;

    float radius, moveSpeed;

    // Use this for initialization


    [Header("SkillSurfing")]
    public Image skillImage1;
    public float cooldown1 = 5f;
    public bool isCooldown1 = false;
    public bool isLockSkill1;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        skillImage1.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        radius = 5f;
        moveSpeed = 5f;
    }
}
