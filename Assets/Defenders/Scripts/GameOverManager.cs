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
    public bool isDisplayingAd;

    private AdManager admgr;
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

        var AdManagerObject = GameObject.FindGameObjectWithTag("AdManager");
        if (AdManagerObject != null)
        {
            admgr = AdManagerObject.GetComponent<AdManager>();
        }
    }

    private void Start()
    {
        needGoldNum.text = "-" + GameController.reviveUseGold.ToString();
        isDisplayingAd = false;
    }

    public void ActivatePanel(int playerCoine)
    {
        isDisplayingAd = false;
        if (showRevive)
        {
            revive.SetActive(true);
            if (playerCoine < GameController.reviveUseGold)
            {
                var useGoldObj = revive.transform.Find("UseGold");
                useGoldObj.gameObject.GetComponent<Button>().interactable = false;
                useGoldObj.gameObject.GetComponent<Image>().raycastTarget = false;
            }

            if (admgr && admgr.isShowRewardVideoReady())
            {
                viewVideoRevive.SetActive(true);
                admgr.UploadUserViewingVideoScene();
                isDisplayingAd = true;
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

            if (admgr && admgr.isShowRewardVideoReady())
            {
                viewVideoAddMoreGold.SetActive(true);
                admgr.UploadUserViewingVideoScene();
                isDisplayingAd = false;
            }
            else
            {
                viewVideoAddMoreGold.SetActive(false);
            }
        }
    }
}
