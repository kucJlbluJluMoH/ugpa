using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class BlasterLobby : MonoBehaviour

{
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

    private RecoilController recoilController;
    private int AttackDealy;
    private bool isReadyToAttack = true;
    private int currentstvol;

   IEnumerator CountDelay()
    {
        yield return  new WaitForSeconds(AttackDealy);
        isReadyToAttack = true;
    }
    private void Shoot()
    {
        CountDelay();
        currentstvol = Random.Range(0, 2);
        Debug.Log("123123");
        if (currentstvol == 1)
        {
            float x, y;
     
            x = Random.Range(-1, 1);
            y = Random.Range(-1, 1);
            
            Vector3 spreadVector = new Vector3(x, y, 0);
            Vector3 dirwithspread = spawnBullet.forward + spreadVector;
            // Создаем пулю и задаем её начальную ориентацию как у объекта spawnBullet
            GameObject currentBullet = Instantiate(BigBullet, spawnBullet.position, spawnBullet.rotation);
            currentBullet.GetComponent<Rigidbody>().AddForce(dirwithspread.normalized * 2000, ForceMode.Impulse);
            recoilController.AddRecoil(10);

        }
        if (currentstvol == 0)
        {

            VFX_fire_small.position = VFX_fire_small_normPos.position;
            float x, y;

            x = Random.Range(-1, 1);
            y = Random.Range(-1, 1);
            
            Vector3 spreadVector = new Vector3(x, y, 0);
            Vector3 dirwithspread = spawnBullet.forward + spreadVector;
            // Создаем пулю и задаем её начальную ориентацию как у объекта spawnBullet
            GameObject currentBullet = Instantiate(SmallBullet, spawnBullet.position, spawnBullet.rotation);
            currentBullet.GetComponent<Rigidbody>().AddForce(dirwithspread.normalized * 1000, ForceMode.Impulse);
            recoilController.AddRecoil(2);
            //plmove.currentRecoil += recoilSmallAmount;
        }


    }

    private void Start()
    {
        recoilController = GetComponent<RecoilController>();
    }
    void Update()
    {
        if(isReadyToAttack)
        {
            isReadyToAttack = false;
            AttackDealy = Random.Range(3, 10);
            Shoot();
        }
        
    }
}
