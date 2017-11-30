using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTowerFx : MonoBehaviour
{

    public GameObject[] hitFxs;

    public static HitTowerFx instance;

    private void Start()
    {
        instance = this;
    }

    public void GetHit(Vector2 contactPoint, Quaternion q)
    {
        GameObject hit = Instantiate(hitFxs[Random.Range(0, hitFxs.Length)], contactPoint, q) as GameObject;
    }

}
