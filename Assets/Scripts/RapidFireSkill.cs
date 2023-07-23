using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RapidFireSkill : MonoBehaviour
{
    public static RapidFireSkill Instance { get; private set; }
    [Header("RapidFireSkill")]
    public Image rapidFireSkill;
    public float cooldownRapid = 30f;
    public bool isCooldownRapid = false;
    public bool isLockSkillRapid;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        rapidFireSkill.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        UseRapidSkill();
    }

    public void UseRapidSkill()
    {
        if (isCooldownRapid)
        {
            rapidFireSkill.fillAmount -= 1 / cooldownRapid * Time.deltaTime;
            if (rapidFireSkill.fillAmount <= 0)
            {
                rapidFireSkill.fillAmount = 1;
                isCooldownRapid = false;
                GameObject.Find("Rapid").GetComponent<Button>().interactable = true;
            }
        }
    }
}
