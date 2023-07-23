using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreezeSkill : MonoBehaviour
{
    public static FreezeSkill Instance { get; private set; }
    [Header("FreezeSkill")]
    public Image freezeSkill;
    public float cooldownFreeze = 30f;
    public bool isCooldownFreeze = false;
    public bool isLockSkillFreeze;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        freezeSkill.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        UseFreezeSkill();
    }

    public void UseFreezeSkill()
    {
        if (isCooldownFreeze)
        {
            freezeSkill.fillAmount -= 1 / cooldownFreeze * Time.deltaTime;
            if (freezeSkill.fillAmount <= 0)
            {
                freezeSkill.fillAmount = 1;
                isCooldownFreeze = false;
                GameObject.Find("Freeze").GetComponent<Button>().interactable = true;
            }
        }
    }
}
