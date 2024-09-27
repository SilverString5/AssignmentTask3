using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private GameObject pacStud;
    [SerializeField] private Animator pacAnim;
    private Tweener tweener;
    private Vector3 location1;
    private Vector3 location2;
    private Vector3 location3;
    private Vector3 location4;
    

    // Start is called before the first frame update
    void Start()
    {
        location4 = new Vector3(-4.57f, 8.44f, -1.0f);
        location1 = new Vector3(0.42f, 8.44f, -1.0f);
        location2 = new Vector3(0.42f, 4.43f, -1.0f);
        location3 = new Vector3(-4.57f, 4.43f, -1.0f);
        tweener = GameObject.FindFirstObjectByType<Tweener>();

    }

    // Update is called once per frame
    void Update()
    {
        if (pacStud.transform.position == location1)
        {
            pacAnim.SetInteger("Param", 0);
            tweener.AddTween(pacStud.transform, pacStud.transform.position, location2, 0.9f);
        }

        if (pacStud.transform.position == location2)
        {
            pacAnim.SetInteger("Param", 1);
            tweener.AddTween(pacStud.transform, pacStud.transform.position, location3, 0.9f);
        }

        if (pacStud.transform.position == location3)
        {
            pacAnim.SetInteger("Param", 2);
            tweener.AddTween(pacStud.transform, pacStud.transform.position, location4, 0.9f);
        }


        if (pacStud.transform.position == location4)
        {
            tweener.AddTween(pacStud.transform, pacStud.transform.position, location1, 0.9f);
            pacAnim.SetInteger("Param", 3);
        } 
            



    }
}
