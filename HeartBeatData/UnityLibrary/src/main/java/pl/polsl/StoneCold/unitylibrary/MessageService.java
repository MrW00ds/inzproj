package pl.polsl.StoneCold.unitylibrary;
import android.content.Intent;
import android.util.Log;

import androidx.localbroadcastmanager.content.LocalBroadcastManager;

import com.google.android.gms.wearable.MessageEvent;
import com.google.android.gms.wearable.WearableListenerService;
public class MessageService extends WearableListenerService {
    @Override
    public void onMessageReceived(MessageEvent msgEvent){
        if(msgEvent.getPath().equals("/measurements")){
            final String message = new String(msgEvent.getData());
            Log.d("MessageService", message);

            Intent messageIntent = new Intent();
            messageIntent.setAction(Intent.ACTION_SEND);
            messageIntent.putExtra("message", message);

            LocalBroadcastManager.getInstance(this).sendBroadcast(messageIntent);
        } else {
            super.onMessageReceived(msgEvent);
        }
    }
}
