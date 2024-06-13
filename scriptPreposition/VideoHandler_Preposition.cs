using Prepostion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class VideoHandler_Preposition : MonoBehaviour
{
    VideoPlayer _videoPlayer;
    int totalframe;
    int currentframe;
    public GameObject loading;
    // Start is called before the first frame update
    void OnEnable()
    {
        
        _videoPlayer = GetComponent<VideoPlayer>();
        //_videoPlayer.clip = UIManager_Preposition.instance.GameVideo[UIManager_Preposition.instance.current_level-1];
        _videoPlayer.Play();
        _videoPlayer.loopPointReached += OnMovieFinished;
        _videoPlayer.prepareCompleted += OnMoviedoPrepared;
    }

    void OnMoviedoPrepared(VideoPlayer player)
    {
        // print("Prepare");
      //  loading.SetActive(false);
    }
    private void Update()
    {

    }



    public void SkipVideo()
    {


        _videoPlayer.Stop();
        transform.parent.gameObject.SetActive(false);
        RenderTexture.active = _videoPlayer.targetTexture;
        GL.Clear(true, true, Color.black);
        RenderTexture.active = null;
    }


    void OnMovieFinished(VideoPlayer player)
    {

        SkipVideo();
        //Debug.Log("Event for movie end called");
    }
}
