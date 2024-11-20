using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class BackgroundMusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource introMusic, ghostNormalMusic, ghostScaredMusic;
    private AudioSource currentMusic;

    private Coroutine revertMusicCoroutine;
    private bool isScaredMusicPlaying;
    
    //start with the IntroMusic until it finishes. It should not be overidden of the ghostnormal or ghostscared music.
    
    //THEN
    //play Ghostnormal music on loop. (turn on the ghost normal music and turn of ghostscared music, if any
    //plat GhostScared music on loop. (turn on ghost scared music and turn off ghostnormal music, if any
    
    // These functions will be called in the coroutines/functions in the PacStudentController accordingly, changing the background music accordingly. This script will be attached to a gameobject called Background manager.
    //There are 3 audios in total!
    
    // Start is called before the first frame update
    void Start()
    {
        PlayIntroMusic();

    }

    private void PlayIntroMusic()
    {
        if (introMusic != null)
        {
            currentMusic = introMusic;
            introMusic.Play();
            StartCoroutine(TransitionToGhostNormalMusic(introMusic.clip.length));
        }
    }

    private IEnumerator TransitionToGhostNormalMusic(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayGhostNormalMusic();
    }

    public void PlayGhostNormalMusic()
    {
        if (isScaredMusicPlaying) return;
        
        ghostScaredMusic.Stop();
        ghostNormalMusic.loop = true;
        currentMusic = ghostNormalMusic;
        ghostNormalMusic.Play();
    }

    public void PlayGhostScaredMusic(float duration)
    {
        if (isScaredMusicPlaying) return;

        isScaredMusicPlaying = true;
        
        ghostNormalMusic.Stop();
        ghostScaredMusic.loop = true;
        currentMusic = ghostScaredMusic;
        ghostScaredMusic.Play();

        if (revertMusicCoroutine != null)
        {
            StopCoroutine(revertMusicCoroutine);
        }

        revertMusicCoroutine = StartCoroutine(RevertToNormalMusicAfterDelay(duration));

    }

    private IEnumerator RevertToNormalMusicAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        isScaredMusicPlaying = false;
        PlayGhostNormalMusic();
    }
}
