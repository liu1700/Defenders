using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public Text levelTime;
    public Text goldNum;
    public Text addGoldText;
    public Text addTime;
    public Text levelCnt;

    private void Start()
    {
        addGoldText.GetComponent<CanvasRenderer>().SetAlpha(0f);
        addTime.GetComponent<CanvasRenderer>().SetAlpha(0f);
        goldNum.text = "0";
    }

    public void UpdateLevel(int lv)
    {
        levelCnt.text = lv.ToString();
    }

    public void performAddGoldAnim(int count)
    {
        var countStr = count.ToString();
        if (count > 0)
        {
            countStr = "+" + countStr;
        }
        StartCoroutine(addGoldAnim(addGoldText, countStr, .6f));
    }

    public void performAddTimeAnim(string t)
    {
        StartCoroutine(addGoldAnim(addTime, t, .8f));
    }

    IEnumerator addGoldAnim(Text addTxt, string content, float dur)
    {
        addTxt.text = content;
        addTxt.CrossFadeAlpha(1f, dur, false);
        yield return new WaitForSeconds(dur);
        addTxt.CrossFadeAlpha(0f, dur, false);
    }
}
