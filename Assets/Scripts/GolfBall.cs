using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfBall : MonoBehaviour
{
    private enum PlayerMode { Golf, Walk }

    [Header("References")]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private GameObject goalFx;

    [Header("Audio")]
    [SerializeField] private AudioSource swingAudio;
    [SerializeField] private AudioSource holeAudio;
    [SerializeField] private AudioSource splashAudio;
    [SerializeField] private AudioSource impactAudio;
    [SerializeField] private float impactSpeedThreshold = 3f;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite golfSprite;
    [SerializeField] private Sprite walkSprite;

    [Header("Attributes")]
    [SerializeField] private float maxPower = 10f;
    [SerializeField] private float power = 2f;
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float maxGoalSpeed = 8f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeedMultiplier = 300f;

    private bool isDragging;
    private bool inHole;
    private float walkInput = 0f;
    private bool isPaused = false;

    private PlayerMode currentMode = PlayerMode.Golf;

    private void Update()
    {
        if (inHole) return;

        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     isPaused = !isPaused;
        //     if (isPaused)
        //     {
        //         LevelManager.main.Pause();
        //     }
        //     else
        //     {
        //         LevelManager.main.UnPause();
        //     }
        // }
        //
        // if (isPaused) return;

        if (currentMode == PlayerMode.Walk)
        {
            HandleWalkInput();
        }

        if (isReady() && Input.GetKeyDown(KeyCode.Space))
        {
            ToggleMode();
        }

        if (currentMode == PlayerMode.Golf && isReady())
        {
            HandleGolfInput();
        }

        if (LevelManager.main.outOfStrokes && rigidBody.velocity.magnitude <= 0.01f && !LevelManager.main.levelCompleted)
        {
            LevelManager.main.LevelComplete();
        }
    }

    private void FixedUpdate()
    {
        if (currentMode == PlayerMode.Walk)
        {
            rigidBody.velocity = new Vector2(walkInput * walkSpeed, rigidBody.velocity.y);

            if (walkInput != 0)
            {
                spriteRenderer.flipX = walkInput < 0;
            }
        }

        // Apply rotation when in Golf mode
        if (currentMode == PlayerMode.Golf && rigidBody.velocity.magnitude > 0.05f)
        {
            float rotationAmount = -rigidBody.velocity.x * rotationSpeedMultiplier * Time.fixedDeltaTime;
            transform.Rotate(0, 0, rotationAmount);
        }
    }

    private void ToggleMode()
    {
        if (currentMode == PlayerMode.Golf)
        {
            currentMode = PlayerMode.Walk;
            lineRenderer.positionCount = 0;

            if (walkSprite != null)
                spriteRenderer.sprite = walkSprite;

            // Reset rotation
            transform.rotation = Quaternion.identity;
        }
        else
        {
            currentMode = PlayerMode.Golf;

            if (golfSprite != null)
                spriteRenderer.sprite = golfSprite;
        }
    }

    private bool isReady()
    {
        return rigidBody.velocity.magnitude <= 0.2f && !inHole;
    }

    private void HandleGolfInput()
    {
        Vector2 inputPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(transform.position, inputPosition);

        if (Input.GetMouseButtonDown(0) && distance < 0.5f && !inHole)
        {
            DragStart(inputPosition);
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            DragChange(inputPosition);
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            DragRelease(inputPosition);
        }
    }

    private void DragStart(Vector2 inputPosition)
    {
        isDragging = true;
        lineRenderer.positionCount = 2;
    }

    private void DragChange(Vector2 position)
    {
        Vector2 direction = (Vector2)transform.position - position;
        Vector2 clampedDirection = Vector2.ClampMagnitude(direction * power, maxPower);

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, (Vector2)transform.position + clampedDirection);
    }

    private void DragRelease(Vector2 position)
    {
        float distance = Vector2.Distance(transform.position, position);
        isDragging = false;
        lineRenderer.positionCount = 0;

        if (distance < 0.5f)
        {
            return;
        }
        swingAudio.Play();
        LevelManager.main.Stroke();

        Vector2 direction = (Vector2)transform.position - position;
        rigidBody.velocity = Vector2.ClampMagnitude(direction * power, maxPower);
    }

    private void HandleWalkInput()
    {
        walkInput = 0f;

        if (Input.GetKey(KeyCode.A))
            walkInput = -1f;
        else if (Input.GetKey(KeyCode.D))
            walkInput = 1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Goal")
        {
            if (inHole) return;

            if (rigidBody.velocity.magnitude <= maxGoalSpeed)
            {
                inHole = true;
                rigidBody.velocity = Vector2.zero;
                GameObject fx = Instantiate(goalFx, transform.position, Quaternion.Euler(0, 0, 45));
                Destroy(fx, 2f);
                holeAudio.Play();

                StartCoroutine(DelayLevelComplete());
            }
        } else if (collision.tag == "Water")
        {
            splashAudio.Play();
            LevelManager.main.Stroke();
            // Reset velocity
            rigidBody.velocity = Vector2.zero;
            rigidBody.angularVelocity = 0f;

            // Reset position to the starting point
            transform.parent.position = new Vector2(-12.25f, -2f);

            // Reset rotation (optional but recommended)
            transform.rotation = Quaternion.identity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impactSpeed = collision.relativeVelocity.magnitude;
        if (impactSpeed >= impactSpeedThreshold)
        {
            impactAudio.Play();
        }
    }

    private IEnumerator DelayLevelComplete()
    {
        yield return new WaitForSeconds(1.5f);
        LevelManager.main.LevelComplete();
    }
}