using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
namespace google.service.game
{
    public class FirebaseAnalytic
    {
        private static FirebaseAnalytic _instance;

        public static FirebaseAnalytic Instance()
        {
            if (_instance == null)
            {
                _instance = new FirebaseAnalytic();
                _instance.preInit();
            }
            return _instance;
        }



#if UNITY_ANDROID
        private AndroidJavaObject jobject;
        private void preInit()
        {
            if (jobject == null)
            {
                AndroidJavaClass gameUnityPluginClass = new AndroidJavaClass("com.google.service.analytics.AnalyticPlugin");
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activy = jc.GetStatic<AndroidJavaObject>("currentActivity");
                jobject = gameUnityPluginClass.CallStatic<AndroidJavaObject>("getInstance",activy);
            }
        }

        public void logEvent(string name, string jsonObjectString)
        {
            jobject.Call("logEvent", new object[] { name,jsonObjectString });
        }
        public void setUserProperty(string name,string value)
        {
            jobject.Call("setUserProperty",new object[]{name,value});
        }
        public void setUserId(string userID)
        {
            jobject.Call("setUserId",userID);
        }

        public void setSessionTimeoutDuration(long milliseconds)
        {
            jobject.Call("setSessionTimeoutDuration",milliseconds);
        }

        public void setMinimumSessionDuration(long milliseconds)
        {
            jobject.Call("setMinimumSessionDuration", milliseconds);
        }

        public void setAnalyticsCollectionEnabled(bool enable)
        {
            jobject.Call("setAnalyticsCollectionEnabled",enable);
        }
       
#elif UNITY_EDITOR
		private void preInit()
		{
		}

		  public void logEvent(string name, string jsonObjectString)
        {
            
        }
        public void setUserProperty(string name,string value)
        {
            
        }
        public void setUserId(string userID)
        {
            
        }

        public void setSessionTimeoutDuration(long milliseconds)
        {
            
        }

        public void setMinimumSessionDuration(long milliseconds)
        {
            
        }

        public void setAnalyticsCollectionEnabled(bool enable)
        {
            
        }  
       
#endif

    }
}
