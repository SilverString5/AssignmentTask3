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
    public static UIManager Instance;
    
    
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
        
        if (scene == SceneManager.GetSceneByBuildIndex(1)){
            hudScoreObject = GameObject.Find("Score");
            if (hudScoreObject != null)
            {
                hudScoreText = hudScoreObject.GetComponent<Text>();
                if (hudScoreText != null)
                {
                    UpdateScoreUI(ScoreManager.Instance.GetScore());
                }
                else
                {
                    Debug.LogWarning("hudScoreText component not found on Score GameObject.");
                }
                
            }
            else
            {
                Debug.LogWarning("hudScoreObject not found in the scene");
            }
            
            
        }
        
        
    }
    
    
    public void UpdateScoreUI(int newScore)
    {
        if (hudScoreText == null)
        {
            hudScoreObject = GameObject.Find("Score");
            if (hudScoreObject)
            {
                hudScoreText = hudScoreObject.GetComponent<Text>();
            }
        }
        if (hudScoreText != null)
        {
            hudScoreText.text = "Score: " + newScore;
        }
        else
        {
            Debug.LogWarning("hudScoreText is still null");
        }
    }
    
}
