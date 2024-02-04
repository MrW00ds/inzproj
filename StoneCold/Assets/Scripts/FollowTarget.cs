using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private GameObject cameraa;
    [SerializeField]
    float rangeY = 2;
    [SerializeField]
    float rangeX = 2;

    float targetValueY = 0;
    float lastValueY = 0;
    float targetValueX = 0;
    float lastValueX = 0;
    [SerializeField]
    float changeSpeed = 0.025f;
    
// Start is called before the first frame update
void Start()
    {
        targetValueY = target.transform.up.y * rangeY;
        lastValueY = targetValueY;
        targetValueX = target.transform.up.x * rangeX;
        lastValueX = targetValueX;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targetValueY = target.transform.up.y * rangeY;
    if (lastValueY > targetValueY)
    {
            lastValueY -= changeSpeed;
    }
    if (lastValueY < targetValueY)
    {
            lastValueY += changeSpeed;
    }

        targetValueX = target.transform.up.x * rangeX;
        if (lastValueX > targetValueX)
        {
            lastValueX -= changeSpeed;
        }
        if (lastValueX < targetValueX)
        {
            lastValueX += changeSpeed;
        }
        cameraa.transform.position = new Vector3(target.transform.position.x + lastValueX, target.transform.position.y + lastValueY, -10);
    }
}
