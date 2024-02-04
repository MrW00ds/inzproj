using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;
using System;

public class EndGame : MonoBehaviour
{
    [SerializeField]
    public GameObject mainCanvas;
    [SerializeField]
    public GameObject endGameCanvas;
    [SerializeField]
    public GameObject timeTMP;

    private TextMeshProUGUI text;

    private float time = 0;

    private bool gameEnded = false;
    IEnumerable<GameObject> Enemies; 
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        text = timeTMP.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        text.text = TimeSpan.FromSeconds(time).ToString(@"mm\:ss");
    }

    private void FixedUpdate()
    {
        Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int alive = 0;
        foreach (GameObject obj in Enemies)
        {
            if (!obj.IsUnityNull()) { alive++; }
        }
        if (alive <= 0 && !gameEnded)
        {
            mainCanvas.SetActive(false);
            endGameCanvas.SetActive(true);
            foreach (var endText in endGameCanvas.GetComponentsInChildren<TextMeshProUGUI>())
            {
                if (endText.transform.name == "Time") {
                    endText.text = TimeSpan.FromSeconds(time).ToString(@"mm\:ss");
                }
            }
            float oldTime = PlayerPrefs.GetFloat("demoRunTime");
            if (oldTime > time ) {
                PlayerPrefs.SetFloat("demoRunTime", time);
            }
            gameEnded = true;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name); //moved to button action
        }
    }

    public void playerKilled() {
        mainCanvas.SetActive(false);
        endGameCanvas.SetActive(true);
        foreach (var endText in endGameCanvas.GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (endText.transform.name == "Time")
            {
                endText.text = "You got killed!";
            }
        }
        gameEnded = true;
    }
}
