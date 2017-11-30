using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{

    /// <summary>
    /// This class spawn random birds inside the scene.
    /// </summary>

    public GameObject[] birds;          //birds in the menu scene
    private bool canSpawn;

    public GameObject spawnerAnchor;
    float x, y;

    IEnumerator Start()
    {
        canSpawn = false;
        yield return new WaitForSeconds(2.0f);
        canSpawn = true;
        x = spawnerAnchor.transform.position.x;
        y = spawnerAnchor.transform.position.y;
    }


    void Update()
    {
        if (canSpawn)
        {
            StartCoroutine(birdSpawner());
        }
    }


    IEnumerator birdSpawner()
    {

        if (!canSpawn)
            yield break;

        int dir = 0;
        if (Random.value > 0.5f)
            dir = 1;
        else
            dir = -1;

        canSpawn = false;

        GameObject b = Instantiate(birds[Random.Range(0, 2)], new Vector3(x * dir, Random.Range(y - 2.0f, y + 2.0f), 0), Quaternion.Euler(0, 180, 0)) as GameObject;
        b.name = "Bird";

        yield return new WaitForSeconds(Random.Range(15f, 20f));
        canSpawn = true;

    }

}
