using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private GameObject player;
    private Rigidbody _rigidbody;
    
    [SerializeField] private bool invincible = false;
    
    //LANE SWITCHING
    private int currentLane = 0;
    private float laneDistance = 3.5f;
    private Vector2 movementInput;
    
    
    
    //DUCKING
    private Vector3 originalScale;
    private float duckDuration = 0.5f;
    private float scaleDuration = 0.1f;

    
    //JUMPING
    public float jumpHeight = 5.0f;
    public float jumpDuration = 0.5f;
    private Vector3 startingPosition;
    private float jumpStartTime;
    private bool isJumping = false;
    private bool rotateSide = false;
    private bool rotateFlip = false;
    
    public float floatFrequency = 3f;
    public float floatAmplitude = 0.08f;
    
    private float originalY;
    
    //Audio
    private AudioSource audioSource;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        originalScale = player.transform.localScale;
        originalY = transform.position.y;
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        duckDuration = 0.5f / (GameManager.instance.GetPlayerSpeedModifier());
        jumpDuration = 0.7f / (GameManager.instance.GetPlayerSpeedModifier());
        
        if (isJumping)
        {
            UpdateJump();
        }
        else
        {
            float newY = originalY + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
    
    void OnMovement(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
        
        if(movementInput.x > 0.5f)
        {
            SwitchLane(1);
        }
        else if(movementInput.x < -0.5f)
        {
            SwitchLane(-1);
        }
        
    }
    
    void OnJump(InputValue inputValue)
    {
        // if (rigidbody != null)
        // {
        //     StartCoroutine(ScaleOverTime(player.transform, originalScale, scaleDuration));
        //     // rigidbody.AddForce(Vector3.up * 8, ForceMode.Impulse);
        //     
        // }
        
        if (isJumping) return;
        
        originalY = transform.position.y;
        startingPosition = transform.position;
        jumpStartTime = Time.time;
        isJumping = true;
        rotateFlip = Random.value > 0.5f;
        rotateSide = Random.value > 0.5f;
    }

    void OnPause() => GameManager.instance.PauseGame();

    void UpdateJump()
    {
        float elapsed = Time.time - jumpStartTime;
        float t = Mathf.Clamp01(elapsed / jumpDuration);
        
        float yOffset = Mathf.Lerp(0, jumpHeight, -4 * t * (t - 1)); 

        float rotationAngleSide;
        if (rotateSide)
        {
            rotationAngleSide = Mathf.Lerp(0, 360, t);
        }
        else
        {
            rotationAngleSide = Mathf.Lerp(0, -360, t);
        }
        
        transform.position = new Vector3(transform.position.x, startingPosition.y + yOffset, transform.position.z);
        
        if (rotateFlip)
        {
            transform.rotation = Quaternion.Euler(Mathf.Lerp(0, 360, t), 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, rotationAngleSide, 0);
        }

        if (t >= 1.0f)
        {
            isJumping = false;
        }
    }
    
    void OnDuck(InputValue inputValue)
    {
        if (_rigidbody)
        {
            // rigidbody.AddForce(Vector3.down * 8, ForceMode.Impulse);
            
            StartCoroutine(DuckAndRevert());
        }
    }
    
    private IEnumerator ScaleOverTime(Transform targetTransform, Vector3 targetScale, float duration)
    {
        Vector3 initialScale = targetTransform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            targetTransform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetTransform.localScale = targetScale;
    }
    
    private IEnumerator DuckAndRevert()
    {
        Vector3 duckedScale = new Vector3(0.2f, 0.2f, 0.2f);
        yield return StartCoroutine(ScaleOverTime(player.transform, duckedScale, scaleDuration));

        yield return new WaitForSeconds(duckDuration);

        yield return StartCoroutine(ScaleOverTime(player.transform, originalScale, scaleDuration));
    }
    
    private Coroutine _laneChangeCoroutine;

    private void SwitchLane(int direction)
    {
        int targetLane = Mathf.Clamp(currentLane + direction, -1, 1);
        
        if (targetLane != currentLane)
        {
            currentLane = targetLane;
            Vector3 targetPosition = new Vector3(currentLane * laneDistance, transform.position.y, transform.position.z);
            
            if (_laneChangeCoroutine != null)
            {
                StopCoroutine(_laneChangeCoroutine);
            }
            
            _laneChangeCoroutine = StartCoroutine(SmoothLaneChange(targetPosition));
        }
    }

    private IEnumerator SmoothLaneChange(Vector3 targetPosition)
    {
        float duration = 0.15f; 
        float elapsed = 0f;

        Vector3 startingPosition = transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float time = Mathf.Clamp01(elapsed / duration);
            
            transform.position = new Vector3(
                Mathf.Lerp(startingPosition.x, targetPosition.x, time),
                transform.position.y, 
                Mathf.Lerp(startingPosition.z, targetPosition.z, time)
            );

            yield return null;
        }
        
        transform.position = targetPosition;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (!invincible)
            {
                GameManager.instance.DecreaseHealth(1);
            }
        }
        if (other.gameObject.CompareTag("Dot"))
        {
            audioSource.Play();
            GameManager.instance.IncreaseCollectedCoins(1);
        }
    }
    
}
