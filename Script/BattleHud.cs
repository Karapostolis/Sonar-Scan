using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BattleHud : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public Slider hpSlider;

    public void SetHUD(Player unit)
    {
        nameText.text = unit.playerName;
        levelText.text = "Lvl " + unit.Level;
        hpSlider.maxValue = unit.MaxHealthPoints;
        hpSlider.value = unit.HealthPoints;
    }

    public void SetHUD(NonPlayer unit)
    {
        nameText.text = unit.enemyName;
        levelText.text = "Lvl " + unit.Level;
        hpSlider.maxValue = unit.MaxHealthPoints;
        hpSlider.value = unit.HealthPoints;
    }

    public void SetHp(int hp)
    {
        hpSlider.value = hp;
    }
}
