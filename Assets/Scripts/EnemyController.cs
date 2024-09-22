using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviour
{
    [FormerlySerializedAs("HP")] public float hp;
    public Transform eyesTransform;
    [FormerlySerializedAs("MoveThreshold")] public float moveThreshold = 0.01f;
    [FormerlySerializedAs("RotationThreshold")] public float rotationThreshold = 1f; // Порог для изменения поворота (настраиваемый)

    [FormerlySerializedAs("Damage")] [Header("Normal Attack")]
    public float damage;
    [FormerlySerializedAs("Speed")] public float speed;
    [FormerlySerializedAs("AttackRangeToDamage")] public float attackRangeToDamage;
    [FormerlySerializedAs("AttackCooldown")] public float attackCooldown;
    [FormerlySerializedAs("AttackAnimDamageDelay")] public float attackAnimDamageDelay;  
    private bool _isDelayAfterAttack = false;
    [FormerlySerializedAs("SuperDamage")] [Header("Super Attack")]
    public float superDamage;
    [FormerlySerializedAs("SuperSpeed")] public float superSpeed;
    [FormerlySerializedAs("SuperAngularSpeed")] public float superAngularSpeed;
    [FormerlySerializedAs("SuperAcceleration")] public float superAcceleration;
    [FormerlySerializedAs("SuperAttackRangeToDamage")] public float superAttackRangeToDamage;
    [FormerlySerializedAs("SuperAttackRangeToStart")] public float superAttackRangeToStart;
    [FormerlySerializedAs("SuperAttackCooldown")] public float superAttackCooldown;
    [FormerlySerializedAs("SuperAttackTimeToStayIn")] public float superAttackTimeToStayIn;
    [FormerlySerializedAs("SuperAttackTimeToContinue")] public float superAttackTimeToContinue;
    [FormerlySerializedAs("MaxDurationOfSuperAttack")] public float maxDurationOfSuperAttack;
    [FormerlySerializedAs("ProbabilityOfSuperAttack")] public int probabilityOfSuperAttack;
    [FormerlySerializedAs("SuperMovingThreshold")] public float superMovingThreshold;
    [FormerlySerializedAs("IsSuperAttacking")] public bool isSuperAttacking = false;
    [FormerlySerializedAs("IsReadyToMove")] public bool isReadyToMove = false;
    [FormerlySerializedAs("IsWaitingAfterSuperAttack")] public bool isWaitingAfterSuperAttack = false;
    private bool _isDelayAfterSuperAttack = false;
    private int _countOfVisiblity = 0;
    private int _maxCountOfVisiblity = 0;
    private Vector3 _savedPlayerPostion;
    private bool _isGotToSavedPlayerTransform = false;
    private bool _isCoolDownSuperAttack = false;
    private bool _isWaitingToSaveTransform = false;
    private int _countOfTimeSuperAttackTimeToContinue;
    private int _maxCountOfTimeSuperAttackTimeToContinue;
    private int _countOfDurationOfSuperAttack;
    private int _maxCountOfDurationOfSuperAttack;
    private bool _forceStopSuper;
    [Header("Audio")]
    public AudioSource ridingAudioSource; // АудиоИсточник для воспроизведения
    public AudioSource audioSource; // АудиоИсточник для воспроизведения
    public AudioClip riding;
    public AudioClip[] audioClips;
    private bool _isStoppingRiding = false;
    private bool _isStartingRiding = true;
    
    
    private bool _hasSeenPlayer = false;
    private Vector3 _previousPosition;
    private Quaternion _previousRotation; // Хранит предыдущий поворот
    private Transform _player;
    private PlayerMovement _playerMovement;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Developermenu _developermenu => Developermenu.Instance;
    void Start()
    {
        _maxCountOfDurationOfSuperAttack = (int)(maxDurationOfSuperAttack / 0.1f);
        _maxCountOfTimeSuperAttackTimeToContinue = (int)(superAttackTimeToContinue / 0.1f);
        _maxCountOfVisiblity = (int)(superAttackTimeToStayIn / 0.1f);
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
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
        yield return new WaitForSeconds(attackAnimDamageDelay);
        if (IsInView())
        {
            float distance = Vector3.Distance(eyesTransform.position, _player.position);
            if (distance < attackRangeToDamage)
            {
                _playerMovement.TakeDamaage(damage);
            }
        }
    }
    
    private IEnumerator AttackCooldownTimer()
    {
        yield return new WaitForSeconds(attackCooldown);
        _isDelayAfterAttack = false;
    }
    private IEnumerator SuperAttackCoolDown()
    {
        yield return new WaitForSeconds(superAttackCooldown);
        _isCoolDownSuperAttack = false;
    }
    private IEnumerator CheckDurationOfSuperAttacK()
    {
        while (true)
        {
            if(isSuperAttacking)
            {
                _countOfDurationOfSuperAttack += 1;
                if (_countOfDurationOfSuperAttack >= _maxCountOfDurationOfSuperAttack)
                {
                    _countOfDurationOfSuperAttack = 0;
                    _forceStopSuper = true;
                }
            }
            else
            {
                _countOfDurationOfSuperAttack = 0;
            }
            yield return new WaitForSeconds(0.1f);
        }

    }
    private IEnumerator SuperAttackVisiblityCheck()
    {
        while (true)
        {
            _countOfTimeSuperAttackTimeToContinue += 1;


            yield return new WaitForSeconds(0.1f);
            RaycastHit hit;
            if (Physics.Raycast(eyesTransform.position, _player.position - eyesTransform.position, out hit) && !_isCoolDownSuperAttack&&!isSuperAttacking)
            {
                if (hit.transform == _player)
                {
                    Vector3 currentPosition = transform.position;
                    if (Vector3.Distance(currentPosition, _player.position) < superAttackRangeToStart)
                    {
                        _countOfVisiblity += 1;
                    }
                    else
                    {
                        _countOfVisiblity = 0;
                    }

                }
                else if (hit.transform != gameObject.transform)
                {
                    _countOfVisiblity = 0;
                }
            }
            if (_countOfVisiblity == _maxCountOfVisiblity && !_isCoolDownSuperAttack)
            {
                _countOfVisiblity = 0;
                if (Random.Range(0, 100) < (probabilityOfSuperAttack - 1))
                {
                    isSuperAttacking = true;
                    _agent.isStopped = true;
                    _agent.speed = 0;
                    _isWaitingToSaveTransform = true;
                    _animator.SetTrigger("BeforeSuper");
                    _isGotToSavedPlayerTransform = false;

                }
            }
        }

    }
    private void CheckIsMoving()
    {
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation; // Получаем текущий поворот

        // Проверяем, изменилось ли положение или поворот
        bool hasMoved = Vector3.Distance(currentPosition, _previousPosition) > moveThreshold;
        bool hasRotated = Quaternion.Angle(currentRotation, _previousRotation) > rotationThreshold; // Порог для поворота

        if (hasMoved | hasRotated)
        {
            _animator.SetBool("Riding", true);
        
            if (!_isStartingRiding)
            {
                StopCoroutine(RidingSmoothStop(0.1f));
                StartCoroutine(RidingSmoothStart(0.1f));

                _isStartingRiding = true;
                _isStoppingRiding = false;
            }
        }
        else
        {
            _animator.SetBool("Riding", false);
        
            if (!_isStoppingRiding)
            {
                StopCoroutine(RidingSmoothStart(0.1f));
                _isStoppingRiding = true;
                _isStartingRiding = false;
                StartCoroutine(RidingSmoothStop(0.1f));
            }
        }

        // Обновляем предыдущие значения
        _previousPosition = currentPosition;
        _previousRotation = currentRotation; // Обновляем предыдущий поворот
    }

    public void TakeDamageEnemy(float damage)
    {
        _hasSeenPlayer = true;
        hp -= damage;
    }
    void Update()
    {
        if (hp <= 0 | _developermenu.isKilledEverybody)
        {
            Destroy(gameObject);
        }
        else
        {
            if(!_developermenu.isFreezed)
            {
                _agent.isStopped = false;
            CheckIsMoving();
            if(!_hasSeenPlayer)
            {
               _hasSeenPlayer = IsInView();
            }
            if (_hasSeenPlayer &&!isSuperAttacking&&!isWaitingAfterSuperAttack&&_countOfTimeSuperAttackTimeToContinue>=_maxCountOfTimeSuperAttackTimeToContinue)
            {
                
                float distance = Vector3.Distance(eyesTransform.position, _player.position);
                if (distance <= attackRangeToDamage)
                {
                    Vector3 lookPos = _player.position - transform.position;
                    lookPos.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(lookPos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.2f);
                    if(!_isDelayAfterAttack)
                    {
                        _animator.SetTrigger("Attack");
                        PlayTrack(0);
                        _isDelayAfterAttack = true;
                        _countOfTimeSuperAttackTimeToContinue = 0;
                        StartCoroutine(AttackAnimDamageTimer());
                        StartCoroutine(AttackCooldownTimer());
                    }
                    else
                    {
                        _agent.speed = 0;
                    }
                }
                else if(!_isDelayAfterAttack)
                {
                    _agent.speed = speed;
                    _agent.acceleration = 10;
                    _agent.angularSpeed = 2000;
                    _agent.avoidancePriority = 50;
                    _agent.SetDestination(_player.position);
                }
            }

            if (_agent.avoidancePriority == 100)
            {
                if ((Vector3.Distance(gameObject.transform.position, _player.position) < superAttackRangeToDamage)&&IsInView())
                {
                    Debug.Log("updateLocate");
                    _forceStopSuper = true;
                    _agent.isStopped = true;
                }
            }
            
            if (isSuperAttacking && isReadyToMove)
            {
                _agent.isStopped = false;
                if (_isWaitingToSaveTransform)
                {
                    _savedPlayerPostion = _player.position;
                    //SavedPlayerTransform = player.transform;
                    _isWaitingToSaveTransform = false;
                }

                Vector3 currentPosition1 = gameObject.transform.position;
                if (Vector3.Distance(currentPosition1, _savedPlayerPostion) < superMovingThreshold)
                {
                    _isGotToSavedPlayerTransform = true;
                }

                if (_forceStopSuper)
                {
                    _isGotToSavedPlayerTransform = true;
                }

                if (!_isGotToSavedPlayerTransform)
                {

                    _animator.SetBool("Rotating", true);
                    _agent.avoidancePriority = 100;
                    _agent.acceleration = superAcceleration;
                    _agent.angularSpeed = superAngularSpeed;
                    _agent.speed = superSpeed;
                    _agent.SetDestination(_savedPlayerPostion);
                }
                else
                {
                    _animator.SetBool("Rotating", false);
                    isReadyToMove = false;
                    isWaitingAfterSuperAttack = true;
                    isSuperAttacking = false;
                    _isCoolDownSuperAttack = true;
                    StartCoroutine(SuperAttackCoolDown());
                    _animator.SetTrigger("AfterSuper");
                    _countOfTimeSuperAttackTimeToContinue = 0;
                    if ((Vector3.Distance(gameObject.transform.position, _player.position) < superAttackRangeToDamage) &&
                        IsInView())
                    {
                        Debug.Log("Popal");
                        _playerMovement.TakeDamaage(superDamage);
                        _playerMovement.Fall();


                    }
                }
            }
            }
            else
            {
                _agent.isStopped = true;
            }
        }
    }
    private bool IsInView()
    {
        RaycastHit hit;
        if (Physics.Raycast(eyesTransform.position, _player.position - eyesTransform.position, out hit))
        { 
            if (hit.transform != _player)
            {
                return false;
            }
        }
        return true;
    }
}
