using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{

    private List<EnemyController> list;

    [Header("Level props")]
    // 期待的玩家完成一局游戏的总时长
    public int ExpectGamingTimePerLvInSec;
    public int ExpectTakingDamagePerLvInPercent;
    public int LvParam;
    public int DamageParam;

    [Header("Enemy Object")]
    public GameObject enemyArcherObject;
    public GameObject enemyBomberObject;

    [Header("Archer respawn area")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;


    private List<Vector3> positions;

    float minEnemyCount, maxEnemyCount;
    List<EnemyController.enemySkillLevels> skillLevels;

    Dictionary<int, EnemyController.enemySkillLevels> levelUnlock;
    Dictionary<EnemyController.enemySkillLevels, string> gameObjectMap;
    Dictionary<int, int[]> enemyCountUnlock;

    public void GenerateEnemyInfoPerLv()
    {
        if (ExpectTakingDamagePerLvInPercent > 0)
        {
            var lvCount = 100 / ExpectTakingDamagePerLvInPercent;
            if (lvCount < 1)
            {
                Debug.Log("ExpectTakingDamagePerLvInPercent must < 100");
                return;
            }
            for (int i = 1; i <= lvCount; i++)
            {
                GenerateEnemyInfoForLv(i);
            }
        }
        else
        {
            Debug.Log("must greater than 0");
        }
    }

    void GenerateEnemyInfoForLv(int lv)
    {
        var needEnemiesCount = ((lv * LvParam) / ExpectGamingTimePerLvInSec) - (DamageParam * ExpectTakingDamagePerLvInPercent);
        Debug.Log("in lv " + lv.ToString() + " need enemies " + needEnemiesCount.ToString());
    }

    void Awake()
    {
        skillLevels = new List<EnemyController.enemySkillLevels>();
        list = new List<EnemyController>();

        levelUnlock = new Dictionary<int, EnemyController.enemySkillLevels>();
        // key: 第几回合, val: 解锁生成难度
        levelUnlock.Add(1, EnemyController.enemySkillLevels.easy);
        levelUnlock.Add(5, EnemyController.enemySkillLevels.normal);
        levelUnlock.Add(10, EnemyController.enemySkillLevels.hard);
        levelUnlock.Add(25, EnemyController.enemySkillLevels.Robinhood);

        // key: 难度, val: 解锁难度对应的gameobject名字
        gameObjectMap = new Dictionary<EnemyController.enemySkillLevels, string>();
        gameObjectMap.Add(EnemyController.enemySkillLevels.easy, "EnemyBody1");
        gameObjectMap.Add(EnemyController.enemySkillLevels.normal, "EnemyBody2");
        gameObjectMap.Add(EnemyController.enemySkillLevels.hard, "EnemyBody3");
        gameObjectMap.Add(EnemyController.enemySkillLevels.Robinhood, "EnemyBody3");

        // key: 第几回合, val: 解锁刷新人数值域
        enemyCountUnlock = new Dictionary<int, int[]>();
        enemyCountUnlock.Add(1, new int[] { 1, 2 });
        enemyCountUnlock.Add(5, new int[] { 2, 3 });
        enemyCountUnlock.Add(8, new int[] { 3, 4 });
        enemyCountUnlock.Add(12, new int[] { 3, 5 });
        enemyCountUnlock.Add(16, new int[] { 3, 6 });
        enemyCountUnlock.Add(21, new int[] { 5, 7 });
        enemyCountUnlock.Add(25, new int[] { 5, 8 });

        minEnemyCount = 1;
        maxEnemyCount = 1;
    }

    //  选中时绘制一个方块
    void OnDrawGizmosSelected()
    {
        var centerX = minX + ((maxX - minX) / 2);
        var centerY = minY + ((maxY - minY) / 2);

        Gizmos.DrawCube(new Vector3(centerX, centerY), new Vector3(maxX - minX, maxY - minY));

    }
    //  绘制一个球
    void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, 0.4f);
    }

    void refreshLevelInfo(int turn)
    {
        if (levelUnlock.ContainsKey(turn))
        {
            skillLevels.Add(levelUnlock[turn]);
        }

        if (enemyCountUnlock.ContainsKey(turn))
        {
            minEnemyCount = enemyCountUnlock[turn][0];
            maxEnemyCount = enemyCountUnlock[turn][1];
        }
    }

    void generatePos()
    {
        var points = Mathf.RoundToInt(UnityEngine.Random.Range(minEnemyCount, maxEnemyCount));

        positions = new List<Vector3>();
        for (int i = 0; i < points; i++)
        {
            float randomX = UnityEngine.Random.Range(minX, maxX);
            float randomY = UnityEngine.Random.Range(minY, maxY);
            positions.Add(new Vector3(randomX, randomY));
        }
    }

    public void KillEnemy(int id)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var e = list[i];
            if (e.enemyId == id)
            {
                list.RemoveAt(i);
                Destroy(e.gameObject, 3f);
                return;
            }
        }
    }

    public bool AllEnemiesDead()
    {
        return list.Count == 0;
    }

    public void ReGenerateEnemies(int turn)
    {
        refreshLevelInfo(turn);

        // for archers
        generatePos();
        for (int i = 0; i < positions.Count; i++)
        {
            var pos = positions[i];
            GameObject ea = Instantiate(enemyArcherObject, pos, Quaternion.Euler(0, 0, 0), gameObject.transform) as GameObject;
            var ctrl = ea.GetComponent<EnemyController>();
            System.Random random = new System.Random();

            EnemyController.enemySkillLevels skillLevel = skillLevels[random.Next(skillLevels.Count)];
            ctrl.InitEnemy(i, skillLevel, gameObjectMap[skillLevel]);
            list.Add(ctrl);
        }

        // for bombers
        GameObject bomber = Instantiate(enemyBomberObject, gameObject.transform.position, Quaternion.Euler(0, 0, 0), gameObject.transform) as GameObject;

    }
}
