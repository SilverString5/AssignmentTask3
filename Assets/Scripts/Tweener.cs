using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    //private Tween activeTween;
    private List<Tween> activeTweens;
    // Start is called before the first frame update
    void Start()
    {
        activeTweens = new List<Tween>();

    }

    // Update is called once per frame
    void Update()
    {
        List<Tween> matches = new List<Tween>();
        foreach (Tween tween in activeTweens)
        {
            if (tween != null)
            {
                if (Vector3.Distance(tween.Target.position, tween.EndPos) > 0.1f)
                {
                    //Time.time - StartPos
                    float timeFraction = (Time.time - tween.StartTime) / tween.Duration;
                    //timeFraction = Mathf.Pow(timeFraction, 3);
                    tween.Target.position = Vector3.Lerp(tween.StartPos, tween.EndPos, timeFraction);
                }
                else
                {
                    tween.Target.position = tween.EndPos;
                    matches.Add(tween);
                    //activeTween = null;
                    
                    //activeTweens.Remove(tween);
                }
            }
            
        }
        foreach (Tween match in matches)
        {
            activeTweens.Remove(match);
        }

        
        
        
    }

    public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
        if (!TweenExists(targetObject))
        {
            Debug.Log("Adding new tween");
            Tween tween = new Tween(targetObject, startPos, endPos, Time.time, duration);
            activeTweens.Add(tween);
            return true;
        }
        return false;
    }

    public bool TweenExists(Transform target)
    {
        foreach (Tween tween in activeTweens)
        {
            if (tween.Target == target)
            {
                return true;
            }
        }
        return false;
    }

    public void ClearTween(Transform target)
    {
        activeTweens.RemoveAll(tween => tween.Target == target);
    }
    
}
