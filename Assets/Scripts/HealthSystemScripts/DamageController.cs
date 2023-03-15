using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour {
  [SerializeField] private int dmg;
  [SerializeField] private HealthController _healthController;

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.CompareTag("Player")) Damage();
  }

  private void Damage() {
    _healthController.PlayerHealth = _healthController.PlayerHealth - dmg;
    _healthController.UpdateHealth();
    gameObject.SetActive(false);
  }
}
