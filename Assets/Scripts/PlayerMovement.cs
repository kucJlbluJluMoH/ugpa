using System.Collections;
using TMPro;
using UnityEngine;
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
    [Header("HP")]
    public TextMeshProUGUI HP_TXT;
    public float HP = 100f;
    public float maxHP = 100f;
    public float regenAmount = 5f;
    public float regenInterval = 1f;
    public float regenDelay = 5f;

    private float lastDamageTime;
    private bool isRegenerating;
    private CharacterController controller;
    private Vector3 velocity;
    private bool isDashing = false;
    private float lastDashTime;
    private Transform cameraTransform;
    private CameraController cameraController;
    void Start()
    {
        lastDamageTime = Time.time;
        isRegenerating = false;
        controller = GetComponent<CharacterController>();
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }
    void Update()
    {
        if (!cameraController.IsPaused)
        {
            if (Input.GetButtonDown("Use"))
            {
                // Испускаем Raycast
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit,cameraController.raycastDistance, ~cameraController.ignoreLayer))
                {
                    
                    // Проверяем, попадает ли объект с именем "DoorInteracted"
                    if (hit.transform != null && hit.transform.tag == "DoorInteracted")
                    {
                        // Выводим сообщение в консоль
                        hit.transform.gameObject.GetComponentInParent<Door>().InteractWithDoor();

                    }
                }
            }

            isGrounded = controller.isGrounded;
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
            if (!isDashing)
            {
                float moveX = Input.GetAxis("Horizontal");
                float moveZ = Input.GetAxis("Vertical");
                Vector3 move = transform.right * moveX + transform.forward * moveZ;
                controller.Move(move * speed * Time.deltaTime);
                if (Input.GetButtonDown("Jump") && isGrounded)
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }
                if (Input.GetKeyDown(KeyCode.LeftShift) && (moveX != 0 || moveZ != 0) && Time.time >= lastDashTime + dashCooldown)
                {
                    StartCoroutine(Dash(move));
                }
            }
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
            if (Time.time - lastDamageTime >= regenDelay && !isRegenerating)
            {
                StartCoroutine(RegenerateHP());
            }
            HP_TXT.text = "[ " + HP + " ]";
        }

    }
    public void Fall()
    {

    }
    public void TakeDamaage(float damage)
    {
        HP -= damage;
        lastDamageTime = Time.time;
        if (isRegenerating)
        {
            StopCoroutine(RegenerateHP());
            isRegenerating = false;
        }
    }
    private IEnumerator Dash(Vector3 direction)
    {
        isDashing = true;
        lastDashTime = Time.time; // Обновляем время последнего рывка
        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            controller.Move(direction * dashSpeed * Time.deltaTime);
            yield return null;
        }
        isDashing = false;
    }
    private IEnumerator RegenerateHP()
    {
        isRegenerating = true;
        while (HP < maxHP && Time.time - lastDamageTime >= regenDelay)
        {
            HP += regenAmount;
            if (HP > maxHP) HP = maxHP;
            yield return new WaitForSeconds(regenInterval);
        }
        isRegenerating = false;
    }
}
