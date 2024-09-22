using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Serialization;

public class BlasterController : MonoBehaviour
{
    // Start is called before the first frame update
    [FormerlySerializedAs("SmallBullet")] public GameObject smallBullet;
    [FormerlySerializedAs("BigBullet")] public GameObject bigBullet;
    [FormerlySerializedAs("VFX_fire")] public GameObject vfxFire;
    [FormerlySerializedAs("VFX_fireShow")] public Transform vfxFireShow;
    public GameObject stvoli;
    [FormerlySerializedAs("VFX_fire_small")] public Transform vfxFireSmall;
    [FormerlySerializedAs("VFX_fire_small_normPos")] public Transform vfxFireSmallNormPos;
    [FormerlySerializedAs("VFX_fire_small_hidePos")] public Transform vfxFireSmallHidePos;
    public Transform spawnBullet;
    public Transform syncIdleTr;
    public Transform syncAimedTr;

    [Header("Shooting Settings")]
    public int smallDamage;
    [HideInInspector]
    public float bigDamage;
    public int bigDamageMaximum;
    public float holdTime = 3f;
    public float shootForce;
    public float shootBigForce;
    public float idlespread;
    public float aimspread;
    public float aimSpeed = 5;
    public float smallRecoilCam = 0.8f;
    public float bigRecoilCam = 1.5f;
    [FormerlySerializedAs("BlinkDelay")] public float blinkDelay = 1.5f;
    public float delayAfterBigShot;
    public float delayAfterSmallShot;
    private float _multiplier = 0.1f;
    private float _multiplierStep;
    private bool _isAiming = false;
    private bool _isBlinking = false;
    private bool _isHoldingLkm = false;
    private bool _isVisibleBlink = true;
    private CameraController _cameraController => CameraController.Instance;
    float _delay = 0;
    float _delayBigstvol = 0;
    private Animator _anim;
    int _currentstvol = 1;//0-small, 1-big

    private GameObject _player;
    private PlayerMovement _plmove;
    private RecoilController _recoilController;
    void Start()
    {
        _multiplierStep = (1 - _multiplier) * 0.1f / holdTime;
        _recoilController = GetComponent<RecoilController>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _anim = GetComponent<Animator>();
        StartCoroutine(Delay());
        StartCoroutine(BlinkBigStvol());
        StartCoroutine(DELAY_BIG_STVOL());
        vfxFire.SetActive(false);
        _plmove = _player.GetComponent<PlayerMovement>();

    }
    private void Shoot()
    {

        if (_currentstvol == 1 && _delayBigstvol <= 0)
        {
            bigDamage = bigDamageMaximum * _multiplier;
            _multiplier = 0.1f;
            _isHoldingLkm = false;
            _isBlinking = false;
            _isVisibleBlink = true;
            float x, y;
            if (_isAiming)
            {
                x = Random.Range(-aimspread, aimspread);
                y = Random.Range(-aimspread, aimspread);
            }
            else
            {
                x = Random.Range(-idlespread, idlespread);
                y = Random.Range(-idlespread, idlespread);
            }
            Vector3 spreadVector = new Vector3(x, y, 0);
            Vector3 dirwithspread = spawnBullet.forward + spreadVector;
            // Создаем пулю и задаем её начальную ориентацию как у объекта spawnBullet
            GameObject currentBullet = Instantiate(bigBullet, spawnBullet.position, spawnBullet.rotation);
            currentBullet.GetComponent<Rigidbody>().AddForce(dirwithspread.normalized * shootBigForce, ForceMode.Impulse);
            _anim.SetTrigger("DecreaseEmis");
            _cameraController.recoilStrength = bigRecoilCam;
            _cameraController.ApplyRecoil();
            _recoilController.AddRecoil(10);
            _delayBigstvol = delayAfterBigShot;
        }
        if (_currentstvol == 0 && _delay <= 0)
        {
           
            vfxFireSmall.position = vfxFireSmallNormPos.position;
            float x, y;
            if(_isAiming)
            {
                x = Random.Range(-aimspread, aimspread);
                y = Random.Range(-aimspread, aimspread);
            }
            else
            {
                x = Random.Range(-idlespread, idlespread);
                y = Random.Range(-idlespread, idlespread);
            }
           
            Vector3 spreadVector = new Vector3(x, y, 0);
            Vector3 dirwithspread = spawnBullet.forward + spreadVector;
            // Создаем пулю и задаем её начальную ориентацию как у объекта spawnBullet
            GameObject currentBullet = Instantiate(smallBullet, spawnBullet.position, spawnBullet.rotation);
            currentBullet.GetComponent<Rigidbody>().AddForce(dirwithspread.normalized * shootForce, ForceMode.Impulse);
            _delay = delayAfterSmallShot;
            _cameraController.recoilStrength = smallRecoilCam;
            _cameraController.ApplyRecoil();
            _recoilController.AddRecoil(2);
            //plmove.currentRecoil += recoilSmallAmount;
        }


    }
 

    public IEnumerator Delay()
    {
        while (true)
        {
            _delay -= 0.1f;
            if (_delay <= delayAfterSmallShot/2)
            {
                vfxFireSmall.position = vfxFireSmallHidePos.position;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator DELAY_BIG_STVOL()
    {
        while (true)
        {
           if(_currentstvol==1 && _isHoldingLkm)
            { 
                if(_multiplier<1)
                {
                    _multiplier += _multiplierStep;
                }
                if(_multiplier>=1)
                {
                    _isBlinking = true;
                }
            }
           
            
            _delayBigstvol -= 0.1f;
           
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator BlinkBigStvol()
    {
        while (true)
        {
            if(_isBlinking)
            {
                if(_isVisibleBlink)
                {
                    _isVisibleBlink = false;
                    vfxFire.transform.position = vfxFireSmallHidePos.position;
                }
                else
                {
                    _isVisibleBlink = true;
                    vfxFire.transform.position = vfxFireShow.position;
                }
            }
            yield return new WaitForSeconds(blinkDelay);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_cameraController.isPaused)
        {

            if (Input.GetMouseButtonDown(0) && _currentstvol == 0)
            {
                Shoot();

            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    if (_currentstvol == 0 && _delay <= 0)
                    {
                        Shoot();
                    }
                    else if (_currentstvol == 1 && _delayBigstvol <= 0)
                    {
                        vfxFire.SetActive(true);
                        _isHoldingLkm = true;
                    }
                }

                else
                {
                    if (_isHoldingLkm && _currentstvol == 1)
                    {
                        Shoot();
                    }
                    _isHoldingLkm = false;
                    vfxFire.transform.position = vfxFireShow.position;
                    vfxFire.SetActive(false);
                    _multiplier = 0.1f;
                }
            }
            if (Input.GetMouseButton(1))
            {
                _isAiming = true;
                transform.position = Vector3.Lerp(transform.position, syncAimedTr.position, Time.deltaTime * aimSpeed);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, syncIdleTr.position, Time.deltaTime * aimSpeed);
                _isAiming = false;
            }

            if (Input.GetButtonDown("SwitchStvol") && !_isHoldingLkm)
            {
                _anim.SetTrigger("Change");
                _delay = 0.5f;
                _currentstvol = Mathf.Abs(_currentstvol - 1);

            }
        }
    }

}
   
 

