using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AIScript : MonoBehaviour {
[SerializeField]
public float radius;
    [SerializeField]
    [Range(0, 360)]
    public float angle;
    [SerializeField]
    public LayerMask targetMask;
    [SerializeField]
    public LayerMask obstructionMask;
    [SerializeField]
    public bool canSeePlayer;
    [SerializeField]
    private float reactionTime;
    [SerializeField]
    public bool sawPlayer = false;

    [SerializeField]
    public float rotationSpeed = 0.2f; 

    private Rigidbody2D parentRB;

    

    public GameObject playerRef;

    private Vector3 lastDestination;

    private NavMeshAgent agent;

    private void Start()
{
    playerRef = GameObject.FindGameObjectWithTag("Player");
    parentRB = gameObject.GetComponent<Rigidbody2D>();
    agent = GetComponent<NavMeshAgent>();
    agent.updateUpAxis = false;
    agent.updateRotation = false;
    lastDestination = transform.position;
    StartCoroutine(FOVRoutine());
}

private IEnumerator FOVRoutine()
{
    WaitForSeconds wait = new WaitForSeconds(0.2f);

    while (true)
    {
        yield return wait;
        FieldOfViewCheck();
    }
}

private void FieldOfViewCheck()
{
        Collider2D[] rangeChecks = Physics2D.OverlapCircleAll(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            UnityEngine.Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            //Debug.Log(directionToTarget + " " + gameObject.transform.up);

            if (Vector3.Angle(gameObject.transform.up, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask);

                if (hit.collider == null) {
                    canSeePlayer = true;
                    sawPlayer = true;
                    lastDestination = playerRef.transform.position;
                }
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }
void Update()
    {

        if (canSeePlayer)
        {
            RotateToTarget(new Vector2(playerRef.transform.position.x, playerRef.transform.position.y));
            Invoke("SawPlayer", reactionTime);
            agent.isStopped = true ;
        }
        else if (sawPlayer) { 
            RotateToTarget(new Vector2(playerRef.transform.position.x, playerRef.transform.position.y));
            agent.isStopped = false ;
            agent.SetDestination(new Vector3(playerRef.transform.position.x, playerRef.transform.position.y, 0));
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }


    private void RotateToTarget(Vector2 targetDirection)
    {
        Vector2 directionToTarget = targetDirection - parentRB.position;
        float targetAngle;

        if (directionToTarget.x > 0)
            targetAngle = Quaternion.LookRotation(directionToTarget).z * 180 + 270;
        else
            targetAngle = Quaternion.LookRotation(directionToTarget).z * 180 + 90;

        float smoothAngle = Mathf.LerpAngle(parentRB.rotation, targetAngle, Time.deltaTime * rotationSpeed);
        parentRB.rotation = smoothAngle;
    }

    private void SawPlayer() { 
        GetComponentInChildren<Gun_shoot>().AIShoot();
    }
}
