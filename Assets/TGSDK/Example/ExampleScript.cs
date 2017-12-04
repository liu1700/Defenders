using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Together;
using System.Text.RegularExpressions;

public class ExampleScript : MonoBehaviour {
	public InputField sceneId;
    public Text logField;

	private string[] scenes;
	private int sceneIndex = 0;

	void Awake (){ 
        TGSDK.SetDebugModel(true);
		TGSDK.SDKInitFinishedCallback = (string msg) => {
			TGSDK.TagPayingUser(TGPayingUser.TGMediumPaymentUser, "CNY", 0, 0);
			Log ("TGSDK finished : " + msg);
		};
#if UNITY_IOS && !UNITY_EDITOR
		TGSDK.Initialize ("hP7287256x5z1572E5n7");
#elif UNITY_ANDROID && !UNITY_EDITOR
		TGSDK.Initialize ("59t5rJH783hEQ3Jd7Zqr");
#endif
	}

    public void Log(string message)
    {
        Debug.Log("[TGSDK-Unity]  "+message);
        if(logField != null)
        {
			if (logField.text.Length > 100) {
				logField.text = message;
			} else {
            	logField.text = logField.text + "\n" + message;
			}
        }
    }

    public void PreloadAd()
    {
		TGSDK.PreloadAdSuccessCallback = (string msg) => {
			Log ("PreloadAdSuccessCallback : " + msg);
            scenes = Regex.Split(msg, ",", RegexOptions.IgnoreCase);            
            foreach (string scene in scenes) {
                Log (string.Format("TGSDK Scene [{0}] Parameter [intParam] = {1}",
                            scene,
                            TGSDK.GetIntParameterFromAdScene(scene, "intParam", -44)
                            ));
                Log (string.Format("TGSDK Scene [{0}] Parameter [floatParam] = {1}",
                            scene,
                            TGSDK.GetFloatParameterFromAdScene(scene, "floatParam", -44.44f)
                            ));
                Log (string.Format("TGSDK Scene [{0}] Parameter [stringParam] = {1}",
                            scene,
                            TGSDK.GetStringParameterFromAdScene(scene, "stringParam", "NONE")
                            ));
                Log (string.Format("TGSDK Scene [{0}] Parameter [imageParam] = {1}",
                            scene,
                            TGSDK.GetStringParameterFromAdScene(scene, "imageParam")
                            ));
            }
			RefreshSceneId();
		};
		TGSDK.PreloadAdFailedCallback = (string msg) => {
			Log ("PreloadAdFailedCallback : " + msg);
		};
		TGSDK.CPAdLoadedCallback = (string msg) => {
			Log ("CPAdLoadedCallback : " + msg);
		};
		TGSDK.VideoAdLoadedCallback = (string msg) => {
			Log ("VideoAdLoadedCallback : " + msg);
		};
		TGSDK.AdShowSuccessCallback = (string msg) => {
			Log ("AdShowSuccessCallback : " + msg);
		};
		TGSDK.AdShowFailedCallback = (string msg) => {
			Log ("AdShowFailedCallback : " + msg);
		};
		TGSDK.AdCompleteCallback = (string msg) => {
			Log ("AdCompleteCallback : " + msg);
		};
		TGSDK.AdCloseCallback = (string msg) => {
			Log ("AdCloseCallback : " + msg);
		};
		TGSDK.AdClickCallback = (string msg) => {
			Log ("AdClickCallback : " + msg);
		};
		TGSDK.AdRewardSuccessCallback = (string msg) => {
			Log ("AdRewardSuccessCallback : " + msg);
		};
		TGSDK.AdRewardFailedCallback = (string msg) => {
			Log ("AdRewardFailedCallback : " + msg);
		};
        TGSDK.PreloadAd();
    }

	private void RefreshSceneId() {
		if (scenes != null && scenes.Length > 0) {
			sceneId.text = scenes [sceneIndex];
		}
	}

	public void LastScene() {
		if (sceneIndex > 0) {
			sceneIndex--;
			RefreshSceneId();
		}
	}

	public void NextScene() {
		if (sceneIndex < scenes.Length-1) {
			sceneIndex++;
			RefreshSceneId();
		}
	}

    public void ShowAd()
    {
		string sceneid = sceneId.text;
		if (TGSDK.CouldShowAd (sceneid)) {
			string cpImagePath = TGSDK.GetCPImagePath(sceneid);
			if (null != cpImagePath) {
				Log("cpImagePath : " + cpImagePath);
				TGSDK.ShowCPView(sceneid);
				TGSDK.ReportCPClose (sceneid);
			}
			TGSDK.ShowAd(sceneid);
		} else {
			Log("Scene "+sceneid+" could not to show");
		}
	}

	public void ShowTestView()
	{
		string sceneid = sceneId.text;
		TGSDK.ShowTestView (sceneid);
	}

}
