using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject buttonlevel1Object;
    private Button button1;
    private GameObject pacStudentGameObject;
    private Animator pacStudentAnimator;
    private Transform pacStudentTransform;
    private GameObject hudScoreObject;
    private Text hudScoreText;
    private GameObject hudTimerObject;
    private Text hudTimertext;
    //public static UIManager Instance;
    
    /*
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    */
    

    public void LoadFirstLevel()
    {
        
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene(1);
        SceneManager.sceneLoaded += OnSceneLoaded;
       
        
    }
    public void QuitGame()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene(0);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        /*
        if (scene == SceneManager.GetSceneByBuildIndex(1)){
            hudScoreObject = GameObject.Find("Score");
            if (hudScoreObject)
            {
                hudScoreText = pacStudentGameObject.GetComponent<Text>();
                UpdateScoreUI(ScoreManager.Instance.GetScore());
                
            }
            
        }
        */
        
    }
    
    /*
    public void UpdateScoreUI(int newScore)
    {
        if (hudScoreText != null)
        {
            hudScoreText.text = "Score: " + newScore;
        }
    }
    */
}
