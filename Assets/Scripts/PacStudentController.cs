using System;
using System.Collections;
using System.Collections.Generic;
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
    public AudioSource pacStudentAudio;
    public AudioSource pacStudentEatPelletAudio;
    public AudioSource pacStudentCollisionSound;
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
    
    
    
    // Start is called before the first frame update
    void Start()
    {
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
        if (Input.GetKeyDown(KeyCode.W))
        {
            playerInput = "w";
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            playerInput = "s";
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            playerInput = "d";
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            playerInput = "a";
        }
        else
        {
            playerInput = null;
        }
    }
    
    private void updateLastInput()
    {
        if (playerInput != lastInput && playerInput != null)
        {
            lastInput = playerInput; //changed playerInput overrides last input.
        }
    }
    
    //add valid tag (invalid tag OR valid tag) if valid tag (if lastInput is valid), then change the currentInput then lerp to grid position
    //check adjacent tiles (what game objects are within 1 unit) //if the game object is within 1 unit
    private void updateCurrentInput()
    {
        
        
        if (checkInputValidity(lastInput) && currentInput != lastInput)
        {
            currentInput = lastInput;
            MoveDirection();
            lastInput = null;
        }
        else if (checkInputValidity(currentInput))
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
        
        if (currentInput != null || lastInput != null)
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

                if (IsPelletInDirection(direction))
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
            if (IsWallInDirection(Vector2.up))
            { 
                return false;
            }
            return true;
        }
        if (value == "a")
        {
            if (IsWallInDirection(Vector2.left))
            {
                return false;
            }
            return true;
            
        }
        
        if (value == "s")
        {
            if (IsWallInDirection(Vector2.down))
            {
                return false;
            }
            return true;
        }
        
        if (value == "d")
        {
            if (IsWallInDirection(Vector2.right))
            {
                return false;
            }
            return true;
        }
        return false;
    }

    private bool IsPelletInDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 2f); // Short ray
        if (hit.collider != null && hit.collider.CompareTag("Valid"))
        {
            Debug.Log("Pellet detected in direction: " + direction);
            return true;
        }

        return false;
    }

    private void PortalFunctionality()
    {
        if (transform.position.y >= tunnelMinY && transform.position.y <= tunnelMaxY && !hasTeleported)
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

        if (transform.position.x > teleportLeftX && transform.position.x < teleportRightX)
        {
            hasTeleported = false;
        }
        
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Valid"))
        {
            Destroy(other.gameObject);
            ScoreManager.Instance.AddScore(10);

            pacStudentEatPelletAudio.Play();
            pacStudentAudio.Pause();
        }

    }
    
    /*
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Valid"))
        {
            // Resume movement audio if PacStudent leaves the pellet collider
            pacStudentAudio.Play();
            pacStudentEatPelletAudio.Pause();
        }
    }
    */
    

}
