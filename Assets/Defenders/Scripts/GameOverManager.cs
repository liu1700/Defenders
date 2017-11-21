using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{

    public Text killText;
    public Text bestText;
    public Text addGoldText;
    public GameObject revive;
    public GameObject moreGold;

    bool showRevive, showAddMoreGold;

    private void Awake()
    {
        revive.SetActive(false);
        moreGold.SetActive(false);
        showRevive = true;
        showAddMoreGold = false;
    }

    public void ActivatePanel(int playerCoine)
    {
        if (showRevive)
        {
            revive.SetActive(true);
            if (playerCoine < 10)
            {
                var useGoldObj = revive.transform.Find("UseGold");
                useGoldObj.gameObject.GetComponent<Button>().interactable = false;
                useGoldObj.gameObject.GetComponent<Image>().raycastTarget = false;
            }
            showRevive = false;
            showAddMoreGold = true;
            return;
        }
        if (showAddMoreGold)
        {
            moreGold.SetActive(true);
            revive.SetActive(false);
            showAddMoreGold = false;
        }
    }
}
