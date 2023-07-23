using UnityEngine;
using UnityEngine.UI;

public class Skill2 : MonoBehaviour
{
    // Định nghĩa thuộc tính tĩnh Instance
    public static Skill2 Instance { get; private set; }

    // Các thuộc tính khác của lớp Skill2
    [SerializeField]
    int numberOfProjectiles;

    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject projectile;

    float radius, moveSpeed;

    [Header("Skill2")]
    public Image skillImage2;
    public float cooldown2 = 5f;
    public bool isCooldown2 = false;
    public bool isLockSkill2;

    private void Start()
    {
        // Khởi tạo thuộc tính Instance trong phương thức Start
        Instance = this;

        // Các công việc khác trong phương thức Start
        skillImage2.fillAmount = 1;
    }

    private void Update()
    {
        radius = 5f;
        moveSpeed = 5f;
        Skill_2();
    }

    public void Skill_2()
    {
        if (isCooldown2)
        {
            skillImage2.fillAmount -= 1 / cooldown2 * Time.deltaTime;
            if (skillImage2.fillAmount <= 0)
            {
                skillImage2.fillAmount = 1;
                isCooldown2 = false;
                GameObject.Find("3arrows").GetComponent<Button>().interactable = true;
            }
        }
    }
}
