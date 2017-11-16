using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{

    List<EnemyController> list;
    public GameObject enemyObject;
    public Vector3 center;
    public float minX, maxX;
    public float minY, maxY;
    List<Vector3> positions;

    float minEnemyCount, maxEnemyCount;
    List<EnemyController.enemySkillLevels> skillLevels;

    Dictionary<int, EnemyController.enemySkillLevels> levelUnlock;
    Dictionary<int, int[]> enemyCountUnlock;

    private void Start()
    {
        skillLevels = new List<EnemyController.enemySkillLevels>();
        list = new List<EnemyController>();
        levelUnlock = new Dictionary<int, EnemyController.enemySkillLevels>();
        // key: 第几回合, val: 解锁生成难度
        levelUnlock.Add(1, EnemyController.enemySkillLevels.easy);
        levelUnlock.Add(5, EnemyController.enemySkillLevels.normal);
        levelUnlock.Add(20, EnemyController.enemySkillLevels.hard);
        levelUnlock.Add(25, EnemyController.enemySkillLevels.Robinhood);

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
                Destroy(e.gameObject);
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
            ctrl.enemySkill = skillLevel;
            ctrl.enemyId = i;
            list.Add(ctrl);
        }

        Debug.Log(turn);
    }
}
