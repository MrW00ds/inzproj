using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{


    // Start is called before the first frame update
    [SerializeField]
    public float bulletDamage = 1f;
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hit");
        HealthScript damagedObject = collision.gameObject.GetComponent<HealthScript>();
        if (damagedObject != null)
        {
            damagedObject.doDamage(bulletDamage);
        }
        Destroy(gameObject);
    }
}
