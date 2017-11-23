using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmController : MonoBehaviour
{

    AudioSource bgm;

    void Start()
    {
        bgm = GetComponent<AudioSource>();
        bgm.volume = 0;

        DontDestroyOnLoad(gameObject);

        StartCoroutine(fadeInBgm());

    }

    IEnumerator fadeInBgm()
    {
        var t = 0.0f;
        while (t < 0.4)
        {
            t += Time.deltaTime;
            bgm.volume = t;
            yield return new WaitForSeconds(0.7f);
        }
        yield return new WaitForSeconds(0);
    }
}
