using System;

namespace google.service.game
{
    // Interface for the methods to be invoked by the native plugin.
	internal interface IGameListener
    {
        void onGameEvent(int result_code,string eventName,string eventData);
    }
}
