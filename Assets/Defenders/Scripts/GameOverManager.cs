using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{

    public Text killText;
    public Text bestText;
    public Text addGoldText;
    public Text needGoldNum;
    public GameObject revive;
    public GameObject moreGold;

    bool showRevive, showAddMoreGold;

    private GameObject AdManagerObject;
    private GameObject viewVideoRevive;
    private GameObject viewVideoAddMoreGold;

    private void Awake()
    {
        viewVideoRevive = revive.transform.Find("ViewVideo").gameObject;
        viewVideoAddMoreGold = moreGold.transform.Find("ViewVideo").gameObject;

        revive.SetActive(false);
        moreGold.SetActive(false);
        showRevive = true;
        showAddMoreGold = false;

        AdManagerObject = GameObject.FindGameObjectWithTag("AdManager");
    }

    private void Start()
    {
        needGoldNum.text = "-" + GameController.reviveUseGold.ToString();
    }

    public void ActivatePanel(int playerCoine)
    {
        if (showRevive)
        {
            revive.SetActive(true);
            if (playerCoine < GameController.reviveUseGold)
            {
                var useGoldObj = revive.transform.Find("UseGold");
                useGoldObj.gameObject.GetComponent<Button>().interactable = false;
                useGoldObj.gameObject.GetComponent<Image>().raycastTarget = false;
            }

            if (AdManagerObject && AdManagerObject.GetComponent<AdManager>().isShowRewardVideoReady())
            {
                viewVideoRevive.SetActive(true);
            }
            else
            {
                viewVideoRevive.SetActive(false);
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

            if (AdManagerObject && AdManagerObject.GetComponent<AdManager>().isShowRewardVideoReady())
            {
                viewVideoAddMoreGold.SetActive(true);
            }
            else
            {
                viewVideoAddMoreGold.SetActive(false);
            }
        }
    }
}
