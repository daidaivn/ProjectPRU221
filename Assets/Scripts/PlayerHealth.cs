using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float health = 0f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TextMeshProUGUI healthText;

    private bool isShieldActive = false;

    private void Start()
    {
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        gameOverPanel.SetActive(false);
        SoundManager.Instance.AddToAudio(audioSource);
    }

    public void ActivateShield()
    {
        if (!isShieldActive)
        {
            SoundManager.Instance.Play(audioSource);
            shieldObject.SetActive(true);
            isShieldActive = true;
            StartCoroutine(DeactivateShieldDelayed(3f));
        }

        if (Skill4.Instance != null)
        {
            Skill4.Instance.isCooldown4 = !Skill4.Instance.isCooldown4;
        }

        GameObject shieldButton = GameObject.Find("Shield");
        if (shieldButton != null)
        {
            shieldButton.GetComponent<Button>().interactable = false;
        }
    }

    private IEnumerator DeactivateShieldDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        DeactivateShield();
    }

    private void DeactivateShield()
    {
        shieldObject.SetActive(false);
        isShieldActive = false;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealth()
    {
        return health;
    }

    public void UpdateHealth(float modifier)
    {
        if (!isShieldActive)
        {
            health += modifier;

            if (health > maxHealth)
            {
                health = maxHealth;
            }
            else if (health <= 0f)
            {
                health = 0f;
                healthSlider.value = health;
                Destroy(gameObject);
                gameOverPanel.SetActive(true);
                Skill4.Instance.skillImage4.fillAmount = 0;
            }
        }

        healthText.text = health.ToString() + "/" + maxHealth.ToString();
    }

    public void AddHealth(float value)
    {
        maxHealth += value;
        health += value;
        healthSlider.maxValue = maxHealth;
        healthText.text = health.ToString() + "/" + maxHealth.ToString();
    }

    private void Update()
    {
        float t = Time.deltaTime / 1f;
        healthSlider.value = Mathf.Lerp(healthSlider.value, health, t);
    }
}
