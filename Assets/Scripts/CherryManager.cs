using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CherryManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject cherryPrefab;
    public float spawnInterval = 10f;
    public Transform canvasCentre;
    
    void Start()
    {
        InvokeRepeating(nameof(SpawnCherry), 0f, spawnInterval);
    }
    

    private void SpawnCherry()
    {
        GameObject newCherry = Instantiate(cherryPrefab);
        CherryController cherryController = newCherry.GetComponent<CherryController>();
        if (cherryController != null)
        {
            cherryController.Initialize(canvasCentre.position);//This line here
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
