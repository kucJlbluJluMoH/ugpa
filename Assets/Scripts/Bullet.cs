using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    private float _smalldamage = 10;
    private float _bigdamage = 50;
    [FormerlySerializedAs("IsSmallBullet")] public bool isSmallBullet = true;
    private int _counterEnemies = 0;
    private BlasterController _blasterController;
    private MeshRenderer _meshRenderer;
    private Light _light;
    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _light= GetComponentInChildren<Light>(); 
        _blasterController = GameObject.FindGameObjectWithTag("Blaster").GetComponent<BlasterController>();
        _smalldamage = _blasterController.smallDamage;
        _bigdamage = _blasterController.bigDamage;
        StartCoroutine(HideInFirstSec());
        _meshRenderer.enabled = false;
        _light.enabled = false;
    }
    private void Awake()
    {

        Destroy(gameObject, 2);
        
    }
    IEnumerator HideInFirstSec()
    {
        yield return new WaitForSeconds(0.05f);
        _meshRenderer.enabled = true;
        _light.enabled = true;
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
                if(isSmallBullet)
                {
                    collision.gameObject.GetComponent<EnemyController>().TakeDamageEnemy(_smalldamage); 
                }
                else
                {
                    collision.gameObject.GetComponent<EnemyController>().TakeDamageEnemy(_bigdamage);
                }
            }
            else
            {
                if (isSmallBullet) 
                { 
                    Destroy(gameObject);
                }
            }



        }


       

    }
    
    
}
