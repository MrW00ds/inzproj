using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100;
    [SerializeField]
    private float actualHealth;


    // Start is called before the first frame update
    void Start()
    {
        actualHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (actualHealth <= 0)
        {
            if (gameObject.tag == "Player")
            {
                GameObject.Find("EventSystem").GetComponent<EndGame>().playerKilled();
                Time.timeScale = 0.2f;
            }
            else { 
                var killedScript = GameObject.Find("Killed").GetComponent<KilledEnemies>();
                killedScript.add();
                Destroy(gameObject);
            }
        }
        
    }

    public void doDamage(float damageDone) 
    { 
        actualHealth -= damageDone;
        //gameObject.GetComponent<SpriteRenderer>().color = new Color(1, (actualHealth/maxHealth), (actualHealth / maxHealth));
    }
}
