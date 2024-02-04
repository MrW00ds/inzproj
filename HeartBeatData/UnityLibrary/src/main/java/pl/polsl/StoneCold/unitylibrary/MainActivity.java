package pl.polsl.StoneCold.unitylibrary;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.os.Bundle;
import android.util.Log;


import androidx.localbroadcastmanager.content.LocalBroadcastManager;

import com.unity3d.player.UnityPlayer;



public class MainActivity extends UnityPlayerActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);

        IntentFilter messageFilter = new IntentFilter(Intent.ACTION_SEND);
        Receiver messageReceiver = new Receiver();
        LocalBroadcastManager.getInstance(this).registerReceiver(messageReceiver, messageFilter);
    }
}
class Receiver extends BroadcastReceiver{
    @Override
    public void onReceive(Context context, Intent intent) {
        String result = intent.getStringExtra("message");
        UnityPlayer.UnitySendMessage("Canvas", "ShowResult", result);
        Log.d("Message received in plugin", "true");
    }
}
