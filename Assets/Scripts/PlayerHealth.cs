using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Sse4_2;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }
    private float health = 0f;
    private int maxSpecial = 3;
    private int special = 0;
    private bool isCharged = false;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider specialSlider;
    [SerializeField] public GameObject gamover;
    private bool shiel = false;
    public GameObject shied;
    [SerializeField] public AudioSource audioSource;
    [SerializeField]
    TextMeshProUGUI healthText;
    [SerializeField]
    TextMeshProUGUI specialText;
    private void Start()
    {
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        specialSlider.maxValue = maxSpecial;
        gamover.SetActive(false);
        SoundManager.Instance.AddToAudio(audioSource);
    }

    public void Shield()
    {
        if (!shiel)
        {
            SoundManager.Instance.Play(audioSource);
            shied.SetActive(true);
            shiel = true;
            Invoke("Noshield", 3f);
        }
        if (Skill4.Instance != null)
            Skill4.Instance.isCooldown4 = !Skill4.Instance.isCooldown4;
        GameObject.Find("Shield").GetComponent<Button>().interactable = false;
    }
    public void Noshield()
    {
        shied.SetActive(false);
        shiel = false;
    }
    public int getSpecialValue()
    {

        return special;

    }
    public bool getIsCharge()
    {

        return this.isCharged;

    }
    public void setIsCharge(bool charge)
    {

        this.isCharged = charge;

    }
    public void setSpecialValue(int special)
    {

        this.special = special;
        specialSlider.value = special;
        specialText.text = getSpecialValue() + "/" + getMaxSpecialValue();
        if (special == 3)
        {
            setIsCharge(true);
        }
        else
        {
            setIsCharge(false);
        }

    }
    public int getMaxSpecialValue()
    {

        return maxSpecial;

    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public float GetHealth()
    {
        return health;
    }
    public void UpdateHealth(float mod)
    {
        if (shiel == false)
        {
            health += mod;

            if (health > maxHealth)
            {
                health = maxHealth;
            }
            else if (health <= 0f)
            {
                health = 0f;
                healthSlider.value = health;
                Destroy(gameObject);
                gamover.SetActive(true);
                Skill4.Instance.skillImage4.fillAmount = 0;
            }
        }
        healthText.text = GetHealth() + "/" + GetMaxHealth();
    }

    public void addSpecial()
    {
        if (this.special < 3)
        {
            setSpecialValue(this.special += 1);
        }
        else
        {
            //Process
            setSpecialValue(3);
        }

    }

    public void AddHealth(float x)
    {
        this.maxHealth += x;
        this.health += x;
        healthSlider.maxValue = this.maxHealth;
        healthText.text = GetHealth() + "/" + GetMaxHealth();
    }

    private void OnGUI()
    {
        float t = Time.deltaTime / 1f;
        healthSlider.value = Mathf.Lerp(healthSlider.value, health, t);

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pill"))
        {
            Destroy(other.gameObject);
            addSpecial();
        }
    }
}
