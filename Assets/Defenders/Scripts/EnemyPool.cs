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

    private void Start()
    {
        list = new List<EnemyController>();
    }

    void generatePos(int points)
    {
        positions = new List<Vector3>();
        for (int i = 0; i < points; i++)
        {
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
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
        generatePos(turn);
        for (int i = 0; i < positions.Count; i++)
        {
            var pos = positions[i];
            GameObject ea = Instantiate(enemyObject, pos, Quaternion.Euler(0, 0, 0), gameObject.transform) as GameObject;
            var ctrl = ea.GetComponent<EnemyController>();
            ctrl.enemyId = i;
            list.Add(ctrl);
        }

        Debug.Log(turn);
    }
}
