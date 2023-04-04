using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour //This script is used to update the Battle hud, like the HP and Ultimate bars, and the names and levels 
{

    public Text nameText;
    public Text lvlText;
    public Slider hpSlider;
    public Slider ultimateSlider;

    public void SetHud(Unit unit)
    {
        nameText.text = unit.unitName;
        lvlText.text = "LVL " + unit.unitLevel;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
        ultimateSlider.maxValue = unit.maxUltimate;
        ultimateSlider.value = 0;
    }

    public void SetLVL(int amount)
    {
        lvlText.text = "LVL " + amount;
    }

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }

    public void SetUltimate(int ultimate)
    {
        ultimateSlider.value = ultimate;
    }
}
