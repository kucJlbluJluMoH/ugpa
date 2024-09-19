using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    private float Smalldamage = 10;
    private float Bigdamage = 50;
    public bool IsSmallBullet = true;
    private int counterEnemies = 0;
    private BlasterController blasterController;
    private MeshRenderer MeshRenderer;
    private Light Light;
    private void Start()
    {
        MeshRenderer = GetComponent<MeshRenderer>();
        Light= GetComponentInChildren<Light>(); 
        blasterController = GameObject.FindGameObjectWithTag("Blaster").GetComponent<BlasterController>();
        Smalldamage = blasterController.smallDamage;
        Bigdamage = blasterController.bigDamage;
        StartCoroutine(HideInFirstSec());
        MeshRenderer.enabled = false;
        Light.enabled = false;
    }
    private void Awake()
    {

        Destroy(gameObject, 2);
        
    }
    IEnumerator HideInFirstSec()
    {
        yield return new WaitForSeconds(0.05f);
        MeshRenderer.enabled = true;
        Light.enabled = true;
        StopAllCoroutines();
    }
    private void Update()
    {
        

    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag!="Player" && collision.gameObject.tag !="Blaster")
        {
            if (collision.gameObject.tag == "Enemy")
            {
                if(IsSmallBullet)
                {
                    collision.gameObject.GetComponent<EnemyController>().TakeDamageEnemy(Smalldamage); 
                }
                else
                {
                    collision.gameObject.GetComponent<EnemyController>().TakeDamageEnemy(Bigdamage);
                }
            }
            else
            {
                if (IsSmallBullet) 
                { 
                    Destroy(gameObject);
                }
            }



        }


       

    }
    
    
}
