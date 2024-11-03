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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadFirstLevel()
    {
        
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene(1);
        SceneManager.sceneLoaded += OnSceneLoaded;
        //ShowLoadingScreen();
        //StartCoroutine(LoadSceneAsync(1));
        
    }
    public void QuitGame()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene(0);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        if (scene == SceneManager.GetSceneByBuildIndex(1)){
            hudScoreObject = GameObject.Find("Score");
            if (hudScoreObject)
            {
                hudScoreText = pacStudentGameObject.GetComponent<Text>();
            }
            
        }
        
    }
}
