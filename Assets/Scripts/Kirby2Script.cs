using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;


public class Kirby2Script : MonoBehaviour
{
    private Rigidbody rb; 
    private float movementX;
    private float movementY;
    private int count;
    public float speed = 0; 
    public float rotationSpeed = 180;
    Vector3 m_EulerAngleVelocity;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    Animator animator;
    public float targetTime = 20.0f;
    public float coolDown = 2.0f;
    public TextMeshProUGUI timerText;
    public Transform cam;
    public AudioSource audioSource;
    Vector2 movementVector;
    public int lives;
    public bool alive;
    public bool win;

    void Start() {
        alive = true;
        rb = GetComponent <Rigidbody>();
        animator = GetComponent<Animator>();        

        count = 0;
        lives = 1;
    
        SetCountText();
        SetTimerText();
        winTextObject.SetActive(false);
    }

    void SetCountText() {
        countText.text = "Count: " + count.ToString();
        if (count >= 12) {
            winTextObject.SetActive(true);
            win = true;
        }
    }

    void OnMove (InputValue movementValue) {
        movementVector = movementValue.Get<Vector2>(); 

        movementX = movementVector.x; 
        movementY = movementVector.y; 

        var WALK = animator.GetBool("WALK");

        if (movementX != 0 || movementY != 0) {
            if (!WALK) {
                animator.SetBool("WALK", true);
            }
        } else {
            if (WALK) {
                animator.SetBool("WALK", false);
            }
        }
    }

    private void FixedUpdate() {
        if (alive == false) {
            movementVector = new Vector2(0, 0);
            return;
        }

        Vector3 movementDirection = new Vector3(movementX, 0.0f, movementY);
        if (movementDirection.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2(movementX, movementY) * Mathf.Rad2Deg + cam.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            Vector3 movement = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            //rb.AddForce(movement * speed);
            rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);
            
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("PickUp")) {
            other.gameObject.SetActive(false);
            count = count + 1;
            targetTime = targetTime + 5.0f;
            SetCountText();

            audioSource.Play();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Inimigo") && coolDown >= 2.0f) {
            coolDown = 0.0f;
            lives = lives - 1;
            // wait for 2 seconds before being hit again            

            if (lives == 0 && win == false) {
                timerEnded();
            }
        }
    }

    void Update(){

        targetTime -= Time.deltaTime;
        coolDown += Time.deltaTime;

        if (targetTime <= 0.0f && win == false)
        {
            timerEnded();
        }
        if (win == true && Input.GetKeyDown(KeyCode.Space)) {
            restartGame();
        }

        SetTimerText();
    }

    void SetTimerText()
    {
      timerText.text = "Time: " + targetTime.ToString("F1");
    }

    void timerEnded()
    {
        winTextObject.SetActive(true);
        
        winTextObject.GetComponent<TextMeshProUGUI>().color = new Color(1, 0, 0);
        winTextObject.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
        winTextObject.GetComponent<TextMeshProUGUI>().fontSize = 20;
        winTextObject.GetComponent<TextMeshProUGUI>().text = "Game Over!\n\nPress Space to Restart!";
        //gameObject.SetActive(false);
        targetTime = 0.0f;
        alive = false;
        if (alive == false && Input.GetKeyDown(KeyCode.Space)) {
            restartGame();
        }

    }

    public void restartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}