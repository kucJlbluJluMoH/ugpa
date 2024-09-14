using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    public float HP;
    public Transform eyesTransform;
    public float MoveThreshold = 0.01f;
    [Header("Normal Attack")]
    public float Damage;
    public float Speed;
    public float AttackRangeToDamage;
    public float AttackCooldown;
    public float AttackAnimDamageDelay;  
    private bool IsDelayAfterAttack = false;
    [Header("Super Attack")]
    public float SuperDamage;
    public float SuperSpeed;
    public float SuperAngularSpeed;
    public float SuperAcceleration;
    public float SuperAttackRangeToDamage;
    public float SuperAttackRangeToStart;
    public float SuperAttackCooldown;
    public float AfterSuperAttackDelay;
    public float SuperAttackTimeToStayIn;
    public float SuperAttackTimeToContinue;
    public float MaxDurationOfSuperAttack;
    public int ProbabilityOfSuperAttack;
    public float SuperMovingThreshold;
    public bool IsSuperAttacking = false;
    public bool IsReadyToMove = false;
    public bool IsWaitingAfterSuperAttack = false;
    private bool IsDelayAfterSuperAttack = false;
    private int CountOfVisiblity = 0;
    private int MaxCountOfVisiblity = 0;
    private Vector3 SavedPlayerPostion;
    private bool IsGotToSavedPlayerTransform = false;
    private bool IsCoolDownSuperAttack = false;
    private bool IsWaitingToSaveTransform = false;
    private int CountOfTimeSuperAttackTimeToContinue;
    private int MaxCountOfTimeSuperAttackTimeToContinue;
    private int CountOfDurationOfSuperAttack;
    private int MaxCountOfDurationOfSuperAttack;
    private bool ForceStopSuper;


    private Vector3 previousPosition;
    private Transform player;
    private PlayerMovement playerMovement;
    private NavMeshAgent agent;
    private Animator animator;
    void Start()
    {
        MaxCountOfDurationOfSuperAttack = (int)(MaxDurationOfSuperAttack / 0.1f);
        MaxCountOfTimeSuperAttackTimeToContinue = (int)(SuperAttackTimeToContinue / 0.1f);
        MaxCountOfVisiblity = (int)(SuperAttackTimeToStayIn / 0.1f);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        StartCoroutine(SuperAttackVisiblityCheck());
        StartCoroutine(CheckDurationOfSuperAttacK());
    }
    private IEnumerator AttackAnimDamageTimer()
    {
        yield return new WaitForSeconds(AttackAnimDamageDelay);
        if (IsInView())
        {
            float distance = Vector3.Distance(eyesTransform.position, player.position);
            if (distance < AttackRangeToDamage)
            {
                playerMovement.TakeDamaage(Damage);
            }
        }
    }
    
    private IEnumerator AttackCooldownTimer()
    {
        yield return new WaitForSeconds(AttackCooldown);
        IsDelayAfterAttack = false;
    }
    private IEnumerator SuperAttackCoolDown()
    {
        yield return new WaitForSeconds(SuperAttackCooldown);
        IsCoolDownSuperAttack = false;
    }
    private IEnumerator CheckDurationOfSuperAttacK()
    {
        while (true)
        {
            if(IsSuperAttacking)
            {
                CountOfDurationOfSuperAttack += 1;
                if (CountOfDurationOfSuperAttack >= MaxCountOfDurationOfSuperAttack)
                {
                    CountOfDurationOfSuperAttack = 0;
                    ForceStopSuper = true;
                }
            }
            else
            {
                CountOfDurationOfSuperAttack = 0;
            }
            yield return new WaitForSeconds(0.1f);
        }

    }
    private IEnumerator SuperAttackVisiblityCheck()
    {
        while (true)
        {
            CountOfTimeSuperAttackTimeToContinue += 1;


            yield return new WaitForSeconds(0.1f);
            RaycastHit hit;
            if (Physics.Raycast(eyesTransform.position, player.position - eyesTransform.position, out hit) && !IsCoolDownSuperAttack&&!IsSuperAttacking)
            {
                if (hit.transform == player)
                {
                    Vector3 currentPosition = transform.position;
                    if (Vector3.Distance(currentPosition, player.position) < SuperAttackRangeToStart)
                    {
                        CountOfVisiblity += 1;
                    }
                    else
                    {
                        CountOfVisiblity = 0;
                    }

                }
                else if (hit.transform != gameObject.transform)
                {
                    CountOfVisiblity = 0;
                }
            }
            if (CountOfVisiblity == MaxCountOfVisiblity && !IsCoolDownSuperAttack)
            {
                CountOfVisiblity = 0;
                if (Random.Range(0, 100) < (ProbabilityOfSuperAttack - 1))
                {
                    IsSuperAttacking = true;
                    agent.isStopped = true;
                    agent.speed = 0;
                    IsWaitingToSaveTransform = true;
                    animator.SetTrigger("BeforeSuper");
                    IsGotToSavedPlayerTransform = false;

                }
            }
        }

    }
    private void CheckIsMoving()
    {
        Vector3 currentPosition = transform.position;
        if (Vector3.Distance(currentPosition, previousPosition) > MoveThreshold)
        {
            animator.SetBool("Riding", true);
        }
        else
        {
            animator.SetBool("Riding", false);
        }
        previousPosition = currentPosition;
    }
    void Update()
    {
        if (HP <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            CheckIsMoving();
            if (IsInView() &&!IsSuperAttacking&&!IsWaitingAfterSuperAttack&&CountOfTimeSuperAttackTimeToContinue>=MaxCountOfTimeSuperAttackTimeToContinue)
            {
                
                float distance = Vector3.Distance(eyesTransform.position, player.position);
                if (distance <= AttackRangeToDamage)
                {
                    Vector3 lookPos = player.position - transform.position;
                    lookPos.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(lookPos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
                    if(!IsDelayAfterAttack)
                    {
                        animator.SetTrigger("Attack");
                        IsDelayAfterAttack = true;
                        CountOfTimeSuperAttackTimeToContinue = 0;
                        StartCoroutine(AttackAnimDamageTimer());
                        StartCoroutine(AttackCooldownTimer());
                    }
                    else
                    {
                        agent.speed = 0;
                    }
                }
                else if(!IsDelayAfterAttack)
                {
                    agent.speed = Speed;
                    agent.acceleration = 10;
                    agent.angularSpeed = 2000;
                    agent.avoidancePriority = 50;
                    agent.SetDestination(player.position);
                }
            }
            if (IsSuperAttacking && IsReadyToMove)
            {
                agent.isStopped = false;
                if (IsWaitingToSaveTransform)
                {
                    SavedPlayerPostion = player.position;
                    //SavedPlayerTransform = player.transform;
                    IsWaitingToSaveTransform = false;
                }
                Vector3 currentPosition1 =gameObject.transform.position;
                if (Vector3.Distance(currentPosition1, SavedPlayerPostion) < SuperMovingThreshold)
                {
                    IsGotToSavedPlayerTransform = true;
                }
                if(ForceStopSuper)
                {
                    IsGotToSavedPlayerTransform = true;
                }
                if (!IsGotToSavedPlayerTransform)
                {
                    
                    animator.SetBool("Rotating", true);
                    agent.avoidancePriority = 100;
                    agent.acceleration = SuperAcceleration;
                    agent.angularSpeed = SuperAngularSpeed;
                    agent.speed = SuperSpeed;
                    agent.SetDestination(SavedPlayerPostion);
                }
                else
                {
                    animator.SetBool("Rotating", false);
                    Debug.Log("stopMove");
                    IsReadyToMove = false;
                    IsWaitingAfterSuperAttack = true;
                    IsSuperAttacking = false;   
                    IsCoolDownSuperAttack = true;
                    StartCoroutine(SuperAttackCoolDown());
                    animator.SetTrigger("AfterSuper");
                    CountOfTimeSuperAttackTimeToContinue = 0;
                    if ((Vector3.Distance(gameObject.transform.position, player.position) < SuperAttackRangeToDamage)&&IsInView())
                    {
                        Debug.Log("Popal");
                        playerMovement.TakeDamaage(SuperDamage);
                        playerMovement.Fall();
                       
                        
                    }
                }
         
            }
        }
    }
    private bool IsInView()
    {
        RaycastHit hit;
        if (Physics.Raycast(eyesTransform.position, player.position - eyesTransform.position, out hit))
        { 
            if (hit.transform != player)
            {
                return false;
            }
        }
        return true;
    }

}
