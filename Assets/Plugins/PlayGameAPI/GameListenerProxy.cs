#if UNITY_ANDROID
using UnityEngine;
using System.Collections;
namespace google.service.game
{
	public class GameListenerProxy : AndroidJavaProxy {
		private IGameListener listener;
		internal GameListenerProxy(IGameListener listener)
            : base("com.google.service.game.IGameListener")
		{
			this.listener = listener;
		}
         void onGameEvent(int result_code,string eventName,string eventData){
           //  Debug.Log("c# gamelisterproxy "+eventGroup+"   "+eventName+"   "+eventData);
             if(listener!=null)
			listener.onGameEvent(result_code,eventName,eventData);
         }
		string toString(){
			return "GameListenerProxy";
		}
	}
}
#endif