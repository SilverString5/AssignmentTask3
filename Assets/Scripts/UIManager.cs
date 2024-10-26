using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameObject buttonlevel1Object;
    private Button button1;
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
        //SceneManager.sceneLoaded += OnSceneLoaded;
        //ShowLoadingScreen();
        //StartCoroutine(LoadSceneAsync(1));
        
    }
    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        //if (scene == SceneManager.GetSceneByBuildIndex(1))
        //{
            //buttonlevel1Object = GameObject.FindGameObjectWithTag("button1");
            //if (buttonlevel1Object)
            //{
                //button1 = buttonlevel1Object.GetComponent<Button>();
                //button1.onClick.AddListener(QuitGame);
            //}


        //}
        
    }
}
