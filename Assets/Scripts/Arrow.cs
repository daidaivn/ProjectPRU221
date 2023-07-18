using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {
    [HideInInspector] public float ArrowVelocity;
	public float ArrowDamage;
	[SerializeField] Rigidbody2D rb;

	private void Start() {
		Destroy(gameObject, 4f);
	}
    private void OnTriggerEnter2D(Collider2D other)
    {
		if (other.gameObject.tag == "Enemy")
		{
			Debug.Log("Enemy Attacked");
			Enemy enemy = other.gameObject.GetComponent<Enemy>();
			enemy.TakeDamage(ArrowDamage);
		}
		Destroy(gameObject);
	}
    private void FixedUpdate() {
		rb.velocity = transform.up * ArrowVelocity;
	}

	
}
