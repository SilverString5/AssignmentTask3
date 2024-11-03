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
        StartCoroutine(SpawnCherryRoutine());
    }

    private IEnumerator SpawnCherryRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnCherry();
        }
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
