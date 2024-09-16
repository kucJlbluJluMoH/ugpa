using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class BlasterController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject SmallBullet;
    public GameObject BigBullet;
    public GameObject VFX_fire;
    public Transform VFX_fireShow;
    public GameObject stvoli;
    public Transform VFX_fire_small;
    public Transform VFX_fire_small_normPos;
    public Transform VFX_fire_small_hidePos;
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
    public float BlinkDelay = 1.5f;
    public float delayAfterBigShot;
    public float delayAfterSmallShot;

    private float multiplier = 0.1f;
    private float multiplierStep;
    private bool isAiming = false;
    private bool isBlinking = false;
    private bool isHoldingLKM = false;
    private bool isVisibleBlink = true;
    private CameraController cameraController;
    float delay = 0;
    float delayBIGSTVOL = 0;
    private Animator anim;
    int currentstvol = 0;//0-small, 1-big

    private GameObject Player;
    private PlayerMovement plmove;
    private RecoilController recoilController;
    void Start()
    {
        multiplierStep = (1 - multiplier) * 0.1f / holdTime;
        recoilController = GetComponent<RecoilController>();
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        Player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        StartCoroutine(DELAY());
        StartCoroutine(BlinkBigStvol());
        StartCoroutine(DELAY_BIG_STVOL());
        VFX_fire.SetActive(false);
        plmove = Player.GetComponent<PlayerMovement>();

    }
    private void Shoot()
    {

        if (currentstvol == 1 && delayBIGSTVOL <= 0)
        {
            bigDamage = bigDamageMaximum * multiplier;
            multiplier = 0.1f;
            isHoldingLKM = false;
            isBlinking = false;
            isVisibleBlink = true;
            float x, y;
            if (isAiming)
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
            GameObject currentBullet = Instantiate(BigBullet, spawnBullet.position, spawnBullet.rotation);
            currentBullet.GetComponent<Rigidbody>().AddForce(dirwithspread.normalized * shootBigForce, ForceMode.Impulse);
            anim.SetTrigger("DecreaseEmis");
            cameraController.recoilStrength = bigRecoilCam;
            cameraController.ApplyRecoil();
            recoilController.AddRecoil(10);
            delayBIGSTVOL = delayAfterBigShot;
        }
        if (currentstvol == 0 && delay <= 0)
        {
           
            VFX_fire_small.position = VFX_fire_small_normPos.position;
            float x, y;
            if(isAiming)
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
            GameObject currentBullet = Instantiate(SmallBullet, spawnBullet.position, spawnBullet.rotation);
            currentBullet.GetComponent<Rigidbody>().AddForce(dirwithspread.normalized * shootForce, ForceMode.Impulse);
            delay = delayAfterSmallShot;
            cameraController.recoilStrength = smallRecoilCam;
            cameraController.ApplyRecoil();
            recoilController.AddRecoil(2);
            //plmove.currentRecoil += recoilSmallAmount;
        }


    }
 

    public IEnumerator DELAY()
    {
        while (true)
        {
            delay -= 0.1f;
            if (delay <= delayAfterSmallShot/2)
            {
                VFX_fire_small.position = VFX_fire_small_hidePos.position;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator DELAY_BIG_STVOL()
    {
        while (true)
        {
           if(currentstvol==1 && isHoldingLKM)
            { 
                if(multiplier<1)
                {
                    multiplier += multiplierStep;
                }
                if(multiplier>=1)
                {
                    isBlinking = true;
                }
            }
           
            
            delayBIGSTVOL -= 0.1f;
           
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator BlinkBigStvol()
    {
        while (true)
        {
            if(isBlinking)
            {
                if(isVisibleBlink)
                {
                    isVisibleBlink = false;
                    VFX_fire.transform.position = VFX_fire_small_hidePos.position;
                }
                else
                {
                    isVisibleBlink = true;
                    VFX_fire.transform.position = VFX_fireShow.position;
                }
            }
            yield return new WaitForSeconds(BlinkDelay);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!cameraController.IsPaused)
        {

            if (Input.GetMouseButtonDown(0) && currentstvol == 0)
            {
                Shoot();

            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    if (currentstvol == 0 && delay <= 0)
                    {
                        Shoot();
                    }
                    else if (currentstvol == 1 && delayBIGSTVOL <= 0)
                    {
                        VFX_fire.SetActive(true);
                        isHoldingLKM = true;
                    }
                }

                else
                {
                    if (isHoldingLKM && currentstvol == 1)
                    {
                        Shoot();
                    }
                    isHoldingLKM = false;
                    VFX_fire.transform.position = VFX_fireShow.position;
                    VFX_fire.SetActive(false);
                    multiplier = 0.1f;
                }
            }
            if (Input.GetMouseButton(1))
            {
                isAiming = true;
                transform.position = Vector3.Lerp(transform.position, syncAimedTr.position, Time.deltaTime * aimSpeed);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, syncIdleTr.position, Time.deltaTime * aimSpeed);
                isAiming = false;
            }

            if (Input.GetButtonDown("SwitchStvol") && !isHoldingLKM)
            {
                anim.SetTrigger("Change");
                delay = 0.5f;
                currentstvol = Mathf.Abs(currentstvol - 1);

            }
        }
    }

}
   
 

