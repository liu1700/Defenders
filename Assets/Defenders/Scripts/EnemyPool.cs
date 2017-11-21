using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{

    private List<EnemyController> list;
    public GameObject enemyObject;
    public Vector3 center;
    public float minX, maxX;
    public float minY, maxY;
    private List<Vector3> positions;

    float minEnemyCount, maxEnemyCount;
    List<EnemyController.enemySkillLevels> skillLevels;

    Dictionary<int, EnemyController.enemySkillLevels> levelUnlock;
    Dictionary<EnemyController.enemySkillLevels, string> gameObjectMap;
    Dictionary<int, int[]> enemyCountUnlock;

    void Awake()
    {
        skillLevels = new List<EnemyController.enemySkillLevels>();
        list = new List<EnemyController>();

        levelUnlock = new Dictionary<int, EnemyController.enemySkillLevels>();
        // key: 第几回合, val: 解锁生成难度
        levelUnlock.Add(1, EnemyController.enemySkillLevels.easy);
        levelUnlock.Add(5, EnemyController.enemySkillLevels.normal);
        levelUnlock.Add(20, EnemyController.enemySkillLevels.hard);
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
        enemyCountUnlock.Add(5, new int[] { 1, 3 });
        enemyCountUnlock.Add(8, new int[] { 2, 4 });
        enemyCountUnlock.Add(12, new int[] { 3, 5 });
        enemyCountUnlock.Add(16, new int[] { 3, 6 });
        enemyCountUnlock.Add(21, new int[] { 4, 6 });
        enemyCountUnlock.Add(25, new int[] { 4, 7 });

        minEnemyCount = 1;
        maxEnemyCount = 1;
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

        generatePos();

        for (int i = 0; i < positions.Count; i++)
        {
            var pos = positions[i];
            GameObject ea = Instantiate(enemyObject, pos, Quaternion.Euler(0, 0, 0), gameObject.transform) as GameObject;
            var ctrl = ea.GetComponent<EnemyController>();
            System.Random random = new System.Random();

            EnemyController.enemySkillLevels skillLevel = skillLevels[random.Next(skillLevels.Count)];
            ctrl.InitEnemy(i, skillLevel, gameObjectMap[skillLevel]);
            list.Add(ctrl);
        }

        Debug.Log(turn);
    }
}
