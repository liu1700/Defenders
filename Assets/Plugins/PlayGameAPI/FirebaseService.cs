using UnityEngine;
using System.Collections;

namespace google.service.game
{
    public class FirebaseService
    {
        //onConfigLoadSuccess or onConfigLoadFail
        public delegate void FirebaseEventHandler(int result_code, string eventName, string msg);
        public event FirebaseEventHandler eventHandler;

        private static FirebaseService _instance;
        
        public static FirebaseService Instance()
        {
            if (_instance == null)
            {
                _instance = new FirebaseService();
                _instance.preInit();
            }
            return _instance;
        }

#if UNITY_ANDROID
        class InnerGameListener : IGameListener
        {
            internal FirebaseService firebaseInstance;
            public void onGameEvent(int result_code, string eventName, string paramString)
            {
                if (firebaseInstance.eventHandler != null)
                    firebaseInstance.eventHandler(result_code, eventName, paramString);
            }
        }
        private AndroidJavaObject jobject;
        private void preInit()
        {
            if (jobject == null)
            {
                AndroidJavaClass gameUnityPluginClass = new AndroidJavaClass("com.google.service.firebase.FirebasePlugin");
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activy = jc.GetStatic<AndroidJavaObject>("currentActivity");
                jobject = gameUnityPluginClass.CallStatic<AndroidJavaObject>("getInstance");
                InnerGameListener innerlistener = new InnerGameListener();
                innerlistener.firebaseInstance = this;
                jobject.Call("setContext", new object[] { activy, new GameListenerProxy(innerlistener) });
            }
        }
        public void logCrash(string msg)
        {
            jobject.Call("logCrash", new object[] { msg });
        }
        public void logCat(int i, string s, string s1)
        {
            jobject.Call("logCat", new object[] { i,s,s1 });
        }
        public void logException(string msg)
        {
            jobject.Call("logException", new object[] { msg });
        }
        public string getStringFromConfig(string key)
        {
            AndroidJavaObject config = getConfig();
            return config.Call<string>("getString");
        }
        public double getDoubleFromConfig(string key)
        {
            AndroidJavaObject config = getConfig();
            return config.Call<double>("getDouble");
        }
        public bool isConfigActivateFetched(string key)
        {
            AndroidJavaObject config = getConfig();
            return config.Call<bool>("activateFetched");
        }
        public bool getBooleanFromConfig(string key)
        {
            AndroidJavaObject config = getConfig();
            return config.Call<bool>("getBoolean");
        }
        public long getLongFromConfig(string key)
        {
            AndroidJavaObject config = getConfig();
            return config.Call<long>("getLong");
        }
        public BitArray getBitArrayFromConfig(string key)
        {
            AndroidJavaObject config = getConfig();
            return config.Call<BitArray>("getByteArray");
        }

        public AndroidJavaObject getConfig()
        {
            return jobject.Call< AndroidJavaObject>("getConfig");
        }

        public void setDefaultConfig(string json)
        {
            jobject.Call("setDefaultConfig", json);
        }

        public void loadConfig()
        {
            jobject.Call("loadConfig");
        }

        public void setDeveloperModeEnabled(bool enable)
        {
            jobject.Call("setDeveloperModeEnabled", enable);
        }

#elif UNITY_EDITOR
		private void preInit()
		{
		}

		  public void logCrash(string msg)
        {
           
        }
        public void logCat(int i, string s, string s1)
        {
            
        }
        public void logException(string msg)
        {
            
        }
        public string getStringFromConfig(string key)
        {
            return null;
        }
        public double getDoubleFromConfig(string key)
        {
            return 0;
        }
        public bool isConfigActivateFetched(string key)
        {
            return false;
        }
        public bool getBooleanFromConfig(string key)
        {
           return false;
        }
        public long getLongFromConfig(string key)
        {
           return 0;
        }
        public BitArray getBitArrayFromConfig(string key)
        {
           return null;
        }

        public AndroidJavaObject getConfig()
        {
            return null;
        }

        public void setDefaultConfig(string json)
        {
            
        }

        public void loadConfig()
        {
            
        }

        public void setDeveloperModeEnabled(bool enable)
        {
            
        }
       
#endif

    }
}
