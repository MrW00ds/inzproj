using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Windows;

public class Plugin_script : MonoBehaviour
{
    [SerializeField]
    private GameObject Result;

    [SerializeField]
    int debugHeartBeat = 0;// nadpisuje wartoœci z zegarka, do testów

    [SerializeField]
    int heartBeat = 60;

    [SerializeField]
    private int lastKnownValue = 60;

    [SerializeField]
    private float changeSpeed = 0.1f;

    [SerializeField]
    private float spreadMod = 0.3f;

    [SerializeField]
    private GameObject mainCamera;

    [SerializeField]
    private GameObject player;

    private Gun_shoot gun;

    private Volume volume;

    private float lastEffectsValue = 0.0f;

    private float orgBulletSpread;

    AndroidJavaObject _pluginActivity;
    // Start is called before the first frame update
    void Start()
    {
        _pluginActivity = new AndroidJavaObject("pl.polsl.StoneCold.unitylibrary.MainActivity");
        volume = mainCamera.gameObject.GetComponent<Volume>();
        gun = player.GetComponentInChildren<Gun_shoot>();
        orgBulletSpread = gun.bulletSpread;
    }
    public void ShowResult(string msg) {
        if (debugHeartBeat == 0)
        {
            //string msg = "1703105157786_ time Last measured: 87,0";
            //Debug.Log("Value received: " + msg);
            var modMsg = msg;
            modMsg = modMsg.Replace(',', '0');
            string result = System.Text.RegularExpressions.Regex.Match(modMsg, @"\d+$").Value;
            if (int.Parse(result) / 100 == 0)
            {
                heartBeat = lastKnownValue;
            }
            else
            {
                heartBeat = int.Parse(result) / 100;
                lastKnownValue = heartBeat;
            }

        }
        else
        {
            heartBeat = debugHeartBeat;
        }
        //Debug.Log("Value parsed: " + heartBeat.ToString());
        Result.GetComponent<TMPro.TextMeshProUGUI>().text = heartBeat.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int input_start = 150;
        int input_end = 300;
        int output_start = 100;
        int output_end = 100;
        if (heartBeat < 150)
        {
            input_start = 120;
            input_end = 150;
            output_start = 80;
            output_end = 100;
        }
        if (heartBeat < 120)
        {
            input_start = 80;
            input_end = 120;
            output_start = 20;
            output_end = 80;
        }
        if (heartBeat < 80)
        {
            input_start = 60;
            input_end = 80;
            output_start = 0;
            output_end = 20;
        }
        if (heartBeat < 60) {
            input_start = 0;
            input_end = 60;
            output_start = 0;
            output_end = 0;
        }

        float slope = 1.0f * (output_end - output_start) / (input_end - input_start);
        float targetValue = output_start + (slope * (heartBeat - input_start));

        if (lastEffectsValue > targetValue)
        {
            lastEffectsValue -= changeSpeed;
        }
        if (lastEffectsValue < targetValue)
        {
            lastEffectsValue += changeSpeed;
        }

        gun.bulletSpread = orgBulletSpread + (spreadMod * (lastEffectsValue / 100));


        foreach (var effect in volume.profile.components)
        {
            try
            {
                Vignette vignette = (Vignette)effect;
                if (vignette != null)
                {
                    vignette.intensity.value = lastEffectsValue / 100;
                }
            }
            catch { }
            try
            {
                FilmGrain filmGrain = (FilmGrain)effect;
                if (filmGrain != null)
                {
                    filmGrain.intensity.value = lastEffectsValue / 100;
                }
            }
            catch { }
            try
            {
                ColorAdjustments colorAdjustments = (ColorAdjustments)effect;
                if (colorAdjustments != null)
                {
                    colorAdjustments.saturation.value = -lastEffectsValue;
                }
            }
            catch { }
        }
    }
}
