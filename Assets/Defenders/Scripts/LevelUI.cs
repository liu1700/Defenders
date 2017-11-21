using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public Text levelNum;
    public Text goldNum;
    public Text addGoldText;

    private void Start()
    {
        addGoldText.GetComponent<CanvasRenderer>().SetAlpha(0f);
        goldNum.text = "0";
    }

    public void performAddGoldAnim(int count)
    {
        StartCoroutine(addGoldAnim(count));
    }

    IEnumerator addGoldAnim(int count)
    {
        addGoldText.text = "+" + count.ToString();
        addGoldText.CrossFadeAlpha(1f, .25f, false);
        yield return new WaitForSeconds(0.25f);
        addGoldText.CrossFadeAlpha(0f, .25f, false);
    }
}
