using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    public float HP;
    public Transform eyesTransform;
    public float MoveThreshold = 0.01f;
    public float RotationThreshold = 1f; // Порог для изменения поворота (настраиваемый)

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
    [Header("Audio")]
    public AudioSource ridingAudioSource; // АудиоИсточник для воспроизведения
    public AudioSource audioSource; // АудиоИсточник для воспроизведения
    public AudioClip riding;
    public AudioClip[] audioClips;
    private bool isStoppingRiding = false;
    private bool isStartingRiding = true;
    
    
    private bool HasSeenPlayer = false;
    private Vector3 previousPosition;
    private Quaternion previousRotation; // Хранит предыдущий поворот
    private Transform player;
    private PlayerMovement playerMovement;
    private NavMeshAgent agent;
    private Animator animator;
    private Developermenu _developermenu;
    void Start()
    {
        _developermenu = GameObject.Find("DeveloperMenuController").GetComponent<Developermenu>();
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
    public void PlayTrack(int trackIndex)
    {
        if (trackIndex >= 0 && trackIndex < audioClips.Length)
        {
            audioSource.clip = audioClips[trackIndex];
            audioSource.Play();
        }
    }

    // Затухание аудио
    public IEnumerator FadeOut(float duration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // Восстановить громкость
    }
    public IEnumerator RidingSmoothStop(float duration)
    {
        float startVolume = ridingAudioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            ridingAudioSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        ridingAudioSource.Stop();
        ridingAudioSource.volume = startVolume; // Восстановить громкость
    }
    public IEnumerator RidingSmoothStart(float duration)
    {
        ridingAudioSource.clip = riding;
        float targetVolume = 0.05f;  // Целевая громкость
        ridingAudioSource.volume = 0; // Установить громкость в 0 в начале;
        ridingAudioSource.Play();
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            ridingAudioSource.volume = Mathf.Lerp(0, targetVolume, t / duration);
            yield return null;
        }

        ridingAudioSource.volume = targetVolume; // Убедитесь, что громкость устанавливается на целевое значение
        ; // Запустить аудио, если оно еще не играет
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
        Quaternion currentRotation = transform.rotation; // Получаем текущий поворот

        // Проверяем, изменилось ли положение или поворот
        bool hasMoved = Vector3.Distance(currentPosition, previousPosition) > MoveThreshold;
        bool hasRotated = Quaternion.Angle(currentRotation, previousRotation) > RotationThreshold; // Порог для поворота

        if (hasMoved | hasRotated)
        {
            animator.SetBool("Riding", true);
        
            if (!isStartingRiding)
            {
                StopCoroutine(RidingSmoothStop(0.1f));
                StartCoroutine(RidingSmoothStart(0.1f));

                isStartingRiding = true;
                isStoppingRiding = false;
            }
        }
        else
        {
            animator.SetBool("Riding", false);
        
            if (!isStoppingRiding)
            {
                StopCoroutine(RidingSmoothStart(0.1f));
                isStoppingRiding = true;
                isStartingRiding = false;
                StartCoroutine(RidingSmoothStop(0.1f));
            }
        }

        // Обновляем предыдущие значения
        previousPosition = currentPosition;
        previousRotation = currentRotation; // Обновляем предыдущий поворот
    }

    public void TakeDamageEnemy(float damage)
    {
        HasSeenPlayer = true;
        HP -= damage;
    }
    void Update()
    {
        if (HP <= 0 | _developermenu.isKilledEverybody)
        {
            Destroy(gameObject);
        }
        else
        {
            if(!_developermenu.isFreezed)
            {
                agent.isStopped = false;
            CheckIsMoving();
            if(!HasSeenPlayer)
            {
               HasSeenPlayer = IsInView();
            }
            if (HasSeenPlayer &&!IsSuperAttacking&&!IsWaitingAfterSuperAttack&&CountOfTimeSuperAttackTimeToContinue>=MaxCountOfTimeSuperAttackTimeToContinue)
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
                        PlayTrack(0);
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

            if (agent.avoidancePriority == 100)
            {
                if ((Vector3.Distance(gameObject.transform.position, player.position) < SuperAttackRangeToDamage)&&IsInView())
                {
                    Debug.Log("updateLocate");
                    ForceStopSuper = true;
                    agent.isStopped = true;
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

                Vector3 currentPosition1 = gameObject.transform.position;
                if (Vector3.Distance(currentPosition1, SavedPlayerPostion) < SuperMovingThreshold)
                {
                    IsGotToSavedPlayerTransform = true;
                }

                if (ForceStopSuper)
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
                    IsReadyToMove = false;
                    IsWaitingAfterSuperAttack = true;
                    IsSuperAttacking = false;
                    IsCoolDownSuperAttack = true;
                    StartCoroutine(SuperAttackCoolDown());
                    animator.SetTrigger("AfterSuper");
                    CountOfTimeSuperAttackTimeToContinue = 0;
                    if ((Vector3.Distance(gameObject.transform.position, player.position) < SuperAttackRangeToDamage) &&
                        IsInView())
                    {
                        Debug.Log("Popal");
                        playerMovement.TakeDamaage(SuperDamage);
                        playerMovement.Fall();


                    }
                }
            }
            }
            else
            {
                agent.isStopped = true;
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
