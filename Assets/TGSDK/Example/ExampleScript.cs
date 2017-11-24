using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Together;
using System.Text.RegularExpressions;

public class ExampleScript : MonoBehaviour {
    public InputField username;
    public Text logField;

	void Awake (){ 
        TGSDK.SetDebugModel(true);
		TGSDK.SDKInitFinishedCallback = (string msg) => {
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
            string[] scenes = Regex.Split(msg, ",", RegexOptions.IgnoreCase);            
#if UNITY_IOS && !UNITY_EDITOR
			username.text = "qfJDzBR0aMUaVIWF2Q7";
#elif UNITY_ANDROID && !UNITY_EDITOR
			username.text = "hiRZYZxDI7c2LaOgrE7";
#endif
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

    public void ShowAd()
    {
		string sceneid = username.text;
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
		string sceneid = username.text;
		TGSDK.ShowTestView (sceneid);
	}

}
