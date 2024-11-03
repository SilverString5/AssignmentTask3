using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class CherryController : MonoBehaviour
{ 
    public float moveSpeed = 3f;
    private Vector3 targetPosition;
    private Vector3 startingPoint;
    private Vector3 center;
    private float area1, area2;
    private bool canBeDestroyed = false;

    public void Initialize(Vector3 centerPosition)
    {
        RandomiseStartingPoint();
        transform.position = startingPoint;
        center = centerPosition;
        Debug.Log("Cherry Initialized, starting LerpToPosition");
        LerpToPosition();

        StartCoroutine(ActivateDestructionCheck(1.5f));
    }
    private void RandomiseStartingPoint() //will be called at each Invoke for SpawnCherry for 10 seconds.
    {
        
        Camera mainCamera = Camera.main;
        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        int side = Random.Range(0, 4); //checking all 4 sides and randomising them!
        switch (side)
        {
            case 0: //Top
                area1 = Random.Range(-camWidth, camWidth);
                area2 = camHeight + 1f;
                break;
            case 1: //Bottom
                area1 = Random.Range(-camWidth, camWidth);
                area2 = -camHeight - 1f;
                break;
            case 2: //Left
                area1 = -camWidth - 1f;
                area2 = Random.Range(-camHeight, camHeight);
                break;
            case 3: //Right
                area1 = camWidth + 1f;
                area2 = Random.Range(-camHeight, camHeight);
                break;
        }

        startingPoint = new Vector3(area1, area2, -1);

    }
    
    private void LerpToPosition()
    {
       
        Vector3 direction = (center - startingPoint).normalized;
        targetPosition = new Vector3(center.x + direction.x * 50f, center.y + direction.y * 50f, startingPoint.z);
        Debug.Log("Starting Lerp to Target Position: " + targetPosition);
        StartCoroutine(MoveCherry());

    }
    
    private IEnumerator MoveCherry()
    {
        Debug.Log("Cherry is moving");
        float timeElapsed = 0f;
        float journeyLength = Vector3.Distance(startingPoint, targetPosition);
        float duration = journeyLength / moveSpeed;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            transform.position = Vector3.Lerp(startingPoint, targetPosition, timeElapsed / duration);
            yield return null;
        }
        
        Destroy(gameObject);

    }

    private IEnumerator ActivateDestructionCheck(float delay)
    {
        yield return new WaitForSeconds(delay);
        canBeDestroyed = true;
    }

    private void Update()
    {
        if (!canBeDestroyed) return;
        Camera mainCamera = Camera.main;
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        if (screenPoint.x < -0.1f || screenPoint.x > 1.1f || screenPoint.y < -0.1f || screenPoint.y > 1.1f)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PacStudent"))
        {
            Destroy(gameObject);
            ScoreManager.Instance.AddScore(100);
        }
    }
    
    
}
