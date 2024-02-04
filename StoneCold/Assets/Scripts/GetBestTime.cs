using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetBestTime : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        var playerTime = PlayerPrefs.GetFloat("demoRunTime", 0);

        if (playerTime > 0) { 
            gameObject.GetComponent<TextMeshProUGUI>().text = TimeSpan.FromSeconds(playerTime).ToString(@"mm\:ss");
        }
        else
        {
            gameObject.GetComponent<TextMeshProUGUI>().text = "None";
        }
    }
}
