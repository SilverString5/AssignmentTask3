using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class PacStudentController : MonoBehaviour
{
    [SerializeField] private GameObject pacStudent;
    private Tweener tweener;
    [SerializeField] private Animator pacStudentAnimator;
    [SerializeField] private LayerMask wallLayerMask;
    public AudioSource pacStudentAudio, pacStudentEatPelletAudio, pacStudentCollisionSound, ghostsScaredAudio;
    public GameObject collisionParticlesPrefab;
    private string playerInput, lastInput, currentInput = null;
    private int spriteState;
    private Vector3 upState, downState, leftState, rightState;
    private bool hasCollided = false;
    private bool isFirstStop = true;

    private float teleportLeftX = -5f;
    private float teleportRightX = 21f;
    private float tunnelMinY = -6.5f;
    private float tunnelMaxY = -3.5f;

    private bool hasTeleported = false;
    private Dictionary<KeyCode, string> inputMappings;
    public bool lastInputValidandNotEqualToCurrentInput => checkInputValidity(lastInput) && currentInput != lastInput;
    public bool currentInputValid => checkInputValidity(currentInput);
    public bool currentAndLastInputNotNull => currentInput != null || lastInput != null;
    public bool wallIsUp => IsWallInDirection(Vector2.up);
    public bool wallIsLeft => IsWallInDirection(Vector2.left);
    public bool wallIsRight => IsWallInDirection(Vector2.right);
    public bool wallIsDown => IsWallInDirection(Vector2.down);

    public bool PacStudentIsWithinPortalYCoordinates =>
        transform.position.y >= tunnelMinY && transform.position.y <= tunnelMaxY;

    public bool NotInPortalXPosition =>
        transform.position.x > teleportLeftX && transform.position.x < teleportRightX;

    public GhostController[] ghosts;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        inputMappings = new Dictionary<KeyCode, string>
        {
            { KeyCode.W, "w" },
            { KeyCode.S, "s" },
            { KeyCode.D, "d" },
            { KeyCode.A, "a" },

        };
        tweener = GetComponent<Tweener>();
        StopMovement();
    }

    // Update is called once per frame
    void Update()
    {
        checkPlayerInput();
        updateLastInput();
        updateCurrentInput();
        PortalFunctionality();
        
        
    }
    //function that checks the player input //else = null
    //ex: if Input. GetKeyDown= W, then change the playerinput to d

    private void checkPlayerInput()
    {
        playerInput = null;

        foreach (var mapping in inputMappings)
        {
            if (Input.GetKeyDown(mapping.Key))
            {
                playerInput = mapping.Value;
                break;
            }
        }
    }
    
    private void updateLastInput()
    {
        if (playerInput != lastInput && playerInput != null)
            lastInput = playerInput; //changed playerInput overrides last input.
        
    }
    
    private void updateCurrentInput()
    {
        
        if (lastInputValidandNotEqualToCurrentInput)
        {
            currentInput = lastInput;
            MoveDirection();
            lastInput = null;
        }
        else if (currentInputValid)
        {
            if (!tweener.TweenExists(transform))
            {
                MoveDirection();
            }
        }
        else
        {
            StopMovement();
        }
        
        
    }

    private void MoveDirection()
    {
        
        hasCollided = false;
        Vector3 endPosition = transform.position;
        string animationName = "";
        Vector2 direction = Vector2.zero;
        
        if (currentAndLastInputNotNull)
        {
            if (currentInput == "w")
            {
                endPosition += Vector3.up;
                animationName = "PacStudentbackAn";
                direction = Vector2.up;
            }
            
            else if (currentInput == "a")
            {
                endPosition += Vector3.left;
                animationName = "PacstudentleftAn";
                direction = Vector2.left;
            }
            
            else if (currentInput == "s")
            {
                endPosition += Vector3.down;
                animationName = "PacstudentfrontAn";
                direction = Vector2.down;
            }

            else if (currentInput == "d")
            {
                endPosition += Vector3.right;
                animationName = "PacstudentrightAn";
                direction = Vector2.right;
                
            }

            if (tweener != null && !tweener.TweenExists(transform))
            {
                Debug.Log("Starting new tween");
                tweener.AddTween(transform, transform.position, endPosition, 0.3f);

                pacStudentAnimator.speed = 1;

                PlayAudio(IsPelletInDirection(direction));// <- using this to check what current audio is playing.

                if (!string.IsNullOrEmpty(animationName))
                {
                    pacStudentAnimator.CrossFade(animationName, 0.1f);
                }
                
            }
            
        }

    }
    
    
    private bool IsWallInDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, wallLayerMask);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    private void StopMovement()
    {
        pacStudentAnimator.speed = 0;
        
        if (pacStudentAudio.isPlaying)
        {
            pacStudentAudio.Pause();
        }
        if (pacStudentEatPelletAudio.isPlaying)
        {
            pacStudentEatPelletAudio.Pause();
        }

        if (isFirstStop)
        {
            isFirstStop = false;
            return;
        }

        if (!hasCollided)
        {
            hasCollided = true;
            pacStudentCollisionSound.Play();

            GameObject particles = Instantiate(collisionParticlesPrefab, transform.position, Quaternion.identity);
            Destroy(particles, 0.5f);
        }
        
        
    }
    
    private bool checkInputValidity(string value)
    {
        
        if (value == "w")
        {
            if (wallIsUp)
            { 
                return false;
            }
            return true;
        }
        if (value == "a")
        {
            if (wallIsLeft)
            {
                return false;
            }
            return true;
        }
        
        if (value == "s")
        {
            if (wallIsDown)
            {
                return false;
            }
            return true;
        }
        
        if (value == "d")
        {
            if (wallIsRight)
            {
                return false;
            }
            return true;
        }
        return false;
    }

    private bool IsPelletInDirection(Vector2 direction)
    {
        Vector2 rayOrigin = (Vector2)transform.position + direction * 1f;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, 1f); // Short ray
        
        if (hit.collider != null)
        {
            Debug.Log($"Raycast hit: {hit.collider.name} with tag {hit.collider.tag}");
            if (hit.collider.CompareTag("Valid"))
            {
                //Debug.Log("Pellet detected.");
                return true;
            }
            else
            {
                //Debug.Log("Hit non-pellet object");
            }
            
        }
        //Debug.Log("No pellet detected.");
        return false;
    }

    private void PortalFunctionality()
    {
        if (PacStudentIsWithinPortalYCoordinates && !hasTeleported)
        {
            if (transform.position.x > teleportRightX)
            {
                if (tweener != null && tweener.TweenExists(transform))
                {
                    tweener.ClearTween(transform);
                }
                transform.position = new Vector3(teleportLeftX, transform.position.y, transform.position.z);
                Debug.Log("Teleported from right to left!");
                hasTeleported = true;
            }
            else if (transform.position.x < teleportLeftX)
            {
                if (tweener != null && tweener.TweenExists(transform))
                {
                    tweener.ClearTween(transform);
                }
                transform.position = new Vector3(teleportRightX, transform.position.y, transform.position.z);
                Debug.Log("Teleported from left to right!");
                hasTeleported = true;
            }
        }

        if (NotInPortalXPosition)
        {
            hasTeleported = false;
        }
        
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Valid"))
        {
            PowerPellet powerPellet = other.GetComponent<PowerPellet>();
            if (powerPellet != null)
            {
                Destroy(other.gameObject);
                TriggerGhostScaredState();
                
            }
            else
            {
                Destroy(other.gameObject);
                ScoreManager.Instance.AddScore(10);
            }
            
            
            
        }

    }

    private void TriggerGhostScaredState()
    {
        foreach (var ghost in ghosts)
        {
            ghost.PlayScaredAnimation();
        }

        BackgroundMusicManager musicManager = FindObjectOfType<BackgroundMusicManager>();
        if (musicManager != null)
        {
            musicManager.PlayGhostScaredMusic(10f);
        }

        StartCoroutine(TriggerGhostTransformState(7f));
        StartCoroutine(ResetGhostScaredState(10f));
    }

    private IEnumerator TriggerGhostTransformState(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (var ghost in ghosts)
        {
            ghost.PlayScaredTransformAnimation();
        }
    }
    private IEnumerator ResetGhostScaredState(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (var ghost in ghosts)
        {
            ghost.PlayNormalAnimation();
        }
    }
    
    private void PlayAudio(bool isEatingPellet)
    {
        if (isEatingPellet)
        {
            if (!pacStudentEatPelletAudio.isPlaying)
            {
                pacStudentAudio.Pause();
                pacStudentEatPelletAudio.Play();
            }
        }
        else
        {
            if (!pacStudentAudio.isPlaying)
            {
                pacStudentEatPelletAudio.Pause();
                pacStudentAudio.Play();
            }
        }
    }
    
    
    

}
