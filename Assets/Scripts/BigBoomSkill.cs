using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BigBoomSkill : MonoBehaviour
{
    public static BigBoomSkill Instance { get; private set; }

    [Header("BigBoomSkill")]
    public Image bigBoomSkill;
    public bool isLockSkillBigBoom;
    public PlayerHealth playerHealth;

    private void Awake()
    {
        // Đảm bảo chỉ có một thể hiện duy nhất của lớp BigBoomSkill
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        this.isLockSkillBigBoom = true;
        playerHealth = FindObjectOfType<PlayerHealth>();
        bigBoomSkill.fillAmount = 0;
        GameObject.Find("BigBoom").GetComponent<Button>().interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth != null && playerHealth.getSpecialValue() == 3)
        {
            this.isLockSkillBigBoom = false;
            bigBoomSkill.fillAmount = 1;
            GameObject.Find("BigBoom").GetComponent<Button>().interactable = true;
        }
    }
}
