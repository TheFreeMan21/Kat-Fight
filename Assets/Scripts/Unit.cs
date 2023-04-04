using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour//this script defines the properties that a player or enemy prefab can have, after that each prefab stores the individual stats.
{
    public string unitName;
    public int unitLevel;

    public int damage;
    public int ultimateDamage;
    public int maxUltimate;
    
    public int maxHP;
    public int currentHP;

    public int defense;

    public bool TakeDamage(int dmg) // This function updates the current health when damage is taken
    {
        dmg -= defense;
        if (dmg <= 0) return false;
        currentHP -= dmg;

        if (currentHP <= 0)
            return true;
        else
            return false;
    }

    public void Heal(int amount) //This one updates the health when someone heals.
    {
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }

    public void Defense(int amount) {// This updates the defense stat when a state change is done.
        defense += amount;
    }
}
