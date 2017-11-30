﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterWeaponManager : MonoBehaviour
{

    /// <summary>
    /// static value holder for different weapon types
    /// </summary>

    static public int arrowDamage = 15;
    static public int axeDamage = 30;

    //the bomb itself has a little damage. But if exploded at the right time, gives more damage.
    static public int bombDamage = 20;
    static public int bombExplosionDamage = 120;
    static public float bombExplosionRadius = 3;

    static public int grenadeDamage = 35;
    static public int swordDamage = 50;

    public static int GetDamage(int baseDamage, EnemyController.enemySkillLevels skills)
    {
        switch (skills)
        {
            case EnemyController.enemySkillLevels.easy:
                return baseDamage;
                break;
            case EnemyController.enemySkillLevels.normal:
                return baseDamage * 2;
                break;
            case EnemyController.enemySkillLevels.hard:
                return baseDamage * 3;
                break;
            case EnemyController.enemySkillLevels.Robinhood:
                return baseDamage * 3;
                break;
            default:
                return baseDamage;
                break;
        }

        return 10;
    }

}
