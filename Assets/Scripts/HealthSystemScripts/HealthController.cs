using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour {
  public int PlayerHealth;

  [SerializeField] private Image[] hearts;
  [SerializeField] private Sprite EmptyHeart;
  [SerializeField] private Sprite FullHeart;

  private void Start() {
    UpdateHealth();
  }

  public void UpdateHealth() {
    if (PlayerHealth <= 0) {
      // Player has died. Return to Main Menu
    }

    for (int i = 0; i < hearts.Length; i++) {
      if (i < PlayerHealth) hearts[i].sprite = FullHeart;
      else hearts[i].sprite = EmptyHeart;
    }
  }
}

