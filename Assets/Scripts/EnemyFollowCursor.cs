using System.Collections;
using UnityEngine;

public class EnemyFollowCursor : MonoBehaviour
{
    public Camera mainCamera; // Ссылка на основную камеру
    public float distance = 5f; // Расстояние от камеры до куба
    private float moveSpeed = 5f; // Скорость перемещения куба
    public float MaxmoveSpeed = 5f; // Скорость перемещения куба
    public float MoveThreshold;
    private Vector3 previousPosition;
    private Animator animator;
    private int AttackDealy;
    private bool IsAttackReady = true;
    private bool IsSppedUp = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
      
        StartCoroutine(SpedUPP());

    }
    IEnumerator Delaay()
    {

        yield return new WaitForSeconds(AttackDealy);
        IsAttackReady = true;
        
    }
    IEnumerator SpedUPP()
    {
        while(true) { 
        yield return new WaitForSeconds(0.05f);
            if (moveSpeed < MaxmoveSpeed && IsSppedUp)
            {
                moveSpeed += 1;
            }
        }
    }
    private void CheckIsMoving()
    {
        Vector3 currentPosition = transform.position;
        if (Vector3.Distance(currentPosition, previousPosition) > MoveThreshold)
        {
            animator.SetBool("Riding", true);
            IsSppedUp=true;
        }
        else
        {
            moveSpeed = 1;
            IsSppedUp = false;
            if (IsAttackReady)
            {
                animator.SetTrigger("Attack");
                AttackDealy = Random.Range(3, 10);
                IsAttackReady= false;
                StartCoroutine(Delaay());
            }
            animator.SetBool("Riding", false);

        }
        previousPosition = currentPosition;
    }
    void Update()
    {
        CheckIsMoving();
        // Получаем позицию курсора на экране
        Vector3 mousePosition = Input.mousePosition;

        // Преобразуем экранные координаты в мировые
        mousePosition.z = distance; // Указываем расстояние до куба
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        // Определяем новую позицию куба
        Vector3 targetPosition = new Vector3(worldPosition.x, transform.position.y, transform.position.z);

        // Плавно перемещаем куб к новой позиции
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
