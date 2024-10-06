using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public float speed = 5.0f;
    public float jumpHeight = 1.3f;
    public float gravity = -9.81f;
    public float dashSpeed = 20.0f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1.0f;
    public bool isGrounded;
    [FormerlySerializedAs("HP_TXT")] [Header("HP")]
    public TextMeshProUGUI hpTxt;
    [FormerlySerializedAs("HP")] public float hp = 100f;
    [FormerlySerializedAs("maxHP")] public float maxHp = 100f;
    public float regenAmount = 5f;
    public float regenInterval = 1f;
    public float regenDelay = 5f;

    private float _lastDamageTime;
    private bool _isRegenerating;
    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _isDashing = false;
    private float _lastDashTime;
    private CameraController _cameraController => CameraController.Instance;
    void Start()
    {
        _lastDamageTime = Time.time;
        _isRegenerating = false;
        _controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        if (!_cameraController.isPaused)
        {
            if (Input.GetButtonDown("Use"))
            {
                // Испускаем Raycast
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit,_cameraController.raycastDistance, ~_cameraController.ignoreLayer))
                {
                    
                    // Проверяем, попадает ли объект с именем "DoorInteracted"
                    if (hit.transform != null && hit.transform.tag == "DoorInteracted")
                    {
                        // Выводим сообщение в консоль
                        hit.transform.gameObject.GetComponentInParent<Door>().InteractWithDoor();

                    }
                }
            }

            isGrounded = _controller.isGrounded;
            if (isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
            if (!_isDashing)
            {
                float moveX = Input.GetAxis("Horizontal");
                float moveZ = Input.GetAxis("Vertical");
                Vector3 move = transform.right * moveX + transform.forward * moveZ;
                _controller.Move(move * speed * Time.deltaTime);
                if (Input.GetButtonDown("Jump") && isGrounded)
                {
                    _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }
                if (Input.GetKeyDown(KeyCode.LeftShift) && (moveX != 0 || moveZ != 0) && Time.time >= _lastDashTime + dashCooldown)
                {
                    StartCoroutine(Dash(move));
                }
            }
            _velocity.y += gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
            if (Time.time - _lastDamageTime >= regenDelay && !_isRegenerating)
            {
                StartCoroutine(RegenerateHp());
            }
            hpTxt.text = "[ " + hp + " ]";
        }

    }
    public void Fall()
    {

    }
    public void TakeDamaage(float damage)
    {
        hp -= damage;
        _lastDamageTime = Time.time;
        if (_isRegenerating)
        {
            StopCoroutine(RegenerateHp());
            _isRegenerating = false;
        }
    }
    private IEnumerator Dash(Vector3 direction)
    {
        _isDashing = true;
        _lastDashTime = Time.time; // Обновляем время последнего рывка
        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            _controller.Move(direction * dashSpeed * Time.deltaTime);
            yield return null;
        }
        _isDashing = false;
    }
    private IEnumerator RegenerateHp()
    {
        _isRegenerating = true;
        while (hp < maxHp && Time.time - _lastDamageTime >= regenDelay)
        {
            hp += regenAmount;
            if (hp > maxHp) hp = maxHp;
            yield return new WaitForSeconds(regenInterval);
        }
        _isRegenerating = false;
    }
}
