using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyFollowCursor : MonoBehaviour
{
    public Camera mainCamera; // ������ �� �������� ������
    public float distance = 5f; // ���������� �� ������ �� ����
    private float _moveSpeed = 5f; // �������� ����������� ����
    [FormerlySerializedAs("MaxmoveSpeed")] public float maxmoveSpeed = 5f; // �������� ����������� ����
    [FormerlySerializedAs("MoveThreshold")] public float moveThreshold;
    private Vector3 _previousPosition;
    private Animator _animator;
    private int _attackDealy;
    private bool _isAttackReady = true;
    private bool _isSppedUp = false;
    private void Start()
    {
        _animator = GetComponent<Animator>();
      
        StartCoroutine(SpedUpp());

    }
    IEnumerator Delaay()
    {

        yield return new WaitForSeconds(_attackDealy);
        _isAttackReady = true;
        
    }
    IEnumerator SpedUpp()
    {
        while(true) { 
        yield return new WaitForSeconds(0.05f);
            if (_moveSpeed < maxmoveSpeed && _isSppedUp)
            {
                _moveSpeed += 1;
            }
        }
    }
    private void CheckIsMoving()
    {
        Vector3 currentPosition = transform.position;
        if (Vector3.Distance(currentPosition, _previousPosition) > moveThreshold)
        {
            _animator.SetBool("Riding", true);
            _isSppedUp=true;
        }
        else
        {
            _moveSpeed = 1;
            _isSppedUp = false;
            if (_isAttackReady)
            {
                _animator.SetTrigger("Attack");
                _attackDealy = Random.Range(3, 10);
                _isAttackReady= false;
                StartCoroutine(Delaay());
            }
            _animator.SetBool("Riding", false);

        }
        _previousPosition = currentPosition;
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
        transform.position = Vector3.Lerp(transform.position, targetPosition, _moveSpeed * Time.deltaTime);
    }
}
