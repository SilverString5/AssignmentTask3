using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CherryController : MonoBehaviour
{ 
    public float moveSpeed = 3f;
    private Vector3 targetPosition;
    private Vector3 startingPoint;
    private Vector3 center;
    private float area1, area2;

    public void Initialize(Vector3 centerPosition)
    {
        RandomiseStartingPoint();
        transform.position = startingPoint;
        center = centerPosition;
        LerpToPosition();
    }
    private void RandomiseStartingPoint() //will be called at each Invoke for SpawnCherry for 10 seconds.
    {
        //area1 = Random.Range(Outside camera, outside camera view) -> x value
        //area2 = Random.Range(Outside camera, outside camera view) -> y value
        //should be random location JUST OUTSIDE of the camera view on any side of the level.
        //Z will just be 0.
        
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

        startingPoint = new Vector3(area1, area2, 0);

    }
    
    private void LerpToPosition()
    {
       
        Vector3 direction = (center - startingPoint).normalized;
        targetPosition = center + direction * 50f;

        StartCoroutine(MoveCherry());

    }
    
    private IEnumerator MoveCherry()
    {
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


}
