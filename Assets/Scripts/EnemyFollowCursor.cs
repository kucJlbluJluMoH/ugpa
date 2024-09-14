using System.Collections;
using UnityEngine;

public class EnemyFollowCursor : MonoBehaviour
{
    public Camera mainCamera; // ������ �� �������� ������
    public float distance = 5f; // ���������� �� ������ �� ����
    private float moveSpeed = 5f; // �������� ����������� ����
    public float MaxmoveSpeed = 5f; // �������� ����������� ����
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
        // �������� ������� ������� �� ������
        Vector3 mousePosition = Input.mousePosition;

        // ����������� �������� ���������� � �������
        mousePosition.z = distance; // ��������� ���������� �� ����
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        // ���������� ����� ������� ����
        Vector3 targetPosition = new Vector3(worldPosition.x, transform.position.y, transform.position.z);

        // ������ ���������� ��� � ����� �������
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
