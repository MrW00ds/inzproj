using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KilledEnemies : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshProUGUI displayTextEnemKill;
    int enemiesKilled = 0;
    int enemies = 0;
    void Start()
    {
        displayTextEnemKill = gameObject.GetComponent<TextMeshProUGUI>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    // Update is called once per frame
    void Update()
    {
        displayTextEnemKill.text = enemiesKilled.ToString()+"/"+ enemies;
    }

    public void add()
    {
        enemiesKilled++;
    }

    public int returnKilled() { 
        return enemiesKilled;
    }
}
