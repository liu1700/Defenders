using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{

    public List<EnemyController> list;

    void Start()
    {

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

    }
}
