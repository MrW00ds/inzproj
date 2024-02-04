using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particlesDeleteAfterDone : MonoBehaviour
{
    [SerializeField]
    float destroyTime = 0.2f;

    float time;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > destroyTime) {
            Destroy(gameObject);
        }
    }
}
